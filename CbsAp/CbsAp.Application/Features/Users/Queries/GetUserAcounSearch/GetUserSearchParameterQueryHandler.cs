using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.UserManagement;
using CbsAp.Application.Features.Users.Queries.Common;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Users.Queries.GetUserAcounSearch
{
    public class GetUserSearchParameterQueryHandler :
        IQueryHandler<GetUserSearchParameterQuery, ResponseResult<PaginatedList<UserSearchPaginationDTO>>>
    {
        private readonly IUserManagementRepository _userManagementRepository;

        public GetUserSearchParameterQueryHandler(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }

        public async Task<ResponseResult<PaginatedList<UserSearchPaginationDTO>>> Handle(
            GetUserSearchParameterQuery request,
            CancellationToken cancellationToken)
        {
            var results =
                await _userManagementRepository.GetUserSearchWithRolesAsync(
                request.FullName!,
                request.UserId!,
                request.IsActive,
                request.PageNumber, 
                request.PageSize,
                request.sortField,
                request.sortOrder
                , cancellationToken);

            return results == null ?
                 ResponseResult<PaginatedList<UserSearchPaginationDTO>>
                .NotFound("No user found!")
              : ResponseResult<PaginatedList<UserSearchPaginationDTO>>
                .OK(results, " retrieve records");
        }
    }
}