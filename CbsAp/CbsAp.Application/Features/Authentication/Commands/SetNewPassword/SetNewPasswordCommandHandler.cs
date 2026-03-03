using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Authentication;
using CbsAp.Application.Shared.Encryption;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.UserManagement;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.Authentication.Commands.SetNewPassword
{
    public class SetNewPasswordCommandHandler : ICommandHandler<SetNewPasswordCommand, ResponseResult<bool>>
    {
        private readonly IUserAuthService _userAuthService;
        private readonly IUnitofWork _unitofWork;
        private readonly IHasher _hash;
        private readonly IUserManagementRepository _userManagementRepository;

        public SetNewPasswordCommandHandler(IUserAuthService userAuthService, IUnitofWork unitofWork, IHasher hash, IUserManagementRepository userManagementRepository)
        {
            _userAuthService = userAuthService;
            _unitofWork = unitofWork;
            _hash = hash;
            _userManagementRepository = userManagementRepository;
        }

        public async Task<ResponseResult<bool>> Handle(SetNewPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = _userManagementRepository.GetUserAccountAsQueryable()
                .Include(u => u.UserLogInfo)
                .FirstOrDefault(u => u.UserLogInfo.PasswordrecoveryToken == request.PasswordrecoveryToken 
                                    && u.UserLogInfo.IsPasswordRecoveryTokenUsed == false
                                    && DateTimeOffset.UtcNow < u.UserLogInfo.RecoveryTokenTime);

            if (user == null || user.UserLogInfo == null)
                return ResponseResult<bool>.BadRequest("Failed to set new password.Your password reset link is expired or is not existed.");

            var isNewPasswordNotAllowed = await
                _userAuthService.GetPasswordHistories(user.UserAccountID,
                    request.NewPassword,
                    cancellationToken);

            if (isNewPasswordNotAllowed)
                return ResponseResult<bool>.BadRequest("You cannot reuse a previous password.");

            var hashPasword = _hash.HashPasword(request.NewPassword, out var salt);
            user.UserLogInfo.PasswordHash = hashPasword;
            user.UserLogInfo.PasswordSalt = Convert.ToBase64String(salt);
            user.UserLogInfo.SetAuditFieldsOnUpdate("self (set new - password)");
            user.UserLogInfo.IsPasswordRecoveryTokenUsed = true;
            user.UserLogInfo.ActivateNewUser = false;

            var savePasswordHistory = new PasswordHistory
            {
                UserAccountID = user.UserLogInfo.UserAccountID,
                PasswordHash = user.UserLogInfo.PasswordHash,
                PasswordSalt = user.UserLogInfo.PasswordSalt,
                CreatedAt = user.UserLogInfo?.LastUpdatedDate!
            };

            user.PasswordHistories.Add(savePasswordHistory);

            var audit = new PasswordResetAudit
            {
                CreatedDate = DateTime.UtcNow,
                IsSuccessfulLoginAfterReset = true
            };

            user.PasswordResetAudits.Add(audit);

            var result = await _unitofWork.SaveChanges(cancellationToken);

            if (!result)
                return ResponseResult<bool>
                    .BadRequest("Failed to set new password. Please contact your administrator.");

            return ResponseResult<bool>.OK("You have successfully set new password.");
        }
    }
}