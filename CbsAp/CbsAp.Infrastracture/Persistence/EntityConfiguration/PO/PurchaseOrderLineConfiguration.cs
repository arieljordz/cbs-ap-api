using CbsAp.Domain.Entities.PO;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.PO
{
    public class PurchaseOrderLineConfiguration : IEntityTypeConfiguration<PurchaseOrderLine>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrderLine> builder)
        {
            CommonConfiguration.ConfigureAuditFields(builder);
            builder.ToTable(nameof(PurchaseOrderLine), "CBSAP");

            //fields
            builder.HasKey(l => l.PurchaseOrderLineID);
            builder.Property(l => l.LineNo);

            builder.Property(l => l.Item)
                .HasMaxLength(90);

            builder.Property(l => l.Description)
                .HasMaxLength(90);

            builder.Property(a => a.Qty)
               .HasColumnType("decimal(18,4)");

            builder.Property(l => l.Unit)
              .HasMaxLength(12);

            builder.Property(a => a.Price)
             .HasColumnType("decimal(18,2)");

            builder.Property(a => a.InvoicedPrice)
            .HasColumnType("decimal(18,2)");

            builder.Property(a => a.Amount)
            .HasColumnType("decimal(18,2)");

            builder.Property(a => a.NetAmount)
            .HasColumnType("decimal(18,2)");

            builder.Property(a => a.TaxAmount)
            .HasColumnType("decimal(18,2)");

            //relationship
            builder.HasOne(l => l.PurchaseOrder)
                .WithMany(po => po.PurchaseOrderLines)
                .HasForeignKey(l => l.PurchaseOrderID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(line => line.TaxCode)
                .WithMany(t => t.PurchaseOrderLines)
                .HasForeignKey(line => line.TaxCodeID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(line => line.Account)
                .WithMany(t => t.PurchaseOrderLines)
                .HasForeignKey(line => line.AccountID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(po => po.PurchaseOrderMatchTrackings)
                .WithOne(po => po.PurchaseOrderLine)
                .HasForeignKey(poline => poline.PurchaseOrderID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}