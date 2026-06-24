using CbsAp.Domain.Entities.Dimensions;
using CbsAp.Domain.Entities.DimensionSetup;
using CbsAp.Domain.Enums;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration
{
    public class DimensionSetupConfiguration : IEntityTypeConfiguration<DimensionSetup>
    {
        public void Configure(EntityTypeBuilder<DimensionSetup> builder)
        {
            CommonConfiguration.ConfigureAuditFields(builder);
            builder.ToTable(nameof(DimensionSetup), "CBSAP");

            builder.HasKey(x => x.DimensionSetupId)
                   .HasAnnotation("SqlServer:Identity", "1, 1");

            builder.Property(x => x.DimensionSetupName)
                   .HasMaxLength(150)
                   .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(x => x.DisplayOrder)
                   .HasColumnType("smallint");

            builder.Property(x => x.DimensionName)
                   .HasMaxLength(150)
                   .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(x => x.Required)
                   .HasColumnType("bit");

            builder.Property(x => x.Show)
               .HasColumnType("bit");
        }
    }
}
