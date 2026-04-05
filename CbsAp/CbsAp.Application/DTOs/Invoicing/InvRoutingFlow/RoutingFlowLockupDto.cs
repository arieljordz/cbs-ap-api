using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.DTOs.Invoicing.InvRoutingFlow
{
    public class RoutingFlowLockupDto
    {
        public long InvRoutingFlowID { get; set; }
        public string? InvRoutingFlowName { get; set; }
        public string? SupplierName { get; set; }
        public bool IsActive { get; set; }

    }
}
