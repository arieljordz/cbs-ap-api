using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.NotificationStrategy;
using CbsAp.Application.Abstractions.NotificationStrategy.NotificationContext;
using CbsAp.Application.Abstractions.Services.Authentication;
using CbsAp.Application.Configurations;
using CbsAp.Application.DTOs.UserAuthentication;
using CbsAp.Application.Shared.ResultPatten;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CbsAp.Application.Features.Authentication.Commands.ForgotPassword
{
    public class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand, ResponseResult<bool>>
    {
        private readonly INotificationContext _notificationContext;

        private readonly IUserAuthService _userAuthService;

        private readonly AppSettings _options;

        private readonly ILogger<ForgotPasswordCommandHandler> _logger;

        private readonly int _resetPasswordLimitPerDay;

        private readonly int _resetPasswordLimitWithoutLogin;

        public ForgotPasswordCommandHandler(
            INotificationContext notificationContext,
            IUserAuthService userAuthService,
            IOptions<AppSettings> option,
            ILogger<ForgotPasswordCommandHandler> logger)
        {
            _notificationContext = notificationContext;
            _userAuthService = userAuthService;
            _options = option.Value;
            _logger = logger;
            _resetPasswordLimitPerDay = _options.RessetPasswordLimitPerDay;
            _resetPasswordLimitWithoutLogin = _options.RessetPasswordLimitWithoutLogin;
        }

        public async Task<ResponseResult<bool>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var userAccount = await _userAuthService.GetUserAccountByEmailAddress(request.EmailAddress);
            if (userAccount == null || string.IsNullOrWhiteSpace(userAccount.EmailAddress))
            {
                _logger.LogError("Password reset requested for non-existent or invalid email: {EmailAddress}", request.EmailAddress);
                return ResponseResult<bool>.OK("An email has been sent to reset your password.");
            }

            // Enforce: max 3 resets without login
            var resetWithoutLogin = await _userAuthService.GetConsecutiveResetWithoutLoginCount(userAccount.UserAccountID);
            if (resetWithoutLogin >= _resetPasswordLimitWithoutLogin)
            {
                // Lock the user
                userAccount.UserLogInfo.IsLockedOut = true;
                userAccount.UserLogInfo.LockoutEndUtc = DateTime.UtcNow;
                await _userAuthService.UpdateUserLogInfoAsync(userAccount.UserLogInfo, cancellationToken);

                _logger.LogWarning("User {UserId} is locked due to too many resets without login.", userAccount.UserID);
                return ResponseResult<bool>.BadRequest("Account is locked. Please contact your administrator.");
            }

            // Enforce: max 2 resets per day
            var resetToday = await _userAuthService.GetDailyPasswordResetCount(userAccount.UserAccountID);
            if (resetToday >= _resetPasswordLimitPerDay)
            {
                _logger.LogWarning("User {UserId} exceeded daily reset limit.", userAccount.UserID);
                return ResponseResult<bool>.BadRequest("You have reached the maximum password reset requests allowed per day. Please contact your administrator or wait for 24 hours to reset your password again.");
            }

            var guidToken = Guid.NewGuid().ToString();
            var resetPasswordDTO = CreateForgotPasswordDTO(guidToken);

            var savePasswordReset = await
                SavePasswordReset(userAccount.UserAccountID, resetPasswordDTO, cancellationToken, request.EmailAddress);

            if (!savePasswordReset.IsSave)
            {
                _logger.LogError("Failed to save password reset request for user: {UserId}", userAccount.UserID);
                return ResponseResult<bool>.OK("An email has been sent to reset your password.");
            }

            var bindData = new Dictionary<string, string>
                {
                    { "UserName", userAccount.FirstName },
                    { "ResetLink", $"{_options.WebUrl}auth/cbsap-snp/{guidToken}" },
                    { "ExpirationDateTime", $"{savePasswordReset.expiration?.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss tt")}" }
                };

            var isEmailSent = await SendPasswordResetEmail(request.EmailAddress, bindData, userAccount.UserAccountID, cancellationToken);
            if (!isEmailSent)
            {
                _logger.LogError("Failed to send password reset email to: {Email}", userAccount.EmailAddress);
                return ResponseResult<bool>.BadRequest("An email has been sent to reset your password.");
            }

            await _userAuthService.AddPasswordResetAuditAsync(userAccount.UserAccountID, false, cancellationToken);

            return ResponseResult<bool>.OK("An email has been sent to reset your password.");
        }

        private static ForgotPasswordDto CreateForgotPasswordDTO(string token)
        {
            return new ForgotPasswordDto
            {
                IsPasswordRecoveryTokenUsed = false,
                PasswordrecoveryToken = token,
                RecoveryTokenTime = DateTimeOffset.UtcNow.AddHours(1)
            };
        }

        private async Task<(bool IsSave, DateTimeOffset? expiration)> SavePasswordReset(
            long userAccountID,
            ForgotPasswordDto resetPasswordDTO,
            CancellationToken cancellationToken,
            string emailAddress)
        {
            var savePasswordReset =
                await _userAuthService.UpdatePasswordReset(
                userAccountID,
                resetPasswordDTO,
                cancellationToken);

            if (!savePasswordReset.isSave)
            {
                _logger.LogError("Failed to save password reset for email: {Email}", emailAddress);
            }
            return (savePasswordReset.isSave, savePasswordReset.expiration);
        }

        private async Task<bool> IsPasswordRecoveryTokenTime(long userAccountID)
        {
            return await _userAuthService.IsPasswordRecoveryTokenTime(userAccountID);
        }

        private async Task<bool> SendPasswordResetEmail(
            string emailAddress,
            Dictionary<string, string> bindData,
            long userAccountID,
            CancellationToken cancellationToken)
        {
            var notificationStrategy =
                _notificationContext.GetNotificationTypeStrategy(NotificationType.ForgotPasswordNotification);
            var isEmailSent = await notificationStrategy.SendNotificationAsync(emailAddress, bindData);

            if (!isEmailSent)
            {
                var invalidatePasswordResetDTO = InValidatePasswordReset();
                var invalidatePasswordReset = await _userAuthService.UpdatePasswordReset(
                    userAccountID,
                    invalidatePasswordResetDTO,
                    cancellationToken);

                if (!invalidatePasswordReset.isSave)
                {
                    _logger.LogError("Failed to invalidate password reset in DB for email: {Email}", emailAddress);
                }

                _logger.LogError("Failed to send password recovery email to '{Email}'.", emailAddress);
            }

            return isEmailSent;
        }

        private static ForgotPasswordDto InValidatePasswordReset()
        {
            return new ForgotPasswordDto
            {
                IsPasswordRecoveryTokenUsed = null,
                PasswordrecoveryToken = null,
                RecoveryTokenTime = null
            };
        }
    }
}