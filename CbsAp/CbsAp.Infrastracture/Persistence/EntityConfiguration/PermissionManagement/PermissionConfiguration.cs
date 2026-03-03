using CbsAp.Domain.Entities.PermissionManagement;
using CbsAp.Domain.Entities.RoleManagement;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.PermissionManagement
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            CommonConfiguration.ConfigureAuditFields(builder);
            builder.ToTable(nameof(Permission), "CBSAP");
            builder.HasKey(x => x.PermissionID)
                .HasAnnotation("SqlServer:Identity", "1, 1");
            builder.Property(p => p.PermissionName).HasMaxLength(100).IsRequired();
            builder.Property(p => p.IsActive).HasDefaultValue(true);
        }
    }
}