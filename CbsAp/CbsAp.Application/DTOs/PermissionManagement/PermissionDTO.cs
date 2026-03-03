using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.DTOs.PermissionManagement
{
    public class PermissionDTO
    {
        public string PermissionName { get; set; }
        public List<int> Roles { get; set; }
    }
}