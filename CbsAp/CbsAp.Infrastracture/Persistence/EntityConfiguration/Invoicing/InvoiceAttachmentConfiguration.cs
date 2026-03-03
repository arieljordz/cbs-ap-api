using CbsAp.Domain.Entities.Invoicing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.Invoicing
{
    public class InvoiceAttachmentConfiguration : IEntityTypeConfiguration<InvoiceAttachnment>
    {
        public void Configure(EntityTypeBuilder<InvoiceAttachnment> builder)
        {
            builder.Property(e => e.CreatedDate);

            builder.Property(e => e.CreatedBy)
                .HasMaxLength(255);

            builder.Ignore(e => e.LastUpdatedDate);
            builder.Ignore(e => e.LastUpdatedBy);

            builder.ToTable(nameof(InvoiceAttachnment), "CBSAP");

            builder.HasKey(i => i.InvoiceAttachnmentID);

            builder.Property(i => i.OriginalFileName)
               .HasMaxLength(255);

            builder.Property(i => i.StorageFileName)
              .HasMaxLength(255);

            builder.Property(i => i.FileType)
            .HasMaxLength(255);

            builder.HasOne(f => f.Invoice)
                .WithMany(i => i.InvoiceAttachnments)
                .HasForeignKey(f => f.InvoiceID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}