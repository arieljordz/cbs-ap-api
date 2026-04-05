using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CbsAp.Domain.Entities.ActivityLog;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.Invoicing
{
    public class ActivityLogConfiguration : IEntityTypeConfiguration<ActivityLog>
    {
        public void Configure(EntityTypeBuilder<ActivityLog> builder)
        {
            CommonConfiguration.ConfigureAuditFields(builder);
            builder.ToTable(nameof(ActivityLog), "CBSAP");

            //fields
            builder.HasKey(a => a.ActivityID);
            builder.Property(a => a.ActivityID).ValueGeneratedOnAdd();

            builder.Property(a => a.metaDataNew)
                 .HasColumnType("nvarchar(max)"); // unlimited length

            builder.Property(a => a.metaDataOld)
                 .HasColumnType("nvarchar(max)"); // unlimited length;
        }
    }
}
