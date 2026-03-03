using CbsAp.Domain.Entities.PermissionManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.PermissionManagement
{
    public class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.ToTable(nameof(Menu), "CBSAP");
            builder.HasKey(m => m.MenuID);
            builder.Property(m => m.MenuID).ValueGeneratedOnAdd();




            builder.Property(m => m.Label).HasMaxLength(100).IsRequired();
            builder.Property(m => m.Icon).HasMaxLength(100);
            builder.Property(m => m.RouterLink)
                .HasMaxLength(100);

           
        }
    }
}