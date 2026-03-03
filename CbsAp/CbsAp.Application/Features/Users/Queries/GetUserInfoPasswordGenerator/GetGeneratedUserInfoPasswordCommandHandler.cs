using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs;
using CbsAp.Application.Features.Users.Commands.Common;
using CbsAp.Application.Shared.Encryption;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Users.Queries.GetUserInfoPasswordGenerator
{
    public class GetGeneratedUserInfoPasswordCommandHandler 
        : IQueryHandler<GenerateUserPasswordCommand, ResponseResult<UpdatePasswordDTO>>
    {
        private readonly IHasher _hash;

        public GetGeneratedUserInfoPasswordCommandHandler(IHasher hash)
        {
            _hash = hash;
        }
        public Task<ResponseResult<UpdatePasswordDTO>> Handle(
            GenerateUserPasswordCommand? request,
            CancellationToken cancellationToken)
        {
           
            var hashPassword = _hash.HashPasword(request.Password!, out var salt);

            var updatePasswordDTO = new UpdatePasswordDTO()
            {
                UserID = request.UserID,
                UserAccountID = request.UserAccountID,
                UserLogInfoID = request.UserLogInfoID,
                PasswordHash = hashPassword,
                PasswordSalt = Convert.ToBase64String(salt)
            };

            return Task.FromResult(ResponseResult<UpdatePasswordDTO>.OK(updatePasswordDTO));
        }
    }
}