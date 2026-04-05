using CbsAp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.DTOs.Invoicing.InvInfoRoutingLevel
{
    public class InvInfoRoutingLevelDto
    {
        public long InvInfoRoutingLevelID { get; set; }
        public long? InvRoutingFlowID { get; set; }
        public long? InvoiceID { get; set; } 
        public long? SupplierInfoID { get; set; }  
        public long? KeywordID { get; set; }
        public long RoleID { get; set; }
        public int Level { get; set; }
        public InvFlowStatus FlowStatus { get; set; }
    }
}
