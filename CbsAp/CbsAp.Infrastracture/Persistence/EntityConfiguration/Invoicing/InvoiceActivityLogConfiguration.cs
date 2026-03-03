using CbsAp.Domain.Entities.Invoicing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.Invoicing
{
    public class InvoiceActivityLogConfiguration : IEntityTypeConfiguration<InvoiceActivityLog>
    {
        public void Configure(EntityTypeBuilder<InvoiceActivityLog> builder)
        {
            builder.Property(e => e.CreatedDate);

            builder.Property(e => e.CreatedBy)
                .HasMaxLength(255);

            builder.Ignore(e => e.LastUpdatedDate);
            builder.Ignore(e => e.LastUpdatedBy);

            builder.ToTable(nameof(InvoiceActivityLog), "CBSAP");

            builder.HasKey(i => i.ActivityLogID);

            builder.Property(i => i.CurrentStatus)
             .HasConversion<int>();

            builder.Property(i => i.PreviousStatus)
             .HasConversion<int>();

            builder.Property(i => i.Reason)
              .HasMaxLength(255);

            builder.Property(i => i.Action)
               .HasConversion<int>();

            builder.HasOne(f => f.Invoice)
                .WithMany(i => i.InvoiceActivityLog)
                .HasForeignKey(f => f.InvoiceID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
