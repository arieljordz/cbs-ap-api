
#nullable disable

namespace CbsAp.Domain.Entities.PermissionManagement
{
    public class ControlElement
    {
        public long ControlID { get; set; }
        public string ActionName { get; set; }
        public string ActionType { get; set; }
        public long OperationID { get; set; }
        public virtual Operation Operation { get; set; }
    }
}