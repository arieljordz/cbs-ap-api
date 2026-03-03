namespace CbsAp.Application.DTOs.Dashboard
{
    public class AssignedInvoiceDTO
    {
        public long InvoiceId { get; set; }
        public string? SupplierName { get; set; }
        public string Queue { get; set; } = "";
        public DateTime? InvoiceDate { get; set; }
        public string? InvoiceNumber { get; set; }
        public DateTime? DueDate { get; set; }
        public Decimal Amount { get; set; }
        public string? AssignedRole { get; set; }

    }
}