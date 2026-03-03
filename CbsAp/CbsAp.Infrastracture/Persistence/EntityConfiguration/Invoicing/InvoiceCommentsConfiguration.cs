using CbsAp.Domain.Entities.Invoicing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.Invoicing
{
    public class InvoiceCommentsConfiguration : IEntityTypeConfiguration<InvoiceComment>
    {
        public void Configure(EntityTypeBuilder<InvoiceComment> builder)
        {
            builder.Property(e => e.CreatedDate);

            builder.Property(e => e.CreatedBy)
                .HasMaxLength(255);

            builder.Ignore(e => e.LastUpdatedDate);
            builder.Ignore(e => e.LastUpdatedBy);

            builder.ToTable(nameof(InvoiceComment), "CBSAP");

            builder.HasKey(i => i.InvoiceCommentID);
            builder.Property(i => i.Comment)
               .HasMaxLength(255);

            builder.HasOne(f => f.Invoice)
                .WithMany(i => i.InvoiceComments)
                .HasForeignKey(f => f.InvoiceID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}