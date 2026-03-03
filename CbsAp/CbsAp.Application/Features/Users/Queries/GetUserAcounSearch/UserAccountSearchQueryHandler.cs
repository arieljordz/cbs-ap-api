using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.UserManagement;
using CbsAp.Application.Features.Users.Queries.Common;
using CbsAp.Application.Shared;

using Mapster;

namespace CbsAp.Application.Features.Users.Queries.GetUserAcounSearch
{
    public class UserAccountSearchQueryHandler : IQueryHandler<GetUserSearchQuery, Result<List<UserSearchPaginationDTO>>>
    {
        private readonly IUserManagementRepository _userManagementRepository;

        public UserAccountSearchQueryHandler(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }

        public async Task<Result<List<UserSearchPaginationDTO>>> Handle(GetUserSearchQuery request, CancellationToken cancellationToken)
        {
            throw new Exception();
        //    var item = await _userManagementRepository.GetUserSearchWithRolesAsync();

        //    var maptoUserSearchWithRoleDTO = item?.Adapt<List<UserSearchWithRolesDTO>>();

        //    return maptoUserSearchWithRoleDTO == null ?
        //        await Result<List<UserSearchWithRolesDTO>>
        //        .FailureAsync(MessageConstants.FormatMessage(MessageConstants.GetNotFound))
        //      : await Result<List<UserSearchWithRolesDTO>>.SuccessAsync(maptoUserSearchWithRoleDTO);
        //
        }
    }
}