using CbsAp.Domain.Entities.InvoicingArchive;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.InvoicingArchive
{
    public class InvoiceAttachnmentArchiveConfiguration : IEntityTypeConfiguration<InvoiceAttachnmentArchive>
    {
        public void Configure(EntityTypeBuilder<InvoiceAttachnmentArchive> builder)
        {
            builder.Property(e => e.CreatedDate);

            builder.Property(e => e.CreatedBy)
                .HasMaxLength(255);

            builder.Ignore(e => e.LastUpdatedDate);
            builder.Ignore(e => e.LastUpdatedBy);

            builder.ToTable(nameof(InvoiceAttachnmentArchive), "CBSAP");

            builder.HasKey(a => a.InvoiceAttachnmentID);

            builder.Property(a => a.OriginalFileName)
                .HasMaxLength(255);

            builder.Property(a => a.StorageFileName)
                .HasMaxLength(255);

            builder.Property(a => a.FileType)
                .HasMaxLength(255);

            builder.HasOne(a => a.Invoice)
                .WithMany(i => i.InvoiceAttachnments)
                .HasForeignKey(a => a.InvoiceID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
