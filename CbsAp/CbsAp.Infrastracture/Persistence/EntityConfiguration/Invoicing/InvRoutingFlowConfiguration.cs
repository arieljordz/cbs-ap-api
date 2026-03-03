using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Enums;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.Invoicing
{
    public class InvRoutingFlowConfiguration : IEntityTypeConfiguration<InvRoutingFlow>
    {
        public void Configure(EntityTypeBuilder<InvRoutingFlow> builder)
        {
            CommonConfiguration.ConfigureAuditFields(builder);
            builder.ToTable(nameof(InvRoutingFlow), "CBSAP");
            builder.HasKey(a => a.InvRoutingFlowID);
            builder.Property(a => a.InvRoutingFlowID).ValueGeneratedOnAdd();

            builder.Property(a => a.InvRoutingFlowName)
             .HasMaxLength(30)
             .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(x => x.MatchReference)
            .HasMaxLength(100);

            builder.Property(a => a.IsActive)
                .IsRequired();

            // Relationships
            builder.HasOne(x => x.SupplierInfo)
                .WithMany()
                .HasForeignKey(x => x.SupplierInfoID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.EntityProfile)
            .WithMany()
            .HasForeignKey(x => x.EntityProfileID)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Levels)
           .WithOne(x => x.InvRoutingFlow)
           .HasForeignKey(x => x.InvRoutingFlowID)
           .OnDelete(DeleteBehavior.Cascade);


            builder.HasMany(x => x.InvInfoRoutingLevels)
            .WithOne(x => x.InvRoutingFlow)
            .HasForeignKey(x=> x.InvRoutingFlowID)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}