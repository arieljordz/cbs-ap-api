namespace CbsAp.Application.DTOs.PermissionManagement.OperationDTO
{
    public class GroupPanelDTO
    {
        public string Panel { get; set; }

        public long PermissionGroupID { get; set; }

        public IEnumerable<OperationsDTO> Operations { get; set; }
    }
}