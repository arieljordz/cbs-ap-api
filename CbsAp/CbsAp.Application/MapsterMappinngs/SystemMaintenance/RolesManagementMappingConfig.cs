using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.Features.Roles.Command.CreateRole;
using CbsAp.Application.Features.Roles.Command.UpdateRole;
using CbsAp.Domain.Entities.PermissionManagement;
using CbsAp.Domain.Entities.RoleManagement;
using Mapster;

namespace CbsAp.Application.MapsterMappinngs.SystemMaintenance
{
    public class RolesManagementMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {

            config.NewConfig<CreateRoleCommand, CreateRoleDTO>()
                .MapWith(dto => new CreateRoleDTO
                {
                    RoleName = dto.roleDTO.RoleName,
                    AuthorisationLimit = dto.roleDTO.AuthorisationLimit,
                    RoleManager1 = dto.roleDTO.RoleManager1,
                    RoleManager2 = dto.roleDTO.RoleManager2,
                    IsActive = dto.roleDTO.IsActive,// primitive
                    CanBeAddedToInvoice = dto.roleDTO.CanBeAddedToInvoice,
                    ReminderNotification = dto.roleDTO.ReminderNotification,
                    RolePermissionGroups = dto.roleDTO.RolePermissionGroups,
                    UserRoles = dto.roleDTO.UserRoles,
                    RoleEntities = dto.roleDTO.RoleEntities
                });

            config.NewConfig<CreateRoleDTO, Role>()
                 .MapWith(dto => new Role
                 {
                     RoleName = dto.RoleName,
                     AuthorisationLimit = dto.AuthorisationLimit,
                     RoleManager1 = dto.RoleManager1,
                     RoleManager2 = dto.RoleManager2,
                     IsActive = dto.IsActive, // primitive
                     CanBeAddedToInvoice = dto.CanBeAddedToInvoice,
                     RoleReminderNotification = 
                        MapToReminderNotification(dto.ReminderNotification, config),
                 });

            config.NewConfig<CreateRoleDTO, RolePermissionGroup>()
                .Ignore(dest => dest.CreatedDate!)
                .Ignore(dest => dest.CreatedBy!)
                 .Ignore(dest => dest.LastUpdatedBy!)
                 .Ignore(dest => dest.LastUpdatedDate!)
                .MapWith(dto => new RolePermissionGroup
                {
                    RoleID = dto.RolePermissionGroups.FirstOrDefault(),
                });

            config.NewConfig<Role, SearchRoleDtO>()
                .MapWith(role => new SearchRoleDtO
                {
                    RoleID = role.RoleID,
                    RoleName = role.RoleName,
                    RoleManager1 = role.RoleManager1,
                    RoleManager2 = role.RoleManager2,
                    AuthorisationLimit = role.AuthorisationLimit,
                    IsActive = role.IsActive
                });

            config.NewConfig<UpdateRoleDTO, Role>()
               .MapWith(dto => new Role
               {
                   RoleName = dto.RoleName,
                   AuthorisationLimit = dto.AuthorisationLimit,
                   RoleManager1 = dto.RoleManager1,
                   RoleManager2 = dto.RoleManager2,
                   IsActive = dto.IsActive, // primitive
                   CanBeAddedToInvoice = dto.CanBeAddedToInvoice,
                   RoleReminderNotification =
                      MapToReminderNotification(dto.ReminderNotification, config),
               });
        }

        private static RoleReminderNotification MapToReminderNotification(
            ReminderNotificationDTO dto,
            TypeAdapterConfig config)
        {
            config.NewConfig<ReminderNotificationDTO, RoleReminderNotification>()
                .MapWith(dto => new RoleReminderNotification
                {
                    RoleID = dto.RoleID,
                    IsNewInvoiceReceiveNotification = dto.IsNewInvoiceReceiveNotification,
                    InvoiceDueDateNotification = dto.InvoiceDueDateNotification,
                    InvoiceEscalateToLevel1ManagerNotification = dto.InvoiceEscalateToLevel1ManagerNotification,
                    ForwardToLevel1Manager = dto.ForwardToLevel1Manager,
                    ForwardToLevel2Manager = dto.ForwardToLevel2Manager
                });

            return dto.Adapt<RoleReminderNotification>();
        }
    }
}
