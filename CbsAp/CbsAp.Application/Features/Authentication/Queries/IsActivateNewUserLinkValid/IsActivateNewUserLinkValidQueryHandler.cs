using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Shared.ResultPatten;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.Authentication.Queries.IsActivateNewUserLinkValid
{
    public class IsActivateNewUserLinkValidQueryHandler : IQueryHandler<IsActivateNewUserLinkValidQuery, ResponseResult<bool>>
    {
        private readonly IUserManagementRepository _userManagementRepository;

        public IsActivateNewUserLinkValidQueryHandler(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }

        public async Task<ResponseResult<bool>> Handle(IsActivateNewUserLinkValidQuery request, CancellationToken cancellationToken)
        {
            var isLinkValid = await _userManagementRepository.GetUserAccountAsQueryable()
                .Include(u => u.UserLogInfo)
                .AnyAsync(u => u.UserLogInfo.ConfirmationToken == request.Token
                                && u.UserLogInfo.ActivateNewUser
                                && DateTimeOffset.UtcNow < u.UserLogInfo.TokenGenerationTime, cancellationToken: cancellationToken);

            if (!isLinkValid)
                return ResponseResult<bool>.OK(false);

            return ResponseResult<bool>.OK(true);
        }
    }
}
