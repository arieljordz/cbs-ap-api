using CbsAp.Domain.Common;
using CbsAp.Domain.Common.Interfaces;

namespace CbsAp.Domain.Entities.Dashboard
{
    public class Notice : BaseAuditableEntity
    {
        public long NoticeID { get; set; }
        public string? Heading { get; set; }
        public string? Message { get; set; }

    }
}