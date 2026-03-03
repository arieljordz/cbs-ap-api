using System.Collections.Generic;

namespace CbsAp.Application.DTOs.Dashboard
{
    public class AssignedInvoiceResultDTO
    {
        public IEnumerable<AssignedInvoiceDTO> Invoices { get; set; } = new List<AssignedInvoiceDTO>();
        public int OverdueCount { get; set; }
        public int TotalCount { get; set; }
    }
}
