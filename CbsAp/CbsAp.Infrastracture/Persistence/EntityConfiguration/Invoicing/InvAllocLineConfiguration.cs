using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.Invoicing
{
    public class InvAllocLineConfiguration : IEntityTypeConfiguration<InvAllocLine>
    {
        public void Configure(EntityTypeBuilder<InvAllocLine> builder)
        {
            CommonConfiguration.ConfigureAuditFields(builder);
            builder.ToTable(nameof(InvAllocLine), "CBSAP");

            builder.HasKey(a => a.InvAllocLineID);

            builder.Property(a => a.LineNo);

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

            // Relationships
            builder.HasOne(a => a.Invoice)
                .WithMany(i => i.InvoiceAllocationLines)
                .HasForeignKey(a => a.InvoiceID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Account)
             .WithMany(i => i.InvoiceAllocationLines)
             .HasForeignKey(a => a.AccountID)
             .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.FreeFields)
                .WithOne()
                .HasForeignKey(f => f.InvAllocLineID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.Dimensions)
                .WithOne()
                .HasForeignKey(d => d.InvAllocLineID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(po => po.PurchaseOrderMatchTrackings)
             .WithOne(po => po.InvAllocLine)
             .HasForeignKey(poline => poline.InvAllocLineID)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}