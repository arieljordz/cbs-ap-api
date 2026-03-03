namespace CbsAp.Application.DTOs.Invoicing.Invoice
{
    public class InvAllocLineDto
    {
        public long InvAllocLineID { get; set; }

        public long? InvoiceID { get; set; }

        public long? LineNo { get; set; }

        public string? PoNo { get; set; }

        public string? PoLineNo { get; set; }

        public long? Account { get; set; }

        public string? LineDescription { get; set; }

        public decimal Qty { get; set; }

        public decimal LineNetAmount { get; set; }

        public decimal LineTaxAmount { get; set; }

        public decimal LineAmount { get; set; }

        public string? Currency { get; set; }

        public long? TaxCodeID { get; set; }

        public string? LineApproved { get; set; }

        public string? Note { get; set; }

        public List<InvAllocLineFreeFieldDto> FreeFields { get; set; } = new();

        public List<InvAllocLineDimensionDto> Dimensions { get; set; } = new();
    }

    public class InvAllocLineFreeFieldDto
    {
        public string? FieldKey { get; set; }

        public string? FieldValue { get; set; }
    }

    public class InvAllocLineDimensionDto
    {
        public string DimensionKey { get; set; }

        public string DimensionValue { get; set; }
    }

    public class InvAllocEntryDto
    {
        public long InvAllocLineID { get; set; }

        public long? InvoiceID { get; set; }

        public long? LineNo { get; set; }
        public string? PoLineNo { get; set; }

        public string? PoNo { get; set; }

        public decimal Qty { get; set; }
        public long? Account { get; set; }

        public string? LineDescription { get; set; }

        public string? Note { get; set; }

        public decimal LineNetAmount { get; set; }

        public long? TaxCodeID { get; set; }

        public decimal LineTaxAmount { get; set; }

        public decimal LineAmount { get; set; }
        public bool IsFromPOMatching { get; set; }
    }
}