using CbsAp.Domain.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.EntityProfileManagement
{
    public class EntityMatchingConfiguration : IEntityTypeConfiguration<EntityMatchingConfig>
    {
        public void Configure(EntityTypeBuilder<EntityMatchingConfig> builder)
        {
            builder.ToTable(nameof(EntityMatchingConfig), "CBSAP");
            builder.Ignore(ur => ur.CreatedBy);
            builder.Ignore(ur => ur.CreatedDate);
            builder.Ignore(ur => ur.LastUpdatedBy);
            builder.Ignore(ur => ur.LastUpdatedDate);

            builder.HasKey(e => e.MatchingConfigID);
            builder.Property(e => e.MatchingConfigID).ValueGeneratedOnAdd();

            builder.Property(e => e.ConfigType)
            .HasConversion<string>()
            .HasMaxLength(10)
            .IsRequired();

            builder.Property(e => e.MatchingLevel)
            .HasMaxLength(50);

            builder.Property(e => e.InvoiceMatchBasis)
                .HasMaxLength(50);

            builder.Property(e => e.DollarAmt)
                .HasColumnType("decimal(18,2)");
            builder.Property(e => e.PercentageAmt)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(e => e.EntityProfile)
                   .WithMany(p => p.MatchingConfigs)
                   .HasForeignKey(e => e.EntityProfileID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}