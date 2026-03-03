using CbsAp.Domain.Entities.PermissionManagement;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.PermissionManagement
{
    public class OperationsConfiguration : IEntityTypeConfiguration<Operation>
    {
        public void Configure(EntityTypeBuilder<Operation> builder)
        {
            builder.ToTable(nameof(Operation), "CBSAP");
            IgnoreBaseAuditFieldConfiguration.IgnoreConfigureAuditFields(builder);

            builder.HasKey(o => o.OperationID);
            builder.Property(o => o.OperationID)
              .ValueGeneratedOnAdd();

            builder.Property(o => o.OperationName).HasMaxLength(100).IsRequired(false);
            builder.Property(o => o.Panel).HasMaxLength(100).IsRequired(false);
            builder.Property(o => o.ApplyOperationIn).HasMaxLength(100).IsRequired(false);

            builder.HasOne(ctrl => ctrl.ControlElement)
                .WithOne(m => m.Operation)
                .HasForeignKey<ControlElement>(ctrl => ctrl.OperationID)
                 .OnDelete(DeleteBehavior.Restrict);
         
            builder.HasMany(o => o.Menus)
           .WithOne(m => m.Operation)
           .HasForeignKey(m => m.OperationID)
           .OnDelete(DeleteBehavior.Restrict);
        }

    }
}