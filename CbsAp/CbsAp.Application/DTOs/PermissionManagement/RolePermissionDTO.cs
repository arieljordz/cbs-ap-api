using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.DTOs.PermissionManagement
{
    public class RolePermissionDTO
    {
        public long RoleID { get; set; }
        public List<PermissionDetailDTO> Permissions { get; set; }
    }
}
