using CbsAp.Application.DTOs.PermissionManagement.OperationDTO;
using CbsAp.Application.DTOs.RolesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.DTOs.PermissionManagement
{
    public class PermissionSearchByIdDTO
    {
        public long PermissionID { get; set; }
        public string PermissionGroupName { get; set; }
        public List<GroupPanelDTO> GroupPanel { get; set; }
        public List<RoleDTO> Roles { get; set; }
    }
}