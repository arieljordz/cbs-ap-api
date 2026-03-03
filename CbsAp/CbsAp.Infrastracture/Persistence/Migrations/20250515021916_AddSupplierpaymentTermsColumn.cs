using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSupplierpaymentTermsColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          

          

            migrationBuilder.AddColumn<string>(
                name: "PaymentTerms",
                schema: "CBSAP",
                table: "SupplierInfo",
                type: "NVARCHAR(4)",
                maxLength: 4,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentTerms",
                schema: "CBSAP",
                table: "SupplierInfo");

        
        }
    }
}
