
namespace CbsAp.Application.DTOs.Keyword
{
    public class CreateUpdateKeywordDTO
    {
        public long KeywordID { get; set; }
        public long? EntityProfileID { get; set; }
        public long InvoiceRoutingFlowID { get; set; }
        public string KeywordName { get; set; } = "";
        public bool IsActive { get; set; }
    }
}
