using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Services.Authentication;
using CbsAp.Application.Shared.Encryption;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Authentication.Commands.ActivateUser
{
    public class ActivateUserCommandHandler : ICommandHandler<ActivateUserCommand, ResponseResult<bool>>
    {
        private readonly IUserAuthService _userAuthService;
        private readonly IHasher _hash;

        public ActivateUserCommandHandler(IUserAuthService userAuthService, IHasher hash)
        {
            _userAuthService = userAuthService;
            _hash = hash;
        }

        public async Task<ResponseResult<bool>> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userAuthService.GetActivateUserToken(request.ConfirmationToken, request.TempPassword);

            if (!user.verifiedTempPass)
            {
                return ResponseResult<bool>.BadRequest("Your temporary password does not match");
            }

            if (user.userLogInfo == null)
            {
                return ResponseResult<bool>.BadRequest("Failed to set new password. New user confirmation details is invalid");
            }

            var hashPasword = _hash.HashPasword(request.NewPassword, out var salt);
            user.userLogInfo.ActivateNewUser = false;
            user.userLogInfo.PasswordHash = hashPasword;
            user.userLogInfo.PasswordSalt = Convert.ToBase64String(salt);
            user.userLogInfo.UserAccount.IsActive = true;


            var result = await _userAuthService.SetNewPasswordUpdate(user.userLogInfo,true,  cancellationToken);

            if (!result)
                return ResponseResult<bool>
                    .BadRequest("Failed to set new password.Your password reset link  is expired or is not existed.");

            return ResponseResult<bool>.OK("You have successfully set new password.");
        }
    }
}