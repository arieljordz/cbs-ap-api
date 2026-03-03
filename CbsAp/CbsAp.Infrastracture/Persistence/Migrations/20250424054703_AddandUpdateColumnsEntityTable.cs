using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddandUpdateColumnsEntityTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefaultInvoiceDueInDays",
                schema: "CBSAP",
                table: "EntityProfile",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "InvAllowPresetAmount",
                schema: "CBSAP",
                table: "EntityProfile",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "InvAllowPresetDimension",
                schema: "CBSAP",
                table: "EntityProfile",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EntityMatchingConfig",
                schema: "CBSAP",
                columns: table => new
                {
                    MatchingConfigID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityProfileID = table.Column<long>(type: "bigint", nullable: false),
                    ConfigType = table.Column<int>(type: "int", maxLength: 10, nullable: false),
                    MatchingLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InvoiceMatchBasis = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ToleranceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ToleranceAmtValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityMatchingConfig", x => x.MatchingConfigID);
                    table.ForeignKey(
                        name: "FK_EntityMatchingConfig_EntityProfile_EntityProfileID",
                        column: x => x.EntityProfileID,
                        principalSchema: "CBSAP",
                        principalTable: "EntityProfile",
                        principalColumn: "EntityProfileID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntityMatchingConfig_EntityProfileID",
                schema: "CBSAP",
                table: "EntityMatchingConfig",
                column: "EntityProfileID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityMatchingConfig",
                schema: "CBSAP");

            migrationBuilder.DropColumn(
                name: "DefaultInvoiceDueInDays",
                schema: "CBSAP",
                table: "EntityProfile");

            migrationBuilder.DropColumn(
                name: "InvAllowPresetAmount",
                schema: "CBSAP",
                table: "EntityProfile");

            migrationBuilder.DropColumn(
                name: "InvAllowPresetDimension",
                schema: "CBSAP",
                table: "EntityProfile");
        }
    }
}