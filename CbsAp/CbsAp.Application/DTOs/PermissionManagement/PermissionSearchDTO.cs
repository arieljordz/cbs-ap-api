using CbsAp.Domain.Common.Interfaces;

namespace CbsAp.Application.DTOs.PermissionManagement
{
    public class PermissionSearchDTO : IIsActiveEntity
    {
        public long PermissionID { get; set; }
        public string PermissionName { get; set; }
        public string PermissionGroupName { get; set; }
        public int CountofRoles { get; set; }
        public int CountofUsers { get; set; }
        public bool IsActive { get; set; }
    }
}