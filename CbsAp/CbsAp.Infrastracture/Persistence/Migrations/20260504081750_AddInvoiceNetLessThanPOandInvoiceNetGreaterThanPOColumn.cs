using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddInvoiceNetLessThanPOandInvoiceNetGreaterThanPOColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "InvoiceNetGreaterThanPO",
                schema: "CBSAP",
                table: "EntityProfile",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "InvoiceNetLessThanPO",
                schema: "CBSAP",
                table: "EntityProfile",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceNetGreaterThanPO",
                schema: "CBSAP",
                table: "EntityProfile");

            migrationBuilder.DropColumn(
                name: "InvoiceNetLessThanPO",
                schema: "CBSAP",
                table: "EntityProfile");
        }
    }
}
