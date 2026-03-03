namespace CbsAp.Application.DTOs.Supplier
{
    public class SupplierSearchDto
    {
        public string? Entity { get; set; }
        public long SupplierInfoID { get; set; }

        public string? SupplierID { get; set; }

        public string? SupplierName { get; set; }
        public string? SupplierTaxID { get; set; }

        public string? BankAccount { get; set; }

        public string? PaymentTerms { get; set; }

        public string? InvRoutingFlowName { get; set; }

        public bool IsActive { get; set; }
    }
}