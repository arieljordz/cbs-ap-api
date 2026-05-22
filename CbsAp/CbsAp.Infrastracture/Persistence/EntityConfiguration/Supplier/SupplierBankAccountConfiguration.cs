using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CbsAp.Domain.Entities.Supplier;
using CbsAp.Domain.Enums;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.Supplier
{
    public class SupplierBankAccountConfiguration : IEntityTypeConfiguration<SupplierBankAccount>
    {
        public void Configure(EntityTypeBuilder<SupplierBankAccount> builder)
        {
            CommonConfiguration.ConfigureAuditFields(builder);
            builder.ToTable(nameof(SupplierBankAccount), "CBSAP");

            builder.HasKey(e => e.SupplierBankAccountID);
            builder.Property(a => a.SupplierBankAccountID)
                   . HasAnnotation("SqlServer:Identity", "1, 1");

            builder.Property(e => e.BankAccountNumber)
                .IsRequired()
                .HasMaxLength(40)
                .HasColumnType(DbDataTypes.NVARCHAR.ToString());


            builder.Property(e => e.BankName)
                .HasMaxLength(40)
                .HasColumnType(DbDataTypes.NVARCHAR.ToString());
        }
    }
}
