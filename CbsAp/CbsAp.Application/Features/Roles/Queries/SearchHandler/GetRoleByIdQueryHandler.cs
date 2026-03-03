using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.Features.Roles.Queries.Common;
using CbsAp.Application.Shared.ResultPatten;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.Roles.Queries.SearchHandler
{
    public class GetRoleByIdQueryHandler : IQueryHandler<SearchRoleByIdQuery, ResponseResult<SearchRoleDtO>>
    {
        private readonly IRoleManagementRepository _roleManagementRepository;

        public GetRoleByIdQueryHandler(IRoleManagementRepository roleManagementRepository)
        {
            _roleManagementRepository = roleManagementRepository;
        }

        public async Task<ResponseResult<SearchRoleDtO>> Handle(
            SearchRoleByIdQuery request,
            CancellationToken cancellationToken)
        {
            var role = await _roleManagementRepository.GetRoleAsQueryable()
                .Include(rn => rn.RoleReminderNotification)
                .Include(re => re.RoleEntities)
                .Include(rp => rp.RolePermissionGroups)
                .Include(ru => ru.UserRoles)
                .Include(rm1 => rm1.RelatedRoleManager1)
                .Include(rm2 => rm2.RelatedRoleManager2)
                .FirstOrDefaultAsync(r => r.RoleID == request.roleID, cancellationToken);

            if (role == null)
                return ResponseResult<SearchRoleDtO>.NotFound("Role not Found");

            var searchRoleDto = new SearchRoleDtO
            {
                RoleID = role.RoleID,
                RoleName = role.RoleName,
                RoleManager1 = role.RoleManager1,
                RoleManager2 = role.RoleManager2,
                RoleManager1Name = role.RelatedRoleManager1 != null ? role.RelatedRoleManager1.RoleName : string.Empty,
                RoleManager2Name = role.RelatedRoleManager2 != null ? role.RelatedRoleManager2.RoleName : string.Empty,
                AuthorisationLimit = role.AuthorisationLimit,
                IsActive = role.IsActive,
                CanBeAddedToInvoice = role.CanBeAddedToInvoice,
                ReminderNotification = role.RoleReminderNotification != null ? new RoleReminderNotificationDto
                {
                    IsNewInvoiceReceiveNotification = role.RoleReminderNotification.IsNewInvoiceReceiveNotification,
                    InvoiceDueDateNotification = role.RoleReminderNotification.InvoiceDueDateNotification,
                    InvoiceEscalateToLevel1ManagerNotification = role.RoleReminderNotification.InvoiceEscalateToLevel1ManagerNotification,
                    ForwardToLevel1Manager = role.RoleReminderNotification.ForwardToLevel1Manager,
                    ForwardToLevel2Manager = role.RoleReminderNotification.ForwardToLevel2Manager
                } : null,
                RoleEntities = [.. (role.RoleEntities?
                    .Select(re => new RoleEntitiyDto
                    {
                        EntityProfileID = re.EntityProfileID,
                        EntityName = re.EntityProfile?.EntityName ?? string.Empty,
                        EntityCode = re.EntityProfile?.EntityCode ?? string.Empty
                    }) ?? Enumerable.Empty<RoleEntitiyDto>())],
                RolePermissions = [.. role.RolePermissionGroups.Select(rp => new RolePermissionDto
                    {
                        PermissionID = rp.PermissionID,
                        PermissionName = rp.Permission.PermissionName
                    }) ?? Enumerable.Empty<RolePermissionDto>()],
                RoleUsers = [.. role.UserRoles.Select(ru => new RoleUserDto
                    {
                        UserAccountID = ru.UserAccountID,
                        UserID = ru.UserAccount.UserID,
                        FullName = ru.UserAccount.FirstName + " " + ru.UserAccount.LastName
                    }) ?? Enumerable.Empty<RoleUserDto>()]
            };

            return ResponseResult<SearchRoleDtO>.SuccessRetrieveRecords(searchRoleDto, "Role");
        }
    }
}