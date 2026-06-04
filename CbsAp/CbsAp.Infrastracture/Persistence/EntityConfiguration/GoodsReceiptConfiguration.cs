using CbsAp.Domain.Entities.GoodReceipts;
using CbsAp.Domain.Enums;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration
{
    public class GoodsReceiptConfiguration : IEntityTypeConfiguration<GoodReceipt>
    {
        public void Configure(EntityTypeBuilder<GoodReceipt> builder)
        {
            CommonConfiguration.ConfigureAuditFields(builder);

            builder.ToTable("GoodReceipts", "CBSAP");

            builder.HasKey(x => x.GoodsReceiptID)
                   .HasAnnotation("SqlServer:Identity", "1, 1");

            builder.Property(x => x.GoodsReceiptNumber)
                   .HasMaxLength(50)
                   .HasColumnType(DbDataTypes.NVARCHAR.ToString())
                   .IsRequired();

            builder.Property(x => x.DeliveryNote)
                   .HasMaxLength(250)
                   .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(x => x.Active)
                   .HasColumnType("bit");

            builder.Property(x => x.DeliveryDate)
                   .HasColumnType(DbDataTypes.DATETIMEOFFSET.ToString());

            builder.Property(x => x.EntityProfileID)
                   .IsRequired();

            builder.Property(x => x.SupplierInfoID)
                   .IsRequired();

            builder.HasOne(x => x.EntityProfile)
                   .WithMany()
                   .HasForeignKey(x => x.EntityProfileID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Supplier)
                   .WithMany()
                   .HasForeignKey(x => x.SupplierInfoID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
