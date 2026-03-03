using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Authentication;
using CbsAp.Application.Configurations;
using CbsAp.Application.Features.Authentication.Common;
using CbsAp.Application.Shared.ResultPatten;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CbsAp.Application.Features.Authentication.Queries
{
    public class LoginQueryHandler : IQueryHandler<LoginQuery, ResponseResult<AuthenticationTokenResult>>
    {
        private readonly ILogger<LoginQueryHandler> _logger;

        private readonly IAuthenticationJwtService _authenticationJwtService;

        private readonly IUserService _userService;
        private readonly IRoleManagementRepository _roleRepository;
        private readonly IPermissionManagementRepository _permissionManagementRepository;

        private readonly IUserAuthService _userAuthService;
        private readonly AuthConfig _options;

        public LoginQueryHandler(

            IAuthenticationJwtService authenticationJwtService,
            IUserService userService,
            IUserAuthService userAuthService,
            IRoleManagementRepository roleRepository,
            IPermissionManagementRepository permissionManagementRepository,
             IOptions<AuthConfig> options,
            ILogger<LoginQueryHandler> logger)
        {
            _authenticationJwtService = authenticationJwtService;
            _userService = userService;
            _userAuthService = userAuthService;
            _options = options.Value;
            _logger = logger;
            _roleRepository = roleRepository;
            _permissionManagementRepository = permissionManagementRepository;
        }

        public async Task<ResponseResult<AuthenticationTokenResult>> Handle(
            LoginQuery request,
            CancellationToken cancellationToken)
        {
            var existUser = await _userService.userExist(request.Username);
            var isnewUser = await _userAuthService.IsNewUser(request.Username);

            if (!existUser || isnewUser)
            {
                _logger.LogError("Unauthorized or unconfirmed user: {Username}", request.Username);
                return ResponseResult<AuthenticationTokenResult>.Unauthorized("Unauthorized user");
            }

            var user = await _userAuthService.GetUserAccountByUserName(request.Username);
            if (user == null)
            {
                return ResponseResult<AuthenticationTokenResult>.Unauthorized("Unauthorized user");
            }

            if (user.IsLockedOut)
            {
                _logger.LogWarning("Locked out user attempted login: {Username}", request.Username);
                return ResponseResult<AuthenticationTokenResult>.Unauthorized("Account locked. Please contact your administrator.");
            }

            // Check reset password abuse
            var resetWithoutLoginCount = await _userAuthService.GetConsecutiveResetWithoutLoginCount(user.UserAccountID);
            if (resetWithoutLoginCount >= 4)
            {
                user.IsLockedOut = true;
                user.LockoutEndUtc = DateTime.UtcNow;
                await _userAuthService.UpdateUserLogInfoAsync(user, cancellationToken);
                _logger.LogWarning("User locked due to excessive resets without login: {Username}", request.Username);
                return ResponseResult<AuthenticationTokenResult>.Unauthorized("Account is locked. Please contact your administrator.");
            }

            var isAuthenticated = await _userAuthService.Authenticate(request.Username, request.Password);

            if (!isAuthenticated)
            {
                user.FailedLoginAttempts++;
                
                if (user.FailedLoginAttempts >= _options.MaxLoginAttempts)
                {
                    user.IsLockedOut = true;
                    user.LockoutEndUtc = DateTime.UtcNow;
                    _logger.LogError("User locked due to login failures: {Username}", request.Username);
                }

                await _userAuthService.UpdateUserLogInfoAsync(user, cancellationToken);
                return ResponseResult<AuthenticationTokenResult>.Unauthorized("Unauthorized user");
            }

            // Successful Login
            user.FailedLoginAttempts = 0;
            user.IsLockedOut = false;
            user.LastLoginDateTime = DateTime.UtcNow;
            await _userAuthService.UpdateUserLogInfoAsync(user, cancellationToken);

            // Reset "reset without login" flag
            await _userAuthService.AddPasswordResetAuditAsync(user.UserAccountID, true, cancellationToken);

            var roles = await _roleRepository.GetActiveRoleByUserAsync(request.Username);
            //todo: get default role; for now get the first role in the list
            var selectedRole = roles.Any() ? roles.ToArray()[0] : null;
            long roleId = selectedRole == null ? 0 : selectedRole.RoleID;
            var permissions = _permissionManagementRepository.GetPermissionOperationsByRole(roleId);
            var permissionActions = permissions.Select(p => p.ActionName).ToArray();
            var authorisationLimit = selectedRole?.AuthorisationLimit ?? 0;
            var token = _authenticationJwtService.GenerateUserJwtToken(request.Username,roleId.ToString(), authorisationLimit, permissionActions);
            var result = new AuthenticationTokenResult(request.Username, token);

            return ResponseResult<AuthenticationTokenResult>.OK(result, "Token successfully generated");
        }
    }
}