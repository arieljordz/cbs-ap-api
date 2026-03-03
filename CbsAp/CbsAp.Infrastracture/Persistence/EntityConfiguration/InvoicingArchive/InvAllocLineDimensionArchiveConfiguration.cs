using CbsAp.Domain.Entities.InvoicingArchive;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.InvoicingArchive
{
    public class InvAllocLineDimensionArchiveConfiguration : IEntityTypeConfiguration<InvAllocLineDimensionArchive>
    {
        public void Configure(EntityTypeBuilder<InvAllocLineDimensionArchive> builder)
        {
            IgnoreBaseAuditFieldConfiguration.IgnoreConfigureAuditFields(builder);
            builder.ToTable(nameof(InvAllocLineDimensionArchive), "CBSAP");

            builder.HasKey(d => d.InvAllocLineDimensionID);

            builder.Property(d => d.DimensionKey)
                .HasMaxLength(100);

            builder.Property(d => d.DimensionValue)
                .HasMaxLength(200);

            builder.HasOne(d => d.AllocationLine)
                .WithMany(a => a.Dimensions)
                .HasForeignKey(d => d.InvAllocLineID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
