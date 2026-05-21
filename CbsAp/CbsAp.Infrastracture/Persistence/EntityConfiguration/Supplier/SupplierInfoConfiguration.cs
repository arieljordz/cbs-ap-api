using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Entities.Supplier;
using CbsAp.Domain.Enums;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.Supplier
{
    public class SupplierInfoConfiguration : IEntityTypeConfiguration<SupplierInfo>
    {
        public void Configure(EntityTypeBuilder<SupplierInfo> builder)
        {
            CommonConfiguration.ConfigureAuditFields(builder);
            builder.ToTable(nameof(SupplierInfo), "CBSAP");


            builder.HasKey(e => e.SupplierInfoID);
            builder.Property(a => a.SupplierInfoID)
                .ValueGeneratedOnAdd()
                .HasAnnotation("SqlServer:Identity", "1, 1"); 

            builder.Property(e => e.SupplierID)
            .IsRequired()  
            .HasMaxLength(40)
            .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(e => e.SupplierTaxID)
                .HasMaxLength(40)
              .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(e => e.SupplierName)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType(DbDataTypes.NVARCHAR.ToString()); 

            builder.Property(e => e.IsActive)
                .IsRequired();

            builder.Property(e => e.Telephone)
                .HasMaxLength(30)
                .HasColumnType(DbDataTypes.NVARCHAR.ToString()); 

            builder.Property(e => e.EmailAddress)
                .HasMaxLength(90)
                .HasColumnType(DbDataTypes.NVARCHAR.ToString()); 

            builder.Property(e => e.Contact)
                .HasMaxLength(90)
                .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(e => e.AddressLine1)
                .HasMaxLength(90)
                .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(e => e.AddressLine2)
                   .HasMaxLength(90)
                   .HasColumnType(DbDataTypes.NVARCHAR.ToString()); 

            builder.Property(e => e.AddressLine3)
                   .HasMaxLength(90)
                   .HasColumnType(DbDataTypes.NVARCHAR.ToString()); 

            builder.Property(e => e.AddressLine4)
                   .HasMaxLength(90)
                   .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(e => e.AddressLine5)
                   .HasMaxLength(90)
                   .HasColumnType(DbDataTypes.NVARCHAR.ToString());
            builder.Property(e => e.AddressLine6)
                   .HasMaxLength(90)
                   .HasColumnType(DbDataTypes.NVARCHAR.ToString()); 

            builder.Property(e => e.Currency)
                .HasMaxLength(3)
                .HasColumnType(DbDataTypes.NVARCHAR.ToString());
            builder.Property(e => e.PaymentTerms)
             .HasMaxLength(4)
             .HasColumnType(DbDataTypes.NVARCHAR.ToString());


            builder.Property(e => e.FreeField1)
                   .HasMaxLength(90)
                   .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(e => e.FreeField2)
                   .HasMaxLength(90)
                   .HasColumnType(DbDataTypes.NVARCHAR.ToString());
            builder.Property(e => e.FreeField3)
                   .HasMaxLength(90)
                   .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(e => e.Notes)
                .HasMaxLength(400)
                .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            // Relationships
            builder.HasOne(e => e.EntityProfile)
                .WithMany()
                .HasForeignKey(e => e.EntityProfileID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Account)
                .WithMany()
                .HasForeignKey(e => e.AccountID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.TaxCode)
                .WithMany()
                .HasForeignKey(e => e.TaxCodeID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.InvRoutingFlow)
                .WithMany()
                .HasForeignKey(e => e.InvRoutingFlowID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(infolevels => infolevels.InvInfoRoutingLevels)
            .WithOne(i => i.SupplierInfo)
            .HasForeignKey(infolevels => infolevels.SupplierInfoID)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
