using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.Invoicing
{
    public class InvRoutingFlowLevelsConfiguration : IEntityTypeConfiguration<InvRoutingFlowLevels>
    {
        public void Configure(EntityTypeBuilder<InvRoutingFlowLevels> builder)
        {
            IgnoreBaseAuditFieldConfiguration.IgnoreConfigureAuditFields(builder);
            builder.ToTable(nameof(InvRoutingFlowLevels), "CBSAP");
            builder.HasKey(a => a.InvRoutingFlowLevelID);
            builder.Property(a => a.InvRoutingFlowLevelID).ValueGeneratedOnAdd();

            builder.Property(a => a.Level).IsRequired();

            builder.HasOne(x => x.InvRoutingFlow)
              .WithMany(x => x.Levels)
              .HasForeignKey(x => x.InvRoutingFlowID)
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
