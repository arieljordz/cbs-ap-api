using CbsAp.Domain.Common;
using CbsAp.Domain.Entities.Invoicing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.Common
{
    public static class CommonConfiguration
    {
        public static void ConfigureAuditFields<T>(EntityTypeBuilder<T> builder) where T : BaseAuditableEntity
        {

            builder.Property(e => e.CreatedDate);

            builder.Property(e => e.CreatedBy)
                .HasMaxLength(255);

            builder.Property(e => e.LastUpdatedDate);

            builder.Property(e => e.LastUpdatedBy)
                .HasMaxLength(255);
        }

        public static void ConfigureFreeFields<T>(this OwnedNavigationBuilder<T, FreeFieldSets> builder)
        where T : class
        {
            builder.Property(f => f.FreeField1).HasColumnName("FreeField1").HasMaxLength(200);
            builder.Property(f => f.FreeField2).HasColumnName("FreeField2").HasMaxLength(200);
            builder.Property(f => f.FreeField3).HasColumnName("FreeField3").HasMaxLength(200);
        }

        public static void ConfigureSpareAmountsFields<T>(this OwnedNavigationBuilder<T, SpareAmountSets> builder)
       where T : class
        {
            builder.Property(f => f.SpareAmount1).HasColumnName("SpareAmount1").HasColumnType("decimal(18,2)");
            builder.Property(f => f.SpareAmount2).HasColumnName("SpareAmount2").HasColumnType("decimal(18,2)");
            builder.Property(f => f.SpareAmount3).HasColumnName("SpareAmount3").HasColumnType("decimal(18,2)");
        }

    }
}