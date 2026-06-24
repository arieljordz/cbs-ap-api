using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.Reports
{
    public class ExportMyInvoiceQuery : IQuery<ResponseResult<byte[]>>
    {
        public string? SupplierName { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? PONumber { get; set; }
        public int RoleId { get; set; } = 0;

        //Advance Search - Supplier Information
        public string? PaymentTerm { get; set; }
        public string? SupplierNo { get; set; }
        public string? SuppABN { get; set; }
        public string? SuppBankAccount { get; set; }

        //Advance Search - Invoice Detail
        public int? EntityProfileID { get; set; }

        public string? GrNo { get; set; }

        public string? DateRangeInvoiceDate { get; set; }

        public DateTime? StartInvoiceDate { get; set; }

        public DateTime? EndInvoiceDate { get; set; }

        public string? DateRangeDueDate { get; set; }

        public DateTime? StartDueDate { get; set; }

        public DateTime? EndDueDate { get; set; }

        public int? DaystillDue { get; set; }

        //Advance Search - Invoice Amounts

        public decimal? NetAmount { get; set; }
        public int? TaxCodeID { get; set; }
        public decimal? TaxAmount { get; set; }
        public string? Currency { get; set; }
        public decimal? TotalAmount { get; set; }

        //Advance Search - Routing flow
        public string? InvRoutingFlowName { get; set; }

        public string? NextRole { get; set; }
        public string? Keyword { get; set; }

        //Advance Search - Transaction Information
        public string? MapID { get; set; }
        public string? DateRangeScanDate { get; set; }

        public DateTime? StartScanDate { get; set; }

        public DateTime? EndScanDate { get; set; }

        public string? InvoiceID { get; set; }


    }
}