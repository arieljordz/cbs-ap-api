using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.RoleManagement;
using Mapster;

namespace CbsAp.Application.Features.Roles.Queries.SearchHandler
{
    public class GetRoleReminderByIdQueryHandler
        : IQueryHandler<GetRoleReminderByIdQuery, ResponseResult<IQueryable<ReminderNotificationDTO>>>
    {
        private readonly IUnitofWork _unitofWork;

        public GetRoleReminderByIdQueryHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<ResponseResult<IQueryable<ReminderNotificationDTO>>> Handle(
            GetRoleReminderByIdQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _unitofWork
                .GetRepository<RoleReminderNotification>()
                .ApplyPredicateAsync(rn => rn.RoleID == request.RoleID);
            var maptoDTO =
                result.ProjectToType<ReminderNotificationDTO>()
                ?? Enumerable.Empty<ReminderNotificationDTO>().AsQueryable();

            return ResponseResult<IQueryable<ReminderNotificationDTO>>.OK(maptoDTO);
        }
    }

}