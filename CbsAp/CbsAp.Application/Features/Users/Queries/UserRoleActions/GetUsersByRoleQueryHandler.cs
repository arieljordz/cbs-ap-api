using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.UserManagement;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Users.Queries.UserRoleActions
{
    public class GetUsersByRoleQueryHandler
        : IQueryHandler<GetUsersByRoleQuery, ResponseResult<IQueryable<ActiveUsersDTO>>>
    {
        private readonly IUserManagementRepository _userManagementRepository;

        public GetUsersByRoleQueryHandler(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }

        public async Task<ResponseResult<IQueryable<ActiveUsersDTO>>>
            Handle(GetUsersByRoleQuery request,
            CancellationToken cancellationToken)
        {
            var results = await _userManagementRepository
                .GetActiveUserByRoleAsync(request.RoleID);

            return ResponseResult<IQueryable<ActiveUsersDTO>>.OK(results);
        }
    }
}