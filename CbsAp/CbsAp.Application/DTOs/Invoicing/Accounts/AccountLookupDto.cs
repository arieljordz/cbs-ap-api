namespace CbsAp.Application.DTOs.Invoicing.Accounts
{
    public class AccountLookupDto
    {
        public long AccountID { get; set; }

        public string AccountName { get; set; }
    }

    public class SearchAccountLookupDto : AccountLookupDto
    {
        public string EntityName { get; set; }
        public string Active { get; set; }
        public string? Dimension1 { get; set; }

        public string? Dimension2 { get; set; }

        public string? Dimension3 { get; set; }

        public string? Dimension4 { get; set; }

        public string? Dimension5 { get; set; }

        public string? Dimension6 { get; set; }

        public string? Dimension7 { get; set; }

        public string? Dimension8 { get; set; }

        public bool? IsTaxCodeMandatory { get; set; }
    }
}