using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Authentication;
using CbsAp.Application.DTOs.UserAuthentication;
using CbsAp.Application.Shared.Encryption;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Domain.Entities.UserManagement;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Services.User
{
    public class UserAuthenticationService : IUserAuthService
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IHasher _hasher;
        private readonly IMapper _mapper;

        public UserAuthenticationService(IUnitofWork unitofWork, IHasher hasher, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _hasher = hasher;
            _mapper = mapper;
        }

        public async Task<bool> Authenticate(string username, string password)
        {
            var filteredUser = await _unitofWork.GetRepository<UserLogInfo>()
                .ApplyPredicateAsync(u => u.UserID == username);

            var user = await filteredUser.FirstOrDefaultAsync();
            if (user == null) return false;
            byte[] storedSalt = Convert.FromBase64String(user.PasswordSalt!);

            var authenticate = _hasher.VerifyPassword(password, user.PasswordHash!, storedSalt);

            return authenticate;
        }

        public async Task<UserAccount?> GetUserAccountByEmailAddress(string emailAddress)
        {
            var filteredQuery =
                await _unitofWork.GetRepository<UserAccount>()
                .ApplyPredicateAsync(u => u.EmailAddress == emailAddress);
            var userAccount = await filteredQuery.FirstOrDefaultAsync();
            return userAccount;
        }

        public async Task<bool> IsPasswordRecoveryTokenTime(long userAccountID)
        {
            DateTimeOffset currentUtcTime = DateTimeOffset.UtcNow;
            var filterQuerUserInfo = await _unitofWork.GetRepository<UserLogInfo>()
                .ApplyPredicateAsync(u => u.UserAccountID == userAccountID);

            var userLogInfo = await filterQuerUserInfo.FirstOrDefaultAsync();

            if (userLogInfo?.RecoveryTokenTime != null)
            {
                var isPassrecoveryTokenUsed = userLogInfo.IsPasswordRecoveryTokenUsed.GetValueOrDefault()!;

                //return that the user is not expired
                return (currentUtcTime < userLogInfo.RecoveryTokenTime && !isPassrecoveryTokenUsed);
            }

            return false;
        }

        public async Task<bool> IsNewUser(string username)
        {
            var filteredUser = await _unitofWork.GetRepository<UserLogInfo>()
                .ApplyPredicateAsync(u => u.UserID == username && u.ActivateNewUser);

            var user = await filteredUser.SingleOrDefaultAsync();

            return !string.IsNullOrEmpty(user?.UserID);
        }

        public async Task<(bool isSave, DateTimeOffset? expiration)> UpdatePasswordReset(
            long userAccountID,
            ForgotPasswordDto userInfoDTO,
            CancellationToken cancellationToken)
        {
            UserLogInfo? getUserAccountLogInfo = await GetUserLogInfo(userAccountID);
            var isSave = false;
            DateTimeOffset? expiration = null;

            if (getUserAccountLogInfo != null)
            {
                var setValue = _mapper.Map(userInfoDTO, getUserAccountLogInfo);

                getUserAccountLogInfo.SetAuditFieldsOnUpdate("self (forgot password reset)");

                await _unitofWork.GetRepository<UserLogInfo>().UpdateAsync(getUserAccountLogInfo.UserLogInfoID, setValue);
                isSave = await _unitofWork.SaveChanges(string.Empty, string.Empty, cancellationToken);

                return (isSave, setValue.RecoveryTokenTime);
            }
            return (false, expiration);
        }

        private async Task<UserLogInfo?> GetUserLogInfo(long userAccountID)
        {
            var filteredUserInfo = await _unitofWork.GetRepository<UserLogInfo>()
                .ApplyPredicateAsync(u => u.UserAccountID == userAccountID);

            return await filteredUserInfo.SingleOrDefaultAsync();
        }

        public async Task<bool> SetNewPasswordUpdate(UserLogInfo userInfoDTO, bool forUserActivation, CancellationToken cancellationToken)
        {
            var getUserAccountLogInfo = await GetUserLogInfo(userInfoDTO.UserAccountID);

            if (getUserAccountLogInfo != null)
            {
                userInfoDTO.SetAuditFieldsOnUpdate("self (set new - password)");

                userInfoDTO.IsPasswordRecoveryTokenUsed = true;

                if (forUserActivation)
                {
                    await _unitofWork.GetRepository<UserAccount>().UpdateAsync(userInfoDTO.UserAccountID, userInfoDTO.UserAccount);
                }
                await _unitofWork.GetRepository<UserLogInfo>().UpdateAsync(userInfoDTO.UserLogInfoID, userInfoDTO);

                var savePasswordHistory = new PasswordHistory
                {
                    UserAccountID = userInfoDTO.UserAccountID,
                    PasswordHash = userInfoDTO.PasswordHash,
                    PasswordSalt = userInfoDTO.PasswordSalt,
                    CreatedAt = userInfoDTO?.LastUpdatedDate!
                };
                await _unitofWork.GetRepository<PasswordHistory>().AddAsync(savePasswordHistory);

                return await _unitofWork.SaveChanges(string.Empty, string.Empty, cancellationToken);
            }
            return false;
        }

        public async Task<UserLogInfo> GetUserLogInfo(string passwordRecoveryToken)
        {
            var filteredUserLogInfo = await _unitofWork.GetRepository<UserLogInfo>()
                .ApplyPredicateAsync(ul =>
                                        ul.PasswordrecoveryToken == passwordRecoveryToken
                                        && ul.IsPasswordRecoveryTokenUsed == false
                                        && DateTimeOffset.UtcNow < ul.RecoveryTokenTime);

            return await filteredUserLogInfo.SingleOrDefaultAsync() ?? new UserLogInfo();
        }

        public async Task<List<PasswordHistory>> GetPasswordHistories(long userAccountID, CancellationToken cancellationToken)
        {
            var passwordHistory = await _unitofWork.GetRepository<PasswordHistory>()
                .ApplyPredicateAsync(ph => ph.UserAccountID == userAccountID);

            var result = await passwordHistory.OrderByDescending(ph => ph.CreatedAt)
                .Take(24)
                .ToListAsync();
            return result;
        }

        public async Task<bool> GetPasswordHistories(long userAccountID, string newPassword, CancellationToken cancellationToken)
        {
            var passwordHistory = await _unitofWork.GetRepository<PasswordHistory>()
                .ApplyPredicateAsync(ph => ph.UserAccountID == userAccountID);

            var history = await passwordHistory.OrderByDescending(ph => ph.CreatedAt)
                .Take(24)
                .ToListAsync();

            bool isHash = false;
            foreach (var old in history)
            {
                byte[] storedSalt = Convert.FromBase64String(old.PasswordSalt!);
                isHash = _hasher.VerifyPassword(newPassword, old.PasswordHash!, storedSalt);

                if (isHash)
                    return true;
            }
            return false;
        }

        public async Task<(UserLogInfo userLogInfo, bool verifiedTempPass)> GetActivateUserToken(
            string confirmationToken,
            string tempPassword)
        {
            var filteredUserInfo = await _unitofWork.GetRepository<UserLogInfo>()
                 .ApplyPredicateAsync(u =>
                                                    u.ConfirmationToken == confirmationToken
                                                    && u.ActivateNewUser
                                                    && DateTimeOffset.UtcNow < u.TokenGenerationTime);

            var userLogInfo = await filteredUserInfo.Include(uli => uli.UserAccount).SingleOrDefaultAsync();
            bool isHash = false;
            if (userLogInfo != null)
            {
                byte[] storedSalt = Convert.FromBase64String(userLogInfo.PasswordSalt!);
                isHash = _hasher.VerifyPassword(tempPassword, userLogInfo.PasswordHash!, storedSalt);
            }

            return (userLogInfo!, isHash);
        }

        public async Task<string> GetThumbnailName(string userName)
        {
            var filteredQuery =
                await _unitofWork.GetRepository<UserAccount>()
                .ApplyPredicateAsync(u => u.UserID == userName);

            var thummbNailName = await filteredQuery
                .Select(u => u.FirstName[0].ToString().ToUpper()
                + u.LastName[0].ToString().ToUpper()).SingleOrDefaultAsync();

            return thummbNailName!;
        }

        public async Task<UserLogInfo?> GetUserAccountByUserName(string username)
        {
            var filteredQuery =
                await _unitofWork.GetRepository<UserLogInfo>()
                .ApplyPredicateAsync(u => u.UserID == username);
            var userLogInfo = await filteredQuery.FirstOrDefaultAsync();
            return userLogInfo!;
        }

        public async Task<bool?> UpdateUserLogInfoAsync(UserLogInfo userLogInfo, CancellationToken cancellationToken)
        {
            await _unitofWork.GetRepository<UserLogInfo>().UpdateAsync(userLogInfo.UserLogInfoID, userLogInfo);

            return await _unitofWork.SaveChanges(string.Empty, string.Empty, cancellationToken);
        }

        public async Task<int> GetDailyPasswordResetCount(long userAccountId)
        {
            var user = await _unitofWork.GetRepository<UserAccount>()
                .FirstOrDefaultAsync(u => u.UserAccountID == userAccountId);

            if (user == null)
                return 0;

            // Target timezone (e.g., Australia/Sydney or GMT+10, fixed offset)
            TimeSpan gmtPlus10Offset = TimeSpan.FromHours(10);

            // Define the "today" in local time (GMT+10)
            var localToday = DateTime.UtcNow.Add(gmtPlus10Offset).Date;

            // Calculate the UTC start and end bounds for that local day
            var utcStart = localToday - gmtPlus10Offset;        // Start of local day in UTC
            var utcEnd = localToday.AddDays(1) - gmtPlus10Offset; // End of local day in UTC

            // Now do the comparison in UTC
            return await _unitofWork.GetRepository<PasswordResetAudit>()
                .Query()
                .Where(p => p.UserAccountID == user.UserAccountID
                            && !p.IsSuccessfulLoginAfterReset
                            && p.CreatedDate != null
                            && p.CreatedDate >= utcStart
                            && p.CreatedDate < utcEnd)
                .CountAsync();
        }

        public async Task<int> GetConsecutiveResetWithoutLoginCount(long userAccountId)
        {
            var user = await _unitofWork.GetRepository<UserAccount>()
                .FirstOrDefaultAsync(u => u.UserAccountID == userAccountId);

            if (user == null)
                return 0;

            var query = _unitofWork.GetRepository<PasswordResetAudit>()
                .Query()
                .Where(p => p.UserAccountID == user.UserAccountID);

            // Get the timestamp of the last successful login after a reset
            var lastSuccessfulReset = await query
                .Where(p => p.IsSuccessfulLoginAfterReset)
                .OrderByDescending(p => p.CreatedDate)
                .Select(p => p.CreatedDate)
                .FirstOrDefaultAsync();

            // If there was never a successful reset/login, count all records
            var countQuery = query
                .Where(p => !p.IsSuccessfulLoginAfterReset);

            if (lastSuccessfulReset != default)
            {
                countQuery = countQuery
                    .Where(p => p.CreatedDate > lastSuccessfulReset);
            }

            return await countQuery.CountAsync();
        }

        public async Task AddPasswordResetAuditAsync(long userAccountId, bool isLoginSuccessful, CancellationToken cancellationToken)
        {
            var audit = new PasswordResetAudit
            {
                UserAccountID = userAccountId,
                CreatedDate = DateTime.UtcNow,
                IsSuccessfulLoginAfterReset = isLoginSuccessful
            };

            await _unitofWork.GetRepository<PasswordResetAudit>().AddAsync(audit);
            await _unitofWork.SaveChanges(string.Empty, string.Empty, cancellationToken);
        }
    }
}