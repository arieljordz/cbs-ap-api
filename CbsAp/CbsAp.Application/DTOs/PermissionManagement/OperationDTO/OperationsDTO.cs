#nullable disable

namespace CbsAp.Application.DTOs.PermissionManagement.OperationDTO
{
    public class OperationsDTO
    {
        public long PermissionGroupID { get; set; }
        public long OperationID { get; set; }
        public string OperationName { get; set; }
        public string Panel { get; set; }
        public string ApplyOperationIn { get; set; }
        public bool Access { get; set; } = false;
    }

    
}