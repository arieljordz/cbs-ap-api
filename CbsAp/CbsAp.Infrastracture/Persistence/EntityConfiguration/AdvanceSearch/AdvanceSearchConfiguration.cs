using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Entities.AdvanceSearch;
using CbsAp.Domain.Enums;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.AdvanceSearch
{
    public class AdvanceSearchConfiguration : IEntityTypeConfiguration<CbsAp.Domain.Entities.AdvanceSearch.AdvanceSearch>
    {
        public void Configure(EntityTypeBuilder<CbsAp.Domain.Entities.AdvanceSearch.AdvanceSearch> builder)
        {
            CommonConfiguration.ConfigureAuditFields(builder);
            builder.ToTable(nameof(AdvanceSearch), "CBSAP");


            builder.HasKey(e => e.AdvanceSearchId);
            builder.Property(a => a.AdvanceSearchId)
                .ValueGeneratedOnAdd()
                .HasAnnotation("SqlServer:Identity", "1, 1");

            builder.Property(e => e.UserId)
            .HasMaxLength(150)
            .HasColumnType(DbDataTypes.NVARCHAR.ToString());

            builder.Property(e => e.JsonFilter)
         .HasColumnType("nvarchar(max)");

            builder.Property(e => e.FormName)
            .HasMaxLength(150)
            .HasColumnType(DbDataTypes.NVARCHAR.ToString());


        }
    }
}
