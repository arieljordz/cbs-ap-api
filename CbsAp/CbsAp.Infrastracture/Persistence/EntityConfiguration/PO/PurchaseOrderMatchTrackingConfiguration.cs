using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.PO;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.PO
{
    public class PurchaseOrderMatchTrackingConfiguration : IEntityTypeConfiguration<PurchaseOrderMatchTracking>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrderMatchTracking> builder)
        {
            CommonConfiguration.ConfigureAuditFields(builder);
            builder.ToTable(nameof(PurchaseOrderMatchTracking), "CBSAP");

            // fields

            builder.HasKey(m => m.PurchaseOrderMatchTrackingID);

            builder.Property(a => a.Account)
            .HasMaxLength(40);
            builder.Property(a => a.Qty)
               .HasColumnType("decimal(18,4)");

            builder.Property(a => a.RemainingQty)
              .HasColumnType("decimal(18,4)");

            builder.Property(a => a.Amount)
          .HasColumnType("decimal(18,2)");

            builder.Property(a => a.NetAmount)
            .HasColumnType("decimal(18,2)");

            builder.Property(a => a.TaxAmount)
            .HasColumnType("decimal(18,2)");

            builder.Property(i => i.MatchingStatus)
                .HasConversion<int>();


            //relationship

            builder.HasOne(pa=> pa.PurchaseOrder)
           .WithMany(pi => pi.PurchaseOrderMatchTrackings)
           .HasForeignKey(pa => pa.PurchaseOrderID)
           .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pa => pa.PurchaseOrderLine)
                .WithMany(pi => pi.PurchaseOrderMatchTrackings)
                .HasForeignKey(pa => pa.PurchaseOrderLineID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pa => pa.Invoice)
               .WithMany(pi => pi.PurchaseOrderMatchTrackings)
               .HasForeignKey(pa => pa.InvoiceID)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pa => pa.InvAllocLine)
             .WithMany(pi => pi.PurchaseOrderMatchTrackings)
             .HasForeignKey(pa => pa.InvAllocLineID)
             .OnDelete(DeleteBehavior.Restrict);



        }
    }
}
