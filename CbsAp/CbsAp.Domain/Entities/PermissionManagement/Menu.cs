namespace CbsAp.Domain.Entities.PermissionManagement
{
    public class Menu
    {
        public long MenuID { get; set; }
        public string Label { get; set; }
        public string? Icon { get; set; }
        public string? RouterLink { get; set; }
        public virtual ICollection<MenuItem> MenuItems { get; set; }
        public long? OperationID { get; set; } 
        public virtual Operation Operation { get; set; } 
    }
}