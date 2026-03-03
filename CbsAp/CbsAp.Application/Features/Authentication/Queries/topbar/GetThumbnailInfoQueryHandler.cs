using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Services.Authentication;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Authentication.Queries.topbar
{
    public class GetThumbnailInfoQueryHandler : IQueryHandler<GetThumbnailInfoQuery, ResponseResult<string>>
    {
        private readonly IUserAuthService _userAuthService;

        public GetThumbnailInfoQueryHandler(IUserAuthService userAuthService)
        {
            _userAuthService = userAuthService;
        }

        public async Task<ResponseResult<string>> Handle(GetThumbnailInfoQuery request, CancellationToken cancellationToken)
        {
            var result = await _userAuthService.GetThumbnailName(request.UserName);
            return result == null ?
                ResponseResult<string>.BadRequest("No thumbnail name found") :
                ResponseResult<string>.Success(result);
        }
    }
}
