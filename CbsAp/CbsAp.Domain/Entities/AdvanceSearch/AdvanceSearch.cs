using CbsAp.Domain.Common;
using CbsAp.Domain.Common.Interfaces;

namespace CbsAp.Domain.Entities.AdvanceSearch
{
    public class AdvanceSearch : BaseAuditableEntity
    {
        public long AdvanceSearchId { get; set; }
        public string UserId { get; set; }
        public string? JsonFilter { get; set; }
        public string? FormName { get; set; }

    }
}