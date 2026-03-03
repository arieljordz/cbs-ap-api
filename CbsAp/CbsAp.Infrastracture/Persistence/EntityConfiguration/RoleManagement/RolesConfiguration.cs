using CbsAp.Domain.Entities.RoleManagement;
using CbsAp.Domain.Enums;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.RoleManagement
{
    public class RolesConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            CommonConfiguration.ConfigureAuditFields(builder);

            builder.ToTable(nameof(Role), "CBSAP");
            builder.HasKey(r => r.RoleID)
                .HasAnnotation("SqlServer:Identity", "1, 1");

            builder.Property(r => r.RoleName)
                .HasMaxLength(200)
                .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(r => r.AuthorisationLimit)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(r => r.RelatedRoleManager1)
                .WithMany()
                .HasForeignKey(r => r.RoleManager1)
                .IsRequired(false);


            builder.HasOne(r => r.RelatedRoleManager2)
             .WithMany()
             .HasForeignKey(r => r.RoleManager2)
             .IsRequired(false);



            builder.Property(r => r.CanBeAddedToInvoice);

            builder.HasOne(r => r.RoleReminderNotification)
                .WithOne(rn => rn.Role)
                .HasForeignKey<RoleReminderNotification>(rn => rn.RoleID);
        }
    }
}