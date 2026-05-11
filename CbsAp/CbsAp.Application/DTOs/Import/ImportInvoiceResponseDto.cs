using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.DTOs.Import
{
    public class ImportInvoiceResponseDto
    {
        public string InvoiceNo { get; set; }
        public int? InvoiceId { get; set; }
        public string PdfFilePath { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
