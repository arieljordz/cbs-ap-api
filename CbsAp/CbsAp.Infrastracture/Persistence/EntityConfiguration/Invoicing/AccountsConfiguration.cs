using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.Invoicing
{
    public class AccountsConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            CommonConfiguration.ConfigureAuditFields(builder);
            builder.ToTable(nameof(Account), "CBSAP");

            //fields
            builder.HasKey(a => a.AccountID);
            builder.Property(a => a.AccountID).ValueGeneratedOnAdd();

            builder.Property(a => a.AccountName)
             .HasMaxLength(50);

            builder.Property(a => a.TaxCodeName)
                .HasMaxLength(100);

            builder.Property(a => a.Dimension1)
               .HasMaxLength(15);

            builder.Property(a => a.Dimension2)
               .HasMaxLength(15);
            builder.Property(a => a.Dimension3)
               .HasMaxLength(15);
            builder.Property(a => a.Dimension4)
               .HasMaxLength(15);
            builder.Property(a => a.Dimension5)
               .HasMaxLength(15);

            builder.Property(a => a.Dimension6)
            .HasMaxLength(15);
            builder.Property(a => a.Dimension7)
            .HasMaxLength(15);

            builder.Property(a => a.Dimension8)
            .HasMaxLength(15);

            builder.Property(a => a.FreeField1)
           .HasMaxLength(15);
            builder.Property(a => a.FreeField2)
           .HasMaxLength(15);
            builder.Property(a => a.FreeField3)
           .HasMaxLength(15);

            //relationship
            builder.HasOne(a => a.EntityProfile)
                .WithMany()
                .HasForeignKey(a => a.EntityProfileID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.TaxCode)
               .WithMany()
               .HasForeignKey(a => a.TaxCodeID)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.InvoiceAllocationLines)
              .WithOne(a => a.Account)
              .HasForeignKey(a => a.AccountID)
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.PurchaseOrderLines)
            .WithOne(a => a.Account)
            .HasForeignKey(a => a.AccountID)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}