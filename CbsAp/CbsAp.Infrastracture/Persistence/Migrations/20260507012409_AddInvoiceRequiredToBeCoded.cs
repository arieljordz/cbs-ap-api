using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddInvoiceRequiredToBeCoded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "InvoiceRequiredToBeCoded",
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
                name: "InvoiceRequiredToBeCoded",
                schema: "CBSAP",
                table: "EntityProfile");
        }
    }
}
