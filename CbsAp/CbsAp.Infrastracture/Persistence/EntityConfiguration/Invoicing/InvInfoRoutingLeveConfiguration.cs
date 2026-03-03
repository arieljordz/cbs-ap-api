using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.Invoicing
{
    public class InvInfoRoutingLeveConfiguration : IEntityTypeConfiguration<InvInfoRoutingLevel>
    {

        public void Configure(EntityTypeBuilder<InvInfoRoutingLevel> builder)
        {
            IgnoreBaseAuditFieldConfiguration.IgnoreConfigureAuditFields(builder);
            builder.ToTable(nameof(InvInfoRoutingLevel), "CBSAP");
            builder.HasKey( a => a.InvInfoRoutingLevelID);
            
            builder.Property(a => a.InvInfoRoutingLevelID).ValueGeneratedOnAdd();
            builder.Property(a => a.Level).IsRequired();

            builder.HasOne(x => x.InvRoutingFlow)
            .WithMany(x => x.InvInfoRoutingLevels)
            .HasForeignKey(x=> x.InvRoutingFlowID)
            .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Invoice)
            .WithMany(x => x.InvInfoRoutingLevels)
            .HasForeignKey(x => x.InvoiceID)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SupplierInfo)
           .WithMany(x => x.InvInfoRoutingLevels)
           .HasForeignKey(x => x.SupplierInfoID)
           .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Keyword)
            .WithMany(x => x.InvInfoRoutingLevels)
            .HasForeignKey(x => x.KeywordID)
            .OnDelete(DeleteBehavior.Restrict);



        }
    }
}
