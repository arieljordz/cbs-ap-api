using CbsAp.Domain.Entities.InvoicingArchive;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.InvoicingArchive
{
    public class InvoiceActivityLogArchiveConfiguration : IEntityTypeConfiguration<InvoiceActivityLogArchive>
    {
        public void Configure(EntityTypeBuilder<InvoiceActivityLogArchive> builder)
        {
            builder.Property(e => e.CreatedDate);

            builder.Property(e => e.CreatedBy)
                .HasMaxLength(255);

            builder.Ignore(e => e.LastUpdatedDate);
            builder.Ignore(e => e.LastUpdatedBy);

            builder.ToTable(nameof(InvoiceActivityLogArchive), "CBSAP");

            builder.HasKey(l => l.ActivityLogID);

            builder.Property(l => l.Reason)
                .HasMaxLength(1000);

            builder.Property(l => l.Action)
                .HasConversion<int>();

            builder.Property(l => l.PreviousStatus)
                .HasConversion<int>();

            builder.Property(l => l.CurrentStatus)
                .HasConversion<int>();

            builder.HasOne(l => l.Invoice)
                .WithMany(i => i.InvoiceActivityLog)
                .HasForeignKey(l => l.InvoiceID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
