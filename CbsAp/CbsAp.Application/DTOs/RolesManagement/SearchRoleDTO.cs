using System.Runtime.CompilerServices;

namespace CbsAp.Application.DTOs.RolesManagement
{
    public class SearchRoleDtO
    {
        public long RoleID { get; set; }
        public string RoleName { get; set; }
        public long? RoleManager1 { get; set; }
        public long? RoleManager2 { get; set; }
        public string? RoleManager1Name { get; set; }
        public string? RoleManager2Name { get; set; }
        public decimal? AuthorisationLimit { get; set; }
        public bool IsActive { get; set; }
        public bool CanBeAddedToInvoice { get; set; }
        public RoleReminderNotificationDto? ReminderNotification { get; set; }
        public List<RoleEntitiyDto>? RoleEntities { get; set; }
        public List<RolePermissionDto>? RolePermissions { get; set; }
        public List<RoleUserDto>? RoleUsers { get; set; }

        public List<RoleDimensionDto>? RoleDimensions { get; set; } = [];

        public List<DropdownOptionDto>? EntityOptions { get; set; } = [];

        public List<DropdownOptionDto>? CategoryOptions { get; set; } = [];
    }

    public class RolePermissionDto
    {
        public long PermissionID { get; set; }
        public string PermissionName { get; set; }
    }

    public class RoleEntitiyDto
    {
        public long EntityProfileID { get; set; }
        public string EntityName { get; set; }
        public string EntityCode { get; set; }
    }

    public class RoleUserDto
    {
        public long UserAccountID { get; set; }
        public string UserID { get; set; }
        public string FullName { get; set; }
    }

    public class RoleReminderNotificationDto
    {
        public bool IsNewInvoiceReceiveNotification { get; set; }
        public int? InvoiceDueDateNotification { get; set; }
        public int? InvoiceEscalateToLevel1ManagerNotification { get; set; }
        public int? ForwardToLevel1Manager { get; set; }
        public int? ForwardToLevel2Manager { get; set; }
    }

    public class DropdownOptionDto
    {
        public string Label { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }

    public class RoleDimensionDto
    {
        public long EntityProfileID { get; set; }
        public long DimensionID { get; set; }
        public string DimensionName { get; set; } = string.Empty;
        public string Assigned { get; set; } = string.Empty;
    }

    public class EntityOptionsDto
    {
        public List<DropdownOptionDto> EntityOptions { get; set; } = [];
    }

    public class CategoryOptionsDto
    {
        public List<DropdownOptionDto> CategoryOptions { get; set; } = [];
    }

}