using CbsAp.Domain.Entities.InvoicingArchive;
using CbsAp.Domain.Enums;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.InvoicingArchive
{
    public class InvoiceArchiveConfiguration : IEntityTypeConfiguration<InvoiceArchive>
    {
        public void Configure(EntityTypeBuilder<InvoiceArchive> builder)
        {
            CommonConfiguration.ConfigureAuditFields(builder);
            builder.ToTable(nameof(InvoiceArchive), "CBSAP");

            builder.HasKey(i => i.InvoiceID);

            builder.Property(i => i.InvoiceNo)
                .HasMaxLength(50);

            builder.Property(i => i.MapID)
                .HasMaxLength(100);

            builder.Property(i => i.ImageID)
                .HasMaxLength(16)
                .HasColumnType("char(16)");

            builder.Property(i => i.SuppBankAccount)
                .HasMaxLength(100);

            builder.Property(i => i.PoNo)
                .HasMaxLength(50);

            builder.Property(i => i.GrNo)
                .HasMaxLength(50);

            builder.Property(i => i.Currency)
                .HasMaxLength(10);

            builder.Property(i => i.PaymentTerm)
                .HasMaxLength(50);

            builder.Property(i => i.Note)
                .HasMaxLength(1000);

            builder.Property(i => i.ApproverRole)
                .HasMaxLength(100);

            builder.Property(i => i.ApprovedUser)
                .HasMaxLength(100);

            builder.Property(i => i.NetAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.TaxAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.TotalAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.QueueType)
                .HasConversion<int>();

            builder.Property(i => i.StatusType)
                .HasConversion<int>();

            builder.OwnsOne(i => i.FreeFields, b => b.ConfigureFreeFields());
            builder.OwnsOne(i => i.SpareAmount, b => b.ConfigureSpareAmountsFields());

            builder.HasOne(i => i.EntityProfile)
                .WithMany()
                .HasForeignKey(i => i.EntityProfileID)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(i => i.SupplierInfo)
                .WithMany()
                .HasForeignKey(i => i.SupplierInfoID)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(i => i.Keyword)
                .WithMany()
                .HasForeignKey(i => i.KeywordID)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(i => i.TaxCode)
                .WithMany()
                .HasForeignKey(i => i.TaxCodeID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(i => i.InvoiceAllocationLines)
                .WithOne(a => a.Invoice)
                .HasForeignKey(a => a.InvoiceID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(i => i.InvoiceComments)
                .WithOne(c => c.Invoice)
                .HasForeignKey(c => c.InvoiceID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(i => i.InvoiceAttachnments)
                .WithOne(a => a.Invoice)
                .HasForeignKey(a => a.InvoiceID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(i => i.InvoiceActivityLog)
                .WithOne(l => l.Invoice)
                .HasForeignKey(l => l.InvoiceID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(i => i.PurchaseOrderMatchTrackings)
                .WithOne(po => po.Invoice)
                .HasForeignKey(po => po.InvoiceID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
