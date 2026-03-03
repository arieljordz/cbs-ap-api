using CbsAp.Domain.Common;
using CbsAp.Domain.Common.Interfaces;
using CbsAp.Domain.Entities.PermissionManagement;
using System.ComponentModel.DataAnnotations.Schema;

namespace CbsAp.Domain.Entities.RoleManagement
{
    public class Role : BaseAuditableEntity, IIsActiveEntity
    {
        public long RoleID { get; set; }

        public string RoleName { get; set; }
        public bool IsActive { get; set; }

        //Limit Amount
        public decimal? AuthorisationLimit { get; set; }

        //userid assigned as role manager
        public long? RoleManager1 { get; set; }

        [ForeignKey("RoleManager1")]
        public virtual Role? RelatedRoleManager1 { get; set; }

        //userid assigned as role manager 1
      
        public long? RoleManager2 { get; set; }

        [ForeignKey("RoleManager2")]
        public virtual Role? RelatedRoleManager2 { get; set; }
        public bool CanBeAddedToInvoice { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        // 1 to 1 Relationship with Role Reminder Notification
        public virtual RoleReminderNotification RoleReminderNotification { get; set; }

        // role permission
        public virtual ICollection<RolePermissionGroup> RolePermissionGroups { get; set; }

        //role entity

        public virtual ICollection<RoleEntity> RoleEntities { get; set; }

        
    }
}