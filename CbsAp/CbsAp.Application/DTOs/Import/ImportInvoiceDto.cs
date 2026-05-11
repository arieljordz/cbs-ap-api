using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.DTOs.Import
{
    public class ImportInvoiceDto
    {
        public string? InvoiceNo { get; set; }
        public string? InvoiceDate { get; set; }
        public string? ScanDate { get; set; }
        public int MapId { get; set; }
        public string? CompanyCode { get; set; }
        public string SupplierId { get; set; }
        public string? PoNo { get; set; }
        public string? GrNo { get; set; }
        public string? Currency { get; set; }
        public decimal NetAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string PdfFilePath { get; set; }
        public string Keyword { get; set; }

    }
}
