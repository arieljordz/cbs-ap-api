namespace CbsAp.Application.DTOs.RolesManagement
{
    public class CreateRoleDTO
    {
        public string RoleName { get; set; }
        public bool IsActive { get; set; }

        //Limit Amount
        public decimal? AuthorisationLimit { get; set; }

        //userid assigned as role manager
        public long? RoleManager1 { get; set; }
        

        //userid assigned as role manager
        public long? RoleManager2 { get; set; }

        public bool CanBeAddedToInvoice { get; set; }

        public ReminderNotificationDTO ReminderNotification { get; set; }

        public List<long> RolePermissionGroups { get; set; }

        public List<long> UserRoles { get; set; }

        public List<long> RoleEntities { get; set; }

    }
}