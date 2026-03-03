using CbsAp.Application.DTOs.PermissionManagement;
using CbsAp.Domain.Entities.RoleManagement;

namespace CbsAp.Application.DTOs.RolesManagement
{
    public class RoleSearchDTO
    {
        public long RoleID { get; set; }

        public string? Entity { get; set; }
        public string? RoleName { get; set; }

        public string RoleManager1 { get; set; }
        public string RoleManager2 { get; set; }

        public decimal? AuthorisationLimit { get; set; }

        public bool IsActive { get; set; }
    }
}