using CbsAp.Domain.Entities.InvoicingArchive;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.InvoicingArchive
{
    public class PurchaseOrderMatchTrackingArchiveConfiguration : IEntityTypeConfiguration<PurchaseOrderMatchTrackingArchive>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrderMatchTrackingArchive> builder)
        {
            CommonConfiguration.ConfigureAuditFields(builder);
            builder.ToTable(nameof(PurchaseOrderMatchTrackingArchive), "CBSAP");

            builder.HasKey(m => m.PurchaseOrderMatchTrackingID);

            builder.Property(m => m.Account)
                .HasMaxLength(40);

            builder.Property(m => m.Qty)
                .HasColumnType("decimal(18,4)");

            builder.Property(m => m.RemainingQty)
                .HasColumnType("decimal(18,4)");

            builder.Property(m => m.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Property(m => m.NetAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(m => m.TaxAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(m => m.MatchingStatus)
                .HasConversion<int>();

            builder.HasOne(m => m.PurchaseOrder)
                .WithMany()
                .HasForeignKey(m => m.PurchaseOrderID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.PurchaseOrderLine)
                .WithMany()
                .HasForeignKey(m => m.PurchaseOrderLineID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Invoice)
                .WithMany(i => i.PurchaseOrderMatchTrackings)
                .HasForeignKey(m => m.InvoiceID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.InvAllocLine)
                .WithMany(l => l.PurchaseOrderMatchTrackings)
                .HasForeignKey(m => m.InvAllocLineID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
