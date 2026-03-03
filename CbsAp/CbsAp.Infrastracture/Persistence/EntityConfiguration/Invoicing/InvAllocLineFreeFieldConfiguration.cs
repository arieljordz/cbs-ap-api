using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.Invoicing
{
    public class InvAllocLineFreeFieldConfiguration : IEntityTypeConfiguration<InvAllocLineFreeField>
    {
        public void Configure(EntityTypeBuilder<InvAllocLineFreeField> builder)
        {
            IgnoreBaseAuditFieldConfiguration.IgnoreConfigureAuditFields(builder);
            builder.ToTable(nameof(InvAllocLineFreeField), "CBSAP");
            builder.HasKey(f => f.InvAllocLineFieldID);

            builder.Property(f => f.FieldKey)
                .HasMaxLength(50);

            builder.Property(f => f.FieldValue)
                .HasMaxLength(200);

            builder.HasOne(f => f.AllocationLine)
                .WithMany(a => a.FreeFields)
                .HasForeignKey(f => f.InvAllocLineID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
