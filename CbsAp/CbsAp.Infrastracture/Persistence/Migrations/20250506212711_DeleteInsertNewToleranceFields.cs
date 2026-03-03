using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DeleteInsertNewToleranceFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToleranceType",
                schema: "CBSAP",
                table: "EntityMatchingConfig");

            migrationBuilder.DropColumn(
             name: "ToleranceAmtValue",
             schema: "CBSAP",
             table: "EntityMatchingConfig");


            migrationBuilder.AddColumn<decimal>(
                name: "DollarAmt",
                schema: "CBSAP",
                table: "EntityMatchingConfig",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
              name: "PercentageAmt",
              schema: "CBSAP",
              table: "EntityMatchingConfig",
              type: "decimal(18,2)",
              nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DollarAmt",
                schema: "CBSAP",
                table: "EntityMatchingConfig");
            
            migrationBuilder.DropColumn(
                name: "PercentageAmt",
                schema: "CBSAP",
                table: "EntityMatchingConfig");


            migrationBuilder.DropColumn(
             name: "ToleranceAmtValue",
             schema: "CBSAP",
             table: "EntityMatchingConfig");

            migrationBuilder.DropColumn(
             name: "ToleranceType",
             schema: "CBSAP",
             table: "EntityMatchingConfig");


        }
    }
}
