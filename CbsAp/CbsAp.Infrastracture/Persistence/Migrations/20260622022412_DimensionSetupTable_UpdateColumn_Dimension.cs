using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DimensionSetupTable_UpdateColumn_Dimension : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DimensionNameId",
                schema: "CBSAP",
                table: "DimensionSetup");

            migrationBuilder.AddColumn<string>(
                name: "DimensionName",
                schema: "CBSAP",
                table: "DimensionSetup",
                type: "NVARCHAR(150)",
                maxLength: 150,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DimensionName",
                schema: "CBSAP",
                table: "DimensionSetup");

            migrationBuilder.AddColumn<long>(
                name: "DimensionNameId",
                schema: "CBSAP",
                table: "DimensionSetup",
                type: "bigint",
                nullable: true);
        }
    }
}
