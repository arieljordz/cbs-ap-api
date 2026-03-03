using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Shared.ResultPatten;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.Authentication.Queries.IsPasswordResetLinkValid
{
    public class IsPasswordResetLinkValidQueryHandler : IQueryHandler<IsPasswordResetLinkValidQuery, ResponseResult<bool>>
    {
        private readonly IUserManagementRepository _userManagementRepository;

        public IsPasswordResetLinkValidQueryHandler(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }

        public async Task<ResponseResult<bool>> Handle(IsPasswordResetLinkValidQuery request, CancellationToken cancellationToken)
        {
            var isLinkValid = await _userManagementRepository.GetUserAccountAsQueryable()
                .Include(u => u.UserLogInfo)
                .AnyAsync(u => u.UserLogInfo.PasswordrecoveryToken == request.Token
                                    && u.UserLogInfo.IsPasswordRecoveryTokenUsed == false
                                    && DateTimeOffset.UtcNow < u.UserLogInfo.RecoveryTokenTime);

            if (!isLinkValid)
                return ResponseResult<bool>.OK(false);

            return ResponseResult<bool>.OK(true);
        }
    }
}
