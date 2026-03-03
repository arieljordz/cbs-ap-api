using CbsAp.Domain.Entities.PermissionManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Infrastracture.Persistence.EntityConfiguration.PermissionManagement
{
    public class ControlElementConfiguration : IEntityTypeConfiguration<ControlElement>
    {
        public void Configure(EntityTypeBuilder<ControlElement> builder)
        {
            builder.ToTable(nameof(ControlElement), "CBSAP");
            builder.HasKey(ctrl => ctrl.ControlID)
                .HasAnnotation("SqlServer:Identity", "1, 1");

            builder.Property(ctrl => ctrl.ActionName).HasMaxLength(100).IsRequired(false);
            builder.Property(ctrl => ctrl.ActionType).HasMaxLength(100).IsRequired(false);


            builder.HasOne(ctrl => ctrl.Operation)
                .WithOne(m => m.ControlElement)
                .HasForeignKey<Operation>(ctrl=> ctrl.OperationID)
                 .OnDelete(DeleteBehavior.Restrict) ;
        }
    }
}
