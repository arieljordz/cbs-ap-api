using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.DTOs.UserManagement
{
    public class UserExportDTO: IIsActiveEntity
    {
        public string UserID { get; set; }

        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }

        public string FullName { get; set; }

        public string LastLoginDateTime { get; set; }

        public bool IsLockedOut { get; set; }

        public int CountOfAssignedRoles { get; set; }
    }
}
