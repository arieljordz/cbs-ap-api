using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.UserManagement;
using CbsAp.Application.Features.Users.Queries.Common;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using Mapster;

namespace CbsAp.Application.Features.Users.Queries.GetUserAcounSearch
{
    public class UserAccountSearchByUserAccountIDQueryHandler
        : IQueryHandler<SearchUserByUserAccountIDQuery, ResponseResult<IEnumerable<UserSearchPaginationDTO>>>
    {
        private readonly IUserManagementRepository _userManagementRepository;

        public UserAccountSearchByUserAccountIDQueryHandler(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }

        public async Task<ResponseResult<IEnumerable<UserSearchPaginationDTO>>> Handle(SearchUserByUserAccountIDQuery request, CancellationToken cancellationToken)
        {
            var item = await _userManagementRepository.GetUserAccountByUserAccountIDAsync(request.userAccountID);

            var maptoUserSearchWithRole = item.Adapt<IEnumerable<UserSearchPaginationDTO>>();

            if (maptoUserSearchWithRole == null)
            {
                return ResponseResult<IEnumerable<UserSearchPaginationDTO>>.BadRequest(MessageConstants.FormatMessage(MessageConstants.GetNotFound));
            }
            return  ResponseResult<IEnumerable<UserSearchPaginationDTO>>.OK(maptoUserSearchWithRole);
        }
    }
}