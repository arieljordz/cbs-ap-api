using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.DTOs.RolesManagement
{
    public class RolesExportDto
    {
        public string EntityName { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string RoleManagerOne { get; set; } = string.Empty;
        public string RoleManagerTwo { get; set; } = string.Empty;
        public string AuthorisationLimit { get; set; } = string.Empty;
        public string PermissionGroups { get; set; } = string.Empty;    
        public bool IsActive { get; set; }
    }
}
