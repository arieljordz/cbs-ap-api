namespace CbsAp.Application.DTOs.Entity
{
    public class EntityDto
    {
        public long EntityProfileID { get; set; }

        public string EntityName { get; set; }

        public string EntityCode { get; set; }

        public string EmailAddress { get; set; }

        public string TaxID { get; set; }

        public string ERPFinanceSystem { get; set; }

        public int? DefaultInvoiceDueInDays { get; set; }

        public bool? InvAllowPresetAmount { get; set; }

        public bool? InvAllowPresetDimension { get; set; }

        public decimal? TaxDollarAmt { get; set; }

        public decimal? TaxPercentageAmt { get; set; }

        public int? InvDueDateCalculation { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }

        public List<EntityMatchingConfigDto>? MatchingConfigs { get; set; } = new List<EntityMatchingConfigDto>();
        public bool AutomaticGoodsDelivered { get; set; }
    }
}
