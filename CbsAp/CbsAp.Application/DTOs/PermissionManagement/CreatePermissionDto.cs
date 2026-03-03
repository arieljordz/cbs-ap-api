using CbsAp.Application.DTOs.PermissionManagement.OperationDTO;

namespace CbsAp.Application.DTOs.PermissionManagement
{
    public class CreatePermissionDto
    {
        public string PermissionName { get; set; }
        public List<GroupPanelDTO> GroupPanel { get; set; }
        public List<int> Roles { get; set; }

        public bool IsActive { get; set; } = true;
    }
}