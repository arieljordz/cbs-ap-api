using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.UserManagement;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.UserManagement;
using CbsAp.Domain.Enums;
using Mapster;

namespace CbsAp.Application.Features.Users.Queries.UserRoleActions
{
    public class ActiveUsersQueryHandler
        : IQueryHandler<ActiveUsersQuery, ResponseResult<IQueryable<ActiveUsersDTO>>>
    {
        private readonly IUnitofWork _unitofWork;

        public ActiveUsersQueryHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<ResponseResult<IQueryable<ActiveUsersDTO>>> Handle(
            ActiveUsersQuery request,
            CancellationToken cancellationToken)
        {
            var users = await _unitofWork
                  .GetRepository<UserAccount>()
                  .ApplyPredicateAsync(u => u.IsActive);

            var activeUsersDTO = users
                .ProjectToType<ActiveUsersDTO>();
            return !activeUsersDTO.Any()
                ? ResponseResult<IQueryable<ActiveUsersDTO>>
                     .NotFound(
                        MessageConstants.Message("Active users", MessageOperationType.NotFound)
                    )
                : ResponseResult<IQueryable<ActiveUsersDTO>>
                    .SuccessRetrieveRecords(activeUsersDTO.OrderBy(au=> au.UserID), "Active users");
        }
    }
}