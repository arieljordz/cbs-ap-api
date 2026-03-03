using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.Invoicing
{
    public class InvAllocLineDimensionConfiguration : IEntityTypeConfiguration<InvAllocLineDimension>
    {
        public void Configure(EntityTypeBuilder<InvAllocLineDimension> builder)
        {
            IgnoreBaseAuditFieldConfiguration.IgnoreConfigureAuditFields(builder);
            builder.ToTable(nameof(InvAllocLineDimension), "CBSAP");
            builder.HasKey(d => d.InvAllocLineDimensionID);

            builder.Property(d => d.DimensionKey)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(d => d.DimensionValue)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasOne(d => d.AllocationLine)
                .WithMany(a => a.Dimensions)
                .HasForeignKey(d => d.InvAllocLineID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}