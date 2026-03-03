using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.DTOs.RolesManagement
{
    public class UpdateRoleBasicInfoDTO
    {
        public long RoleID { get; set; }
        public string? RoleName { get; set; }
        public bool IsActive { get; set; }

        //Limit Amount
        public decimal? AuthorisationLimit { get; set; }

        //userid assigned as role manager
        public long? RoleManager1 { get; set; }

        //userid assigned as role manager
        public long? RoleManager2 { get; set; }

        public bool CanBeAddedToInvoice { get; set; }
    }
}