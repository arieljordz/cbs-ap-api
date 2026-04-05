using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.PermissionManagement;
using CbsAp.Domain.Entities.RoleManagement;
using CbsAp.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CbsAp.Application.Features.Roles.Command.UpdateRole
{
    public class UpdateRoleCommandHandler : ICommandHandler<UpdateRoleCommand, ResponseResult<string>>
    {
        private readonly IUnitofWork _unitOfWork;

        private readonly ILogger<UpdateRoleCommandHandler> _logger;

        public UpdateRoleCommandHandler(IUnitofWork unitofWork, ILogger<UpdateRoleCommandHandler> logger)
        {
            _unitOfWork = unitofWork;
            _logger = logger;
        }

        public async Task<ResponseResult<string>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var dto = request.UpdateRoleDTO;
            var roleRepo = _unitOfWork.GetRepository<Role>();

            var roleName = dto.RoleName?.Trim();

            if (string.IsNullOrWhiteSpace(roleName))
            {
                return ResponseResult<string>.BadRequest("Role name is required.");
            }

            if (await IsRolExist(dto.RoleName!, dto.RoleID))
            {
                return ResponseResult<string>.Confict(
                    MessageConstants.Message(MessageOperationType.Exist, roleName)
                );
            }

            var role = await roleRepo
                .Query()
                .Include(r => r.RoleReminderNotification)
                .Include(r => r.RelatedRoleManager1)
                .Include(r => r.RelatedRoleManager2)
                .Include(r => r.RolePermissionGroups)
                .Include(r => r.UserRoles)
                .Include(r => r.RoleEntities)
                .FirstOrDefaultAsync(r => r.RoleID == dto.RoleID, cancellationToken);

            if (role == null)
            {
                return ResponseResult<string>.NotFound("Role not found.");
            }

            // Update scalar properties
            role.RoleName = dto.RoleName?.Trim() ?? role.RoleName;
            role.IsActive = dto.IsActive;
            role.AuthorisationLimit = dto.AuthorisationLimit;
            role.RoleManager1 = dto.RoleManager1;
            role.RoleManager2 = dto.RoleManager2;
            role.CanBeAddedToInvoice = dto.CanBeAddedToInvoice;
            role.RoleReminderNotification = new RoleReminderNotification
            {
                IsNewInvoiceReceiveNotification = dto?.ReminderNotification?.IsNewInvoiceReceiveNotification ?? false,
                InvoiceDueDateNotification = dto?.ReminderNotification?.InvoiceDueDateNotification,
                InvoiceEscalateToLevel1ManagerNotification = dto?.ReminderNotification?.InvoiceEscalateToLevel1ManagerNotification,
                ForwardToLevel1Manager = dto?.ReminderNotification?.ForwardToLevel1Manager,
                ForwardToLevel2Manager = dto?.ReminderNotification?.ForwardToLevel2Manager
            };

            // ======= Update RolePermissionGroups =======
            var newPermissionIds = dto?.RolePermissionGroups?.ToHashSet() ?? [];

            var toRemovePermissions = role.RolePermissionGroups
                .Where(rpg => !newPermissionIds.Contains(rpg.PermissionID))
                .ToList();

            foreach (var item in toRemovePermissions)
            {
                role.RolePermissionGroups.Remove(item);
            }

            // Add new ones
            foreach (var permId in newPermissionIds)
            {
                if (!role.RolePermissionGroups.Any(rpg => rpg.PermissionID == permId))
                {
                    role.RolePermissionGroups.Add(new RolePermissionGroup { PermissionID = permId });
                }
            }

            // ======= Update UserRoles =======
            var newUserIds = dto?.UserRoles?.ToHashSet() ?? new HashSet<long>();

            var toRemoveUserRoles = role.UserRoles
                .Where(ur => !newUserIds.Contains(ur.UserAccountID))
                .ToList();

            foreach (var item in toRemoveUserRoles)
            {
                role.UserRoles.Remove(item);
            }

            foreach (var userId in newUserIds)
            {
                if (!role.UserRoles.Any(ur => ur.UserAccountID == userId))
                {
                    role.UserRoles.Add(new UserRole { UserAccountID = userId });
                }
            }

            // ======= Update RoleEntities =======
            var newEntityIds = dto?.RoleEntities?.ToHashSet() ?? new HashSet<long>();

            var toRemoveEntities = role.RoleEntities
             .Where(re => !newEntityIds.Contains(re.EntityProfileID))
             .ToList();

            foreach (var item in toRemoveEntities)
            {
                role.RoleEntities.Remove(item);
            }

            foreach (var entityId in newEntityIds)
            {
                if (!role.RoleEntities.Any(re => re.EntityProfileID == entityId))
                {
                    role.RoleEntities.Add(new RoleEntity { EntityProfileID = entityId });
                }
            }

            // Audit update fields
            role.SetAuditFieldsOnUpdate(request.updatedBy);

            var saveResult = await _unitOfWork.SaveChanges(string.Empty,string.Empty,cancellationToken);
            if (saveResult)
            {
                return ResponseResult<string>.Success("Role updated successfully.");
            }

            _logger.LogError("Error updating role with ID: {RoleID}", dto?.RoleID);
            return ResponseResult<string>.BadRequest("Error updating role.");
        }

        private async Task<bool> IsRolExist(string roleName, long roleID)
        {
            var role =
                await _unitOfWork.GetRepository<Role>()
                .ApplyPredicateAsync(e => e.RoleID != roleID);

            var duplicates =
                await role.Select(r => new { r.RoleName }).ToListAsync();

            bool roleNameExist = duplicates.Any(r =>
            r.RoleName.ToLower().Trim() == roleName.ToLower().Trim());

            return roleNameExist;
        }
    }
}