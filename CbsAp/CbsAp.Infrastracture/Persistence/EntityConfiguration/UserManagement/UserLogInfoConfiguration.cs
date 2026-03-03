using CbsAp.Domain.Entities.UserManagement;
using CbsAp.Domain.Enums;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.UserManagement
{
    public class UserLogInfoConfiguration : IEntityTypeConfiguration<UserLogInfo>
    {
        public void Configure(EntityTypeBuilder<UserLogInfo> builder)
        {
            CommonConfiguration.ConfigureAuditFields(builder);
            builder.ToTable(nameof(UserLogInfo), "CBSAP");

            builder.HasKey(x => x.UserLogInfoID);
            builder.HasAlternateKey(x => x.UserID);

            builder.Property(x => x.PasswordHash)
                .HasMaxLength(500)
                .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(x => x.PasswordSalt)
               .HasMaxLength(500)
               .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(x => x.ConfirmationToken)
                 .HasMaxLength(1000)
                  .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(x => x.PasswordrecoveryToken)
                 .HasMaxLength(1000)
                  .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            
        }
    }
}