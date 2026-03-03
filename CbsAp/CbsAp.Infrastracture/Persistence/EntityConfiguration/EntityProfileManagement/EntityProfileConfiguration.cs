using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Enums;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.EntityProfileManagement
{
    public class EntityProfileConfiguration : IEntityTypeConfiguration<EntityProfile>
    {
        public void Configure(EntityTypeBuilder<EntityProfile> builder)
        {
            CommonConfiguration.ConfigureAuditFields(builder);
            builder.ToTable(nameof(EntityProfile), "CBSAP");
            builder.HasKey(pg => pg.EntityProfileID);
            builder.Property(pg => pg.EntityProfileID)
                .ValueGeneratedOnAdd();

            builder.Property(ef => ef.EntityName)
                .HasMaxLength(255)
                .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(ef => ef.EntityCode)
               .HasMaxLength(100)
               .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(ef => ef.EmailAddress)
           .HasMaxLength(100)
           .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(ef => ef.TaxID)
             .HasMaxLength(100)
             .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(ef => ef.ERPFinanceSystem)
               .HasMaxLength(100)
               .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(e => e.InvAllowPresetAmount)
            .HasColumnType("bit");

            builder.Property(e => e.InvAllowPresetDimension)
                .HasColumnType("bit");

            builder.Property(e => e.TaxDollarAmt)
               .HasColumnType("decimal(18,2)");
            builder.Property(e => e.TaxPercentageAmt)
                .HasColumnType("decimal(18,2)");

            //relationships

            builder.HasMany(e => e.MatchingConfigs)
              .WithOne(m => m.EntityProfile)
              .HasForeignKey(m => m.EntityProfileID);
        }
    }
}