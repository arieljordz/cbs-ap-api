namespace CbsAp.Application.DTOs.Supplier
{
    public class ExportSupplierDto
    {
        public string? Entity { get; set; }

        public string? SupplierID { get; set; }

        public string? SupplierName { get; set; }

        public string? SupplierTaxID { get; set; }

        public string? BankAccount { get; set; }

        public string? PaymentTerms { get; set; }

        public string? InvoiceRoutingFlow { get; set; }

        public string ActiveStatus { get; set; }
    }
}