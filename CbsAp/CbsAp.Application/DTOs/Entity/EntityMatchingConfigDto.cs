namespace CbsAp.Application.DTOs.Entity
{
    public class EntityMatchingConfigDto
    {
        public long MatchingConfigID { get; set; }
        public long EntityProfileID { get; set; }
        public string ConfigType { get; set; } = null!;
        public string? MatchingLevel { get; set; }
        public string? InvoiceMatchBasis { get; set; }


        public decimal? DollarAmt { get; set; }
        public decimal? PercentageAmt { get; set; }
    }
}