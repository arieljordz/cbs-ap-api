using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.UserManagement;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Users.Queries.UserRoleActions
{
    public record GetUserNotInRoleQueryHandler
        : IQueryHandler<GetUserNotInRoleQuery, ResponseResult<IQueryable<ActiveUsersDTO>>>
    {
        private readonly IUserManagementRepository _userManagementRepository;

        public GetUserNotInRoleQueryHandler(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }
        public async Task<ResponseResult<IQueryable<ActiveUsersDTO>>> Handle(
            GetUserNotInRoleQuery request,
            CancellationToken cancellationToken)
        {
            var results = await _userManagementRepository
                .GetActiveUserNotInRoleAsync(request.RoleID);

            return ResponseResult<IQueryable<ActiveUsersDTO>>.OK(results);
        }
    }
}