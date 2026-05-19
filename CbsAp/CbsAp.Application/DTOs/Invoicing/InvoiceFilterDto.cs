using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CbsAp.Application.DTOs.Invoicing.Invoice;

namespace CbsAp.Application.DTOs.Invoicing
{
    public class InvoiceFilterDto : InvoiceSearchBaseDto
    {
        public long InvoiceID { get; set; }

        public string? Entity { get; set; }

        public string? SuppName { get; set; }

        public DateTimeOffset? InvoiceDate { get; set; }

        public string? InvoiceNo { get; set; }

        public string? PoNo { get; set; }

        public DateTimeOffset? DueDate { get; set; }

        public string? GrossAmount { get; set; }
        public string? NextRole { get; set; } = string.Empty;
        public string ExceptionReason { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public DateTimeOffset? CreatedDate { get; set; }
        public DateTimeOffset? LastUpdatedDate { get; set; }
    }
}
