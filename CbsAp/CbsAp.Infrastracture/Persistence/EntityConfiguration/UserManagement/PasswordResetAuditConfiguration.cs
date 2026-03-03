using CbsAp.Domain.Entities.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.UserManagement
{
    public class PasswordResetAuditConfiguration : IEntityTypeConfiguration<PasswordResetAudit>
    {
        public void Configure(EntityTypeBuilder<PasswordResetAudit> builder)
        {
            builder.HasKey(p => p.PasswordResetAuditID);

            // Table name (optional if conventions are followed)
            builder.ToTable("PasswordResetAudits", "CBSAP");

            // Relationships
            builder.HasOne(p => p.UserAccount)
                   .WithMany(u => u.PasswordResetAudits)
                   .HasForeignKey(p => p.UserAccountID)
                   .OnDelete(DeleteBehavior.Cascade);

            // Properties
            builder.Property(p => p.CreatedDate)
                   .IsRequired();

            builder.Property(p => p.IsSuccessfulLoginAfterReset)
                   .IsRequired();

            // Indexes (optional, improves performance on queries)
            builder.HasIndex(p => new { p.UserAccountID, p.CreatedDate });
        }
    }
}
