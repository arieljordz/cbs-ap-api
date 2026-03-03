using CbsAp.Domain.Entities.UserManagement;
using CbsAp.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.UserManagement
{
    public class PasswordHistoryConfiguration : IEntityTypeConfiguration<PasswordHistory>
    {
        public void Configure(EntityTypeBuilder<PasswordHistory> builder)
        {
            builder.ToTable(nameof(PasswordHistory), "CBSAP");
            builder.Ignore(ur => ur.CreatedBy);
            builder.Ignore(ur => ur.CreatedDate);
            builder.Ignore(ur => ur.LastUpdatedBy);
            builder.Ignore(ur => ur.LastUpdatedDate);

            builder.HasKey(ph => ph.PasswordHistoryID)
                .HasAnnotation("SqlServer:Identity", "1, 1");


            builder.Property(x => x.PasswordHash)
               .HasMaxLength(500)
               .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(x => x.PasswordSalt)
               .HasMaxLength(500)
               .HasColumnType(DbDataTypes.NVARCHAR.ToString());


            builder.Property(ph => ph.CreatedAt)
                .IsRequired();

            builder.HasOne(ph => ph.UserAccount)
               .WithMany(u => u.PasswordHistories)
               .HasForeignKey(ph => ph.UserAccountID)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}