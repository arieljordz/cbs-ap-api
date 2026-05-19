using System.Reflection.Emit;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Enums;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.Invoicing
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            CommonConfiguration.ConfigureAuditFields(builder);
            builder.ToTable(nameof(Invoice), "CBSAP");

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

            builder.Property(i => i.LockedBy)
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

            builder.OwnsOne(
                i => i.FreeFields,
                b => b.ConfigureFreeFields());
            builder.OwnsOne(
               i => i.SpareAmount,
               b => b.ConfigureSpareAmountsFields());

            // Relationships
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

            builder.HasOne(i => i.InvRoutingFlow)
                .WithMany()
                .HasForeignKey(i => i.InvRoutingFlowID)
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
           .WithOne(a => a.Invoice)
           .HasForeignKey(a => a.InvoiceID)
           .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(i => i.InvoiceComments)
             .WithOne(a => a.Invoice)
             .HasForeignKey(a => a.InvoiceID)
             .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(i => i.InvoiceAttachnments)
            .WithOne(a => a.Invoice)
            .HasForeignKey(a => a.InvoiceID)
            .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(i => i.InvoiceActivityLog)
           .WithOne(a => a.Invoice)
           .HasForeignKey(a => a.InvoiceID)
           .OnDelete(DeleteBehavior.Cascade);


            builder.HasMany(po => po.PurchaseOrderMatchTrackings)
                .WithOne(po => po.Invoice)
                .HasForeignKey(poline => poline.InvoiceID)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasMany(infolevels =>  infolevels.InvInfoRoutingLevels)
               .WithOne(i => i.Invoice)
               .HasForeignKey(infolevels => infolevels.InvoiceID)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.ApprovedUserInvoices)
                .WithMany()
                .HasForeignKey(i => i.ApprovedUser)
                .OnDelete(DeleteBehavior.Restrict);



            builder.HasOne(i => i.ApproverInvoices)
                .WithMany()
                .HasForeignKey(i => i.ApproverRole)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}