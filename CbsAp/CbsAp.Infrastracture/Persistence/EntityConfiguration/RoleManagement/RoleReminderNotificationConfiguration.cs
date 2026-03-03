using CbsAp.Domain.Entities.RoleManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.RoleManagement
{
    public class RoleReminderNotificationConfiguration : IEntityTypeConfiguration<RoleReminderNotification>
    {
        public void Configure(EntityTypeBuilder<RoleReminderNotification> builder)
        {
            builder.ToTable(nameof(RoleReminderNotification), "CBSAP");
            builder.Ignore(ur => ur.CreatedBy);
            builder.Ignore(ur => ur.CreatedDate);
            builder.Ignore(ur => ur.LastUpdatedBy);
            builder.Ignore(ur => ur.LastUpdatedDate);

            builder.HasKey(rn => rn.RoleReminderNotificationID);

            builder.HasAlternateKey(rn => rn.RoleID);
        }
    }
}