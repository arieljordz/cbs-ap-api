namespace CbsAp.Application.DTOs.PermissionManagement
{
    public class PermissionGroupDTO
    {
        public long PermissionGroupID { get; set; }
        public long PermissionID { get; set; }
        public long OperationID { get; set; }
        public bool Access { get; set; }
    }
}