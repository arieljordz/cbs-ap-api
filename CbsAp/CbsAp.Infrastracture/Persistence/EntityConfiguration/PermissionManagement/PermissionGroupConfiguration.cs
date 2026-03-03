using CbsAp.Domain.Entities.PermissionManagement;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.PermissionManagement
{
    public class PermissionGroupConfiguration : IEntityTypeConfiguration<PermissionGroup>
    {
        public void Configure(EntityTypeBuilder<PermissionGroup> builder)
        {
            IgnoreBaseAuditFieldConfiguration.IgnoreConfigureAuditFields(builder);
            builder.ToTable(nameof(PermissionGroup), "CBSAP");
            builder.HasKey(pg => pg.PermissionGroupID)
                .HasAnnotation("SqlServer:Identity", "1, 1");


            builder.HasIndex(pg => new { pg.PermissionID, pg.OperationID })
                .IsUnique(false);

            builder.Property(pg => pg.Access).IsRequired();

            builder.HasOne(pg => pg.Permission)
                .WithMany(p => p.PermissionGroups)
                .HasForeignKey(pg => pg.PermissionID)
               .IsRequired(false);

            builder.HasOne(pg => pg.Operations)
                .WithMany(op => op.PermissionGroups)
                .HasForeignKey(pg => pg.OperationID);
             

        }
    }
}