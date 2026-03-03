namespace CbsAp.Application.DTOs.Invoicing.Accounts
{
    public class AccountExportDto
    {
        public long AccountID { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public string EntityName { get; set; } = string.Empty;
        public string Active { get; set; } = string.Empty;
        public string? Dimension1 { get; set; }
        public string? Dimension2 { get; set; }
        public string? Dimension3 { get; set; }
        public string? Dimension4 { get; set; }
        public string? Dimension5 { get; set; }
        public string? Dimension6 { get; set; }
        public string? Dimension7 { get; set; }
        public string? Dimension8 { get; set; }
        public required string IsTaxCodeMandatory { get; set; }
    }
}
