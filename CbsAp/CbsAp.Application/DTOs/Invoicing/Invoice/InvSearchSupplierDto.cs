namespace CbsAp.Application.DTOs.Invoicing.Invoice
{
    public class InvSearchSupplierDto
    {
        public long SupplierInfoID { get; set; }

        public string? SupplierID { get; set; }

        public string? SupplierTaxID { get; set; }

        public string? Entity { get; set; }

        public string? SupplierName { get; set; }

        public long? InvoiceRoutingFlowID { get; set; }

        public string? InvoiceRoutingFlowName { get; set; }

        public bool IsActive { get; set; }
    }
}