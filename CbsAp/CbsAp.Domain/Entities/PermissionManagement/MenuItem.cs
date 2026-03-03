namespace CbsAp.Domain.Entities.PermissionManagement
{
    public class MenuItem
    {
        public long MenuItemID { get; set; }
        public long? MenuID { get; set; }
        public string Label { get; set; }
        public string Icon { get; set; }
        public string RouterLink { get; set; }
        public virtual Menu Menu { get; set; }
    }
}