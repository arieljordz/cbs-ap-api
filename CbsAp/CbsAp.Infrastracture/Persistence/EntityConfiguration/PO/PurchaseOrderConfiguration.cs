using CbsAp.Domain.Entities.PO;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.PO
{
    public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
        {
            CommonConfiguration.ConfigureAuditFields(builder);
            builder.ToTable(nameof(PurchaseOrder), "CBSAP");

            //fields
            builder.HasKey(p => p.PurchaseOrderID);

            builder.Property(p => p.PoNo)
                .HasMaxLength(50);

            builder.Property(po => po.Currency)
                .HasMaxLength(10);

            builder.Property(po => po.SupplierTaxID)
             .HasMaxLength(40);

            builder.Property(po => po.SupplierNo)
             .HasMaxLength(40);

            builder.Property(po => po.Currency)
               .HasMaxLength(3);

            builder.Property(po => po.NetAmount)
               .HasColumnType("decimal(18,2)");

            builder.Property(po => po.TaxAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(po => po.TotalAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(po => po.IsActive)
               .IsRequired();

            builder.Property(po => po.MatchReference1)
               .HasMaxLength(90);

            builder.Property(po => po.MatchReference2)
               .HasMaxLength(90);

            builder.Property(po => po.Note)
               .HasMaxLength(1000);

            builder.Property(po => po.FreeField1)
            .HasMaxLength(200);
            builder.Property(po => po.FreeField2)
           .HasMaxLength(200);
            builder.Property(po => po.FreeField3)
           .HasMaxLength(200);

            //relationships
            builder.HasOne(po => po.EntityProfile)
                .WithMany()
                .HasForeignKey(po => po.EntityProfileID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(po => po.SupplierInfo)
                .WithMany()
                .HasForeignKey(po => po.SupplierInfoID)
                .OnDelete(DeleteBehavior.Restrict);



            builder.HasMany(po => po.PurchaseOrderLines)
                .WithOne(po => po.PurchaseOrder)
                .HasForeignKey(poline => poline.PurchaseOrderID)
                .OnDelete(DeleteBehavior.Restrict);



            builder.HasMany(po => po.PurchaseOrderMatchTrackings)
                .WithOne(po => po.PurchaseOrder)
                .HasForeignKey(poline => poline.PurchaseOrderID)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
