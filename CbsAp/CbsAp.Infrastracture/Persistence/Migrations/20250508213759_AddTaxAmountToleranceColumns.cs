using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTaxAmountToleranceColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TaxDollarAmt",
                schema: "CBSAP",
                table: "EntityProfile",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxPercentageAmt",
                schema: "CBSAP",
                table: "EntityProfile",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaxDollarAmt",
                schema: "CBSAP",
                table: "EntityProfile");

            migrationBuilder.DropColumn(
                name: "TaxPercentageAmt",
                schema: "CBSAP",
                table: "EntityProfile");
        }
    }
}
