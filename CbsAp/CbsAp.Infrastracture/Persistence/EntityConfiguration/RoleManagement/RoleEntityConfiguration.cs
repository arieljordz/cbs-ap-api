using CbsAp.Domain.Entities.RoleManagement;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.RoleManagement
{
    public class RoleEntityConfiguration : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            IgnoreBaseAuditFieldConfiguration.IgnoreConfigureAuditFields(builder);
            builder.ToTable(nameof(RoleEntity), "CBSAP");
            builder.HasKey(re => re.RoleEntityID)
              .HasAnnotation("SqlServer:Identity", "1, 1");

            builder.HasAlternateKey(re => new { re.RoleID, re.EntityProfileID });

            builder.HasOne(re => re.EntityProfile)
                .WithMany(r => r.RoleEntities)
                .HasForeignKey(re => re.EntityProfileID);

            builder.HasOne(re => re.Role)
                .WithMany(r => r.RoleEntities)
                .HasForeignKey(r => r.RoleID);
        }
    }
}