using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Domain.Common.Interfaces;

namespace CbsAp.Application.DTOs.UserManagement
{
    public class UserSearchPaginationDTO : IIsActiveEntity
    {
        public long UserAccountID { get; set; }

        public string UserID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }

        public string FullName { get; set; }

        public string LastLoginDateTime { get; set; }
        public bool IsLockedOut { get; set; }

        public List<RoleDTO> UserRoles { get; set; }

        public int CountOfAssignedRoles { get; set; }
    }
}