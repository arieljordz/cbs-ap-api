using CbsAp.Application.DTOs.UserAuthentication;
using CbsAp.Domain.Entities.UserManagement;

namespace CbsAp.Application.Abstractions.Services.Authentication
{
    public interface IUserAuthService
    {
        Task<bool> Authenticate(string username, string password);

        Task<bool> IsNewUser(string username);

        Task<UserAccount?> GetUserAccountByEmailAddress(string emailAddress);
        Task<UserLogInfo?> GetUserAccountByUserName(string username);

        Task<(bool isSave, DateTimeOffset? expiration)> UpdatePasswordReset(
            long userAccountID,
            ForgotPasswordDto userInfoDTO,
            CancellationToken cancellationToken);

        Task<bool> IsPasswordRecoveryTokenTime(long userAccountID);

        Task<UserLogInfo> GetUserLogInfo(string passwordRecoveryToken);

        Task<(UserLogInfo userLogInfo, bool verifiedTempPass)> GetActivateUserToken(string confirmationToken, string tempPassword);

        Task<bool> SetNewPasswordUpdate(UserLogInfo userInfoDTO, bool forUserActivation, CancellationToken cancellationToken);

        Task<bool> GetPasswordHistories(long userAccountID, string newPassword, CancellationToken cancellationToken);

        Task<string> GetThumbnailName(string userName);

        Task<bool?> UpdateUserLogInfoAsync(UserLogInfo userLogInfo, CancellationToken cancellationToken);
        Task<int> GetDailyPasswordResetCount(long userId);
        Task<int> GetConsecutiveResetWithoutLoginCount(long userId);
        Task AddPasswordResetAuditAsync(long userAccountId, bool isLoginSuccessful, CancellationToken cancellationToken);
    }
}
