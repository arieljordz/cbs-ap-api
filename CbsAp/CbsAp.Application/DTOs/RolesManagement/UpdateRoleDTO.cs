namespace CbsAp.Application.DTOs.RolesManagement
{
    /// <summary>
    /// Defines the <see cref="UpdateRoleDTO" />
    /// </summary>
    public class UpdateRoleDTO
    {
        public long RoleID { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public decimal? AuthorisationLimit { get; set; }
        public long? RoleManager1 { get; set; }
        public long? RoleManager2 { get; set; }
        public bool CanBeAddedToInvoice { get; set; }
        public ReminderNotificationDTO? ReminderNotification { get; set; }
        public List<long> RolePermissionGroups { get; set; } = [];
        public List<long> UserRoles { get; set; } = [];
        public List<long> RoleEntities { get; set; } = [];
    }
}
