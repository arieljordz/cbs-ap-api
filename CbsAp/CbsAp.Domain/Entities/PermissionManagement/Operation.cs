using CbsAp.Domain.Common;

namespace CbsAp.Domain.Entities.PermissionManagement
{
    public class Operation : BaseAuditableEntity
    {
        public long OperationID { get; set; }
        public string OperationName { get; set; }
        public string Panel { get; set; }
        public string ApplyOperationIn { get; set; }

        public virtual ICollection<PermissionGroup> PermissionGroups { get; set; } = new List<PermissionGroup>();
        public virtual ControlElement ControlElement { get; set; }
        public virtual ICollection<Menu> Menus { get; set; }
    }
}