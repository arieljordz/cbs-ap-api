using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.NotificationStrategy;
using CbsAp.Application.Abstractions.NotificationStrategy.NotificationContext;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Authentication;
using CbsAp.Application.Configurations;
using CbsAp.Application.DTOs.UserAuthentication;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CbsAp.Application.Features.Users.Commands.LockUnlockUser;

public class LockUnlockUserCommandHandler : ICommandHandler<LockUnlockUserCommand, ResponseResult<string>>
{
    private readonly IUserManagementRepository _userRepo;
    private readonly IUnitofWork _unitOfWork;
    private readonly IUserAuthService _authService;
    private readonly INotificationContext _notificationContext;
    private readonly ILogger<LockUnlockUserCommandHandler> _logger;
    private readonly AppSettings _appSettings;

    public LockUnlockUserCommandHandler(
        IUserManagementRepository userRepo,
        IUnitofWork unitOfWork,
        IUserAuthService authService,
        INotificationContext notificationContext,
        IOptions<AppSettings> options,
        ILogger<LockUnlockUserCommandHandler> logger)
    {
        _userRepo = userRepo;
        _unitOfWork = unitOfWork;
        _authService = authService;
        _notificationContext = notificationContext;
        _logger = logger;
        _appSettings = options.Value;
    }

    public async Task<ResponseResult<string>> Handle(LockUnlockUserCommand request, CancellationToken cancellationToken)
    {
        var user = await GetUserWithLogInfo(request.UserAccountID, cancellationToken);
        if (user == null)
        {
            return ResponseResult<string>.NotFound("User not found.");
        }

        UpdateUserLockStatus(user, request.IsLockedOut);

        if (!request.IsLockedOut)
        {
            var (IsSuccess, Response) = await HandleUnlockFlowAsync(user, cancellationToken);
            if (!IsSuccess)
                return Response;
        }

        var saveSuccess = await _unitOfWork.SaveChanges(cancellationToken);
        if (!saveSuccess)
        {
            return ResponseResult<string>.BadRequest("Error updating user lock status.");
        }

        var action = request.IsLockedOut ? "locked" : "unlocked";
        return ResponseResult<string>.OK($"User {user.UserID} has been successfully {action}.");
    }

    private async Task<UserAccount?> GetUserWithLogInfo(long userAccountId, CancellationToken cancellationToken)
    {
        return await _userRepo.GetUserAccountAsQueryable()
            .Include(u => u.UserLogInfo)
            .FirstOrDefaultAsync(u => u.UserAccountID == userAccountId, cancellationToken);
    }

    private void UpdateUserLockStatus(UserAccount user, bool isLocked)
    {
        user.UserLogInfo.IsLockedOut = isLocked;
        if (isLocked)
        {
            user.UserLogInfo.LockoutEndUtc = DateTime.UtcNow;
        }
        else
        {
            user.UserLogInfo.LockoutEndUtc = null;
            user.UserLogInfo.FailedLoginAttempts = 0;
        }
    }

    private async Task<(bool IsSuccess, ResponseResult<string> Response)> HandleUnlockFlowAsync(UserAccount user, CancellationToken cancellationToken)
    {
        var token = Guid.NewGuid().ToString();

        user.UserLogInfo.PasswordrecoveryToken = token;
        user.UserLogInfo.RecoveryTokenTime = DateTimeOffset.UtcNow.AddHours(1);
        user.UserLogInfo.IsPasswordRecoveryTokenUsed = false;

        var bindData = CreateEmailBindData(user.FirstName, token, user.UserLogInfo.RecoveryTokenTime);

        var strategy = _notificationContext.GetNotificationTypeStrategy(NotificationType.ForgotPasswordNotification);
        var emailSent = await strategy.SendNotificationAsync(user.EmailAddress, bindData);

        if (!emailSent)
        {
            _logger.LogError("Failed to send password reset email to: {Email}", user.EmailAddress);
            return (false, ResponseResult<string>.BadRequest("An email has been sent to reset your password."));
        }

        return (true, ResponseResult<string>.OK("An email has been sent to reset your password."));
    }

    private Dictionary<string, string> CreateEmailBindData(string firstName, string token, DateTimeOffset? expiration)
    {
        return new Dictionary<string, string>
        {
            { "UserName", firstName },
            { "ResetLink", $"{_appSettings.WebUrl}auth/cbsap-snp/{token}" },
            { "ExpirationDateTime", expiration?.ToLocalTime().ToString("dd/MM/yyyy hh:mm:ss tt") ?? "N/A" }
        };
    }
}
