using CbsAp.Domain.Entities.PermissionManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.PermissionManagement
{
    public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> builder)
        {
            builder.ToTable(nameof(MenuItem), "CBSAP");
            builder.HasKey(mi => mi.MenuItemID);
             builder.Property(mi => mi.MenuItemID).ValueGeneratedOnAdd();

            builder.Property(mi => mi.Label).HasMaxLength(100).IsRequired();
            builder.Property(mi => mi.Icon).HasMaxLength(100).IsRequired();
            builder.Property(mi => mi.RouterLink).HasMaxLength(100);

            builder.HasOne(mi => mi.Menu)
                .WithMany(m => m.MenuItems)
                .HasForeignKey(mi => mi.MenuID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}