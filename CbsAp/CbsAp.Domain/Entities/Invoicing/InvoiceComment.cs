using CbsAp.Domain.Common;

namespace CbsAp.Domain.Entities.Invoicing
{
    public class InvoiceComment : BaseAuditableEntity
    {
        public long InvoiceCommentID { get; set; }
        public string? Comment { get; set; }
        public long InvoiceID { get; set; }
        public virtual Invoice? Invoice { get; set; }
    }
}