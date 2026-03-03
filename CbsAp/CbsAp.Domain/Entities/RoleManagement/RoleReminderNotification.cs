using CbsAp.Domain.Common;

namespace CbsAp.Domain.Entities.RoleManagement
{
    public class RoleReminderNotification : BaseAuditableEntity
    {
        public long RoleReminderNotificationID { get; set; }

        public bool IsNewInvoiceReceiveNotification { get; set; }

        /// <summary>
        /// Send email reminder X days before the due date, 0=inactive
        /// </summary>
        public int? InvoiceDueDateNotification { get; set; }

        /// <summary>
        /// Send email  to assigned Level 1 Manager of role after X days that the invoice is still not
        /// processed before the due date
        /// </summary>
        public int? InvoiceEscalateToLevel1ManagerNotification { get; set; }

        /// <summary>
        /// Transfer invoice to Level 1 Manager  after X days after escalation email
        /// </summary>
        public int? ForwardToLevel1Manager { get; set; }

        /// <summary>
        /// Transfer invoice to Level 2 Manager  X days after the invoice  was forwarded to the Level 1 Manager
        /// </summary>
        public int? ForwardToLevel2Manager { get; set; }

        // Foreign Key to Role
        public long RoleID { get; set; }

        public virtual Role Role { get; set; }
    }
}