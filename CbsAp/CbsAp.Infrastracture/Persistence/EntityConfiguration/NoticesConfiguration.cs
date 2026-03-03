using CbsAp.Domain.Entities.Dashboard;
using CbsAp.Domain.Enums;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration
{
    public class NoticesConfiguration : IEntityTypeConfiguration<Notice>
    {
        public void Configure(EntityTypeBuilder<Notice> builder)
        {
            CommonConfiguration.ConfigureAuditFields(builder);

            builder.ToTable(nameof(Notice), "CBSAP");
            builder.HasKey(x => x.NoticeID).HasAnnotation("SqlServer:Identity", "1, 1");

            builder.Property(p => p.Heading)
                .HasMaxLength(200)
                .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(p => p.Message)
                 .HasMaxLength(300)
                 .HasColumnType(DbDataTypes.NVARCHAR.ToString());
        }
    }
}