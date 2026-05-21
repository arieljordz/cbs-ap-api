using CbsAp.Domain.Entities.GoodReceipts;
using CbsAp.Domain.Enums;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration
{
    public class GoodsReceiptLineConfiguration : IEntityTypeConfiguration<GoodsReceiptLine>
    {
        public void Configure(EntityTypeBuilder<GoodsReceiptLine> builder)
        {
            CommonConfiguration.ConfigureAuditFields(builder);

            builder.ToTable("GoodsReceiptLine", "CBSAP");

            builder.HasKey(x => x.GoodsReceiptLineID)
                   .HasAnnotation("SqlServer:Identity", "1, 1");

            builder.Property(x => x.LineNo)                   
                   .IsRequired();

            builder.Property(x => x.GoodsReceiptID)
                   .IsRequired();

            builder.Property(x => x.Qty)
                   .HasColumnType("decimal(18,4)");

            builder.Property(x => x.Amount)
                   .HasColumnType("decimal(18,4)");

            builder.Property(x => x.ReceiptNo)
                   .HasColumnType(DbDataTypes.NVARCHAR.ToString())
                   .HasMaxLength(50);

            builder.Property(x => x.PurchaseOrderNo)
                   .HasColumnType(DbDataTypes.NVARCHAR.ToString())
                   .HasMaxLength(50);

            builder.Property(x => x.SupplierNo)
                   .HasColumnType(DbDataTypes.NVARCHAR.ToString())
                   .HasMaxLength(50);

            builder.Property(x => x.FreeField1)
                   .HasMaxLength(200);

            builder.Property(x => x.FreeField2)
                   .HasMaxLength(200);

            builder.Property(x => x.FreeField3)
                   .HasMaxLength(200);

            builder.Property(x => x.InvoiceStatus)
                   .IsRequired();
        }
    }
}
