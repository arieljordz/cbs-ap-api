using CbsAp.Domain.Entities.InvoicingArchive;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.InvoicingArchive
{
    public class InvAllocLineArchiveConfiguration : IEntityTypeConfiguration<InvAllocLineArchive>
    {
        public void Configure(EntityTypeBuilder<InvAllocLineArchive> builder)
        {
            CommonConfiguration.ConfigureAuditFields(builder);
            builder.ToTable(nameof(InvAllocLineArchive), "CBSAP");

            builder.HasKey(a => a.InvAllocLineID);

            builder.Property(a => a.PoNo)
                .HasMaxLength(50);

            builder.Property(a => a.PoLineNo)
                .HasMaxLength(50);

            builder.Property(a => a.LineDescription)
                .HasMaxLength(500);

            builder.Property(a => a.Currency)
                .HasMaxLength(10);

            builder.Property(a => a.LineApproved)
                .HasMaxLength(20);

            builder.Property(a => a.Note)
                .HasMaxLength(1000);

            builder.Property(a => a.Qty)
                .HasColumnType("decimal(18,4)");

            builder.Property(a => a.LineNetAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(a => a.LineTaxAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(a => a.LineAmount)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(a => a.Invoice)
                .WithMany(i => i.InvoiceAllocationLines)
                .HasForeignKey(a => a.InvoiceID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Account)
                .WithMany()
                .HasForeignKey(a => a.AccountID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.FreeFields)
                .WithOne(f => f.AllocationLine)
                .HasForeignKey(f => f.InvAllocLineID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.Dimensions)
                .WithOne(d => d.AllocationLine)
                .HasForeignKey(d => d.InvAllocLineID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.PurchaseOrderMatchTrackings)
                .WithOne(po => po.InvAllocLine)
                .HasForeignKey(po => po.InvAllocLineID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
