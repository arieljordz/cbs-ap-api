using CbsAp.Domain.Common;

namespace CbsAp.Domain.Entities.InvoicingArchive
{
    public class InvoiceCommentArchive : BaseAuditableEntity
    {
        public long InvoiceCommentID { get; set; }
        public string? Comment { get; set; }
        public long InvoiceID { get; set; }
        public virtual InvoiceArchive? Invoice { get; set; }
    }
}
