using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Authentication;
using CbsAp.Application.Features.Authentication.Common;
using CbsAp.Application.Shared.Encryption;
using CbsAp.Application.Shared.ResultPatten;
using MediatR;

namespace CbsAp.Application.Features.Authentication.Commands.SwitchRole
{
    public class SwitchRoleCommandHandler : ICommandHandler<SwitchRoleCommand, ResponseResult<AuthenticationTokenResult>>
    {
        private readonly IUserAuthService _userAuthService;
        private readonly IRoleManagementRepository _roleRepository;
        private readonly IPermissionManagementRepository _permissionManagementRepository;
        private readonly IAuthenticationJwtService _authenticationJwtService;

        public SwitchRoleCommandHandler(IUserAuthService userAuthService, IRoleManagementRepository roleRepository,IAuthenticationJwtService authenticationJwtService, IPermissionManagementRepository permissionManagementRepository)
        {
            _userAuthService = userAuthService;
            _roleRepository = roleRepository;
            _authenticationJwtService = authenticationJwtService;   
            _permissionManagementRepository = permissionManagementRepository;
        }

        public async Task<ResponseResult<AuthenticationTokenResult>> Handle(SwitchRoleCommand command, CancellationToken cancellationToken)
        {
            var selectedRole = await _roleRepository.GetUserRoleAsync(command.UserName, command.RoleID);
            long roleId = selectedRole == null ? 0 : selectedRole.RoleID;
            var permissions = _permissionManagementRepository.GetPermissionOperationsByRole(roleId);
            var permissionActions = permissions.Select(p => p.ActionName).ToArray();
            var authorisationLimit = selectedRole?.AuthorisationLimit ?? 0;
            var token = _authenticationJwtService.GenerateUserJwtToken(command.UserName, roleId.ToString(), authorisationLimit, permissionActions);
            var result = new AuthenticationTokenResult(command.UserName, token);

            return ResponseResult<AuthenticationTokenResult>.OK(result, "Token successfully generated");
        }
    }
}