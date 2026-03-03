using CbsAp.Application.DTOs.UserAuthentication;
using CbsAp.Application.Features.Authentication.Common;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.UserManagement;
using Mapster;

namespace CbsAp.Application.MapsterMappinngs.Authentication
{
    public class AuthenticationMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<ResponseResult<AuthenticationTokenResult>, ResponseResult<AuthenticationResponse>>()
               .Map(dest => dest.IsSuccess, src => src.IsSuccess)
               .Map(dest => dest.ResponseData!.UserName, src => src.ResponseData!.UserName)
               .Map(dest => dest.ResponseData!.Token, src => src.ResponseData!.Token)
               .Map(dest => dest.Messages, src => src.Messages);

            config.NewConfig<ForgotPasswordDto, UserLogInfo>()

                .Map(dest => dest.PasswordrecoveryToken, src => src.PasswordrecoveryToken)
                .Map(dest => dest.IsPasswordRecoveryTokenUsed, src => src.IsPasswordRecoveryTokenUsed)
                .Map(dest => dest.RecoveryTokenTime, src => src.RecoveryTokenTime);
        }
    }
}
