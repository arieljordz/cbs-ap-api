using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.DTOs.PermissionManagement
{
    public class PermissionReportDTO
    {
        public long PermissionGroupID { get; set; }

        public string PermissionGroupName { get; set; }
        public int CountofAssignedRoles { get; set; }
        public int CountofUsers { get; set; }
        public string ActiveStatus { get; set; }
    }
}
