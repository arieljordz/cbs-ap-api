using CbsAp.Domain.Entities.TaxCodes;
using CbsAp.Domain.Enums;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration
{
    public class TaxCodesConfiguration : IEntityTypeConfiguration<TaxCode>
    {
        public void Configure(EntityTypeBuilder<TaxCode> builder)
        {
            CommonConfiguration.ConfigureAuditFields(builder);

            builder.ToTable(nameof(TaxCode), "CBSAP");
            builder.HasKey(x => x.TaxCodeID).HasAnnotation("SqlServer:Identity", "1, 1");

            builder.Property(x => x.EntityID);
            builder.Property(x => x.TaxCodeName)
                 .HasMaxLength(100)
                 .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(x => x.Code)
                .HasMaxLength(50)
                .HasColumnType(DbDataTypes.NVARCHAR.ToString());
            builder.Property(p => p.TaxRate)
                  .HasColumnType("decimal(18,2)");

            //relationship
            builder.HasOne(x => x.EntityProfile)
                    .WithMany()
                    .HasForeignKey(ur => ur.EntityID)
                   .IsRequired(false);

            builder.HasMany(a => a.PurchaseOrderLines)
             .WithOne(a => a.TaxCode)
             .HasForeignKey(a => a.TaxCodeID)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}