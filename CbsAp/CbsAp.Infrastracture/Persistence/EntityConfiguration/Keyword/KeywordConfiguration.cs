using CbsAp.Domain.Entities.Keywords;
using CbsAp.Domain.Enums;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.System
{
    public class KeywordConfiguration : IEntityTypeConfiguration<Keyword>
    {
        public void Configure(EntityTypeBuilder<Keyword> builder)
        {
            CommonConfiguration.ConfigureAuditFields(builder);
            builder.ToTable(nameof(Keyword), "CBSAP");
            builder.HasKey(k => k.KeywordID).HasAnnotation("SqlServer:Identity", "1, 1");
            builder.Property(k => k.KeywordName)
                 .HasMaxLength(100)
                 .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(k => k.EntityProfileID)
                .IsRequired(false);

            builder.Property(k => k.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
                
            builder.Property(k => k.InvoiceRoutingFlowID);
            builder.Property(k => k.EntityProfileID);

            builder.HasOne(k => k.EntityProfile)
                  .WithMany()
                  .HasForeignKey(k => k.EntityProfileID)
                  .IsRequired(false);
                  

            builder.HasOne(k => k.InvRoutingFlow)
                  .WithMany()
                  .HasForeignKey(k => k.InvoiceRoutingFlowID)
                  .IsRequired(true);

            builder.HasMany(infolevels => infolevels.InvInfoRoutingLevels)
             .WithOne(i => i.Keyword)
             .HasForeignKey(infolevels => infolevels.KeywordID)
             .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
