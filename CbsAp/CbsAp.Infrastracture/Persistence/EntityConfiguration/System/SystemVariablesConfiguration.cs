using CbsAp.Domain.Entities.System;
using CbsAp.Domain.Enums;
using CbsAp.Infrastracture.Persistence.EntityConfiguration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.System
{
    public class SystemVariablesConfiguration : IEntityTypeConfiguration<SystemVariable>
    {
        public void Configure(EntityTypeBuilder<SystemVariable> builder)
        {
            CommonConfiguration.ConfigureAuditFields(builder);
            builder.ToTable(nameof(SystemVariable), "CBSAP");
            builder.HasKey(sv => sv.SystemVariableID).HasAnnotation("SqlServer:Identity", "1, 1");
            builder.Property(sv => sv.Name)
                 .HasMaxLength(500)
                 .HasColumnType(DbDataTypes.NVARCHAR.ToString());
            builder.Property(sv => sv.Value)
                .HasMaxLength(500)
                .HasColumnType(DbDataTypes.NVARCHAR.ToString());
            builder.Property(sv => sv.Description)
                .HasMaxLength(500)
                .HasColumnType(DbDataTypes.NVARCHAR.ToString());
        }
    }
}
