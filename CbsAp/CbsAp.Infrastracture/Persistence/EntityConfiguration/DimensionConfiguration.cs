using CbsAp.Domain.Entities.Dimensions;
using CbsAp.Domain.Enums;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration
{
    public class DimensionConfiguration : IEntityTypeConfiguration<Dimension>
    {
        public void Configure(EntityTypeBuilder<Dimension> builder)
        {
            CommonConfiguration.ConfigureAuditFields(builder);
            builder.ToTable(nameof(Dimension), "CBSAP");

            builder.HasKey(x => x.DimensionID)
                   .HasAnnotation("SqlServer:Identity", "1, 1");

            builder.Property(x => x.DimensionCode)
                   .HasColumnName("Dimension")
                   .HasMaxLength(15)
                   .HasColumnType(DbDataTypes.NVARCHAR.ToString())
                   .IsRequired();

            builder.Property(x => x.Name)
                   .HasMaxLength(50)
                   .HasColumnType(DbDataTypes.NVARCHAR.ToString())
                   .IsRequired();

            builder.Property(x => x.IsActive)
                   .HasColumnType("bit");

            builder.Property(x => x.FreeField1)
                   .HasMaxLength(90)
                   .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(x => x.FreeField2)
                   .HasMaxLength(90)
                   .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(x => x.FreeField3)
                   .HasMaxLength(90)
                   .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(x => x.EntityProfileID)
                   .IsRequired();

            builder.HasOne(x => x.EntityProfile)
                   .WithMany()
                   .HasForeignKey(x => x.EntityProfileID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
