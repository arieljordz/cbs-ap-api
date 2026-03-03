namespace CbsAp.Application.DTOs.Keyword
{
    public class KeywordDTO
    {
        public long KeywordID { get; set; }
        public string KeywordName { get; set; } = string.Empty;
        public long? EntityProfileID { get; set; }
        public string EntityName { get; set; } = string.Empty;
        public long InvoiceRoutingFlowID { get; set; }
        public string InvoiceRoutingFlowName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
