using CbsAp.Domain.Entities.UserManagement;
using CbsAp.Domain.Enums;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.UserManagement
{
    public class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
    {
        public void Configure(EntityTypeBuilder<UserAccount> builder)
        {
            CommonConfiguration.ConfigureAuditFields(builder);

            builder.ToTable(nameof(UserAccount), "CBSAP");
            builder.HasKey(x => x.UserAccountID).HasAnnotation("SqlServer:Identity", "1, 1");
            builder.HasAlternateKey(key => key.UserID);

            builder.Property(p => p.FirstName)
                .HasMaxLength(200)
                .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(p => p.LastName)
                 .HasMaxLength(200)
                .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(p => p.EmailAddress)
                .IsRequired()
                 .HasMaxLength(200)
                .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            //builder.Property(p => p.BirthDate)
            //    .IsRequired()
            //    .HasColumnType(DbDataTypes.DATE.ToString());

            // Configure one-to-one relationship with User Log info

            builder.HasOne(a => a.UserLogInfo)
                 .WithOne(a => a.UserAccount)
                 .HasForeignKey<UserLogInfo>(a => a.UserAccountID);
        }
    }
}