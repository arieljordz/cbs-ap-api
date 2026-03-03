using CbsAp.Domain.Entities.InvoicingArchive;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.InvoicingArchive
{
    public class InvoiceCommentArchiveConfiguration : IEntityTypeConfiguration<InvoiceCommentArchive>
    {
        public void Configure(EntityTypeBuilder<InvoiceCommentArchive> builder)
        {
            builder.Property(e => e.CreatedDate);

            builder.Property(e => e.CreatedBy)
                .HasMaxLength(255);

            builder.Ignore(e => e.LastUpdatedDate);
            builder.Ignore(e => e.LastUpdatedBy);

            builder.ToTable(nameof(InvoiceCommentArchive), "CBSAP");

            builder.HasKey(c => c.InvoiceCommentID);

            builder.Property(c => c.Comment)
                .HasMaxLength(1000);

            builder.HasOne(c => c.Invoice)
                .WithMany(i => i.InvoiceComments)
                .HasForeignKey(c => c.InvoiceID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
