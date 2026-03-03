using CbsAp.Domain.Entities.PermissionManagement;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.PermissionManagement
{
    public class RolePermissionGroupConfiguration :
        IEntityTypeConfiguration<RolePermissionGroup>
    {
        public void Configure(EntityTypeBuilder<RolePermissionGroup> builder)
        {
            IgnoreBaseAuditFieldConfiguration.IgnoreConfigureAuditFields(builder);
            builder.ToTable(nameof(RolePermissionGroup), "CBSAP");
            builder.HasKey(rpg => rpg.RolePermissionGroupID)
                .HasAnnotation("SqlServer:Identity", "1, 1");

            builder.HasAlternateKey(rpg => new { rpg.RoleID, rpg.PermissionID });

            builder.HasOne(rpg => rpg.Role)
                .WithMany(r => r.RolePermissionGroups)
                .HasForeignKey(rpg => rpg.RoleID);

            builder.HasOne(rpg => rpg.Permission)
                .WithMany(pg => pg.RolePermissionGroups)
                .HasForeignKey(rpg => rpg.PermissionID);
        }
    }
}