using CbsAp.Domain.Entities.RoleManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.UserManagement
{
    public class UserRolesConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable(nameof(UserRole), "CBSAP");
            builder.Ignore(ur => ur.CreatedBy);
            builder.Ignore(ur => ur.CreatedDate);
            builder.Ignore(ur => ur.LastUpdatedBy);
            builder.Ignore(ur => ur.LastUpdatedDate);

            builder.HasKey(ur => ur.UserRoleID).HasAnnotation("SqlServer:Identity", "1, 1");

            builder.HasAlternateKey(ur => new { ur.UserAccountID, ur.RoleID });

            builder.HasOne(ur => ur.UserAccount)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserAccountID);

            builder
                 .HasOne(ur => ur.Role)
                 .WithMany(r => r.UserRoles)
                 .HasForeignKey(ur => ur.RoleID);
        }
    }
}