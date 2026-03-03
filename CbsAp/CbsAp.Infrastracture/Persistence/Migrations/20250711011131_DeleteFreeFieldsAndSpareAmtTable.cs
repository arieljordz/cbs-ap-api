using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DeleteFreeFieldsAndSpareAmtTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceFreeField",
                schema: "CBSAP");

            migrationBuilder.DropTable(
                name: "InvoiceSpareAmount",
                schema: "CBSAP");

            migrationBuilder.AddColumn<string>(
                name: "FreeField1",
                schema: "CBSAP",
                table: "Invoice",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FreeField2",
                schema: "CBSAP",
                table: "Invoice",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FreeField3",
                schema: "CBSAP",
                table: "Invoice",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SpareAmount1",
                schema: "CBSAP",
                table: "Invoice",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SpareAmount2",
                schema: "CBSAP",
                table: "Invoice",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SpareAmount3",
                schema: "CBSAP",
                table: "Invoice",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FreeField1",
                schema: "CBSAP",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "FreeField2",
                schema: "CBSAP",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "FreeField3",
                schema: "CBSAP",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "SpareAmount1",
                schema: "CBSAP",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "SpareAmount2",
                schema: "CBSAP",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "SpareAmount3",
                schema: "CBSAP",
                table: "Invoice");

            migrationBuilder.CreateTable(
                name: "InvoiceFreeField",
                schema: "CBSAP",
                columns: table => new
                {
                    InvoiceFreeFieldID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceID = table.Column<long>(type: "bigint", nullable: true),
                    FieldKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FieldValue = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceFreeField", x => x.InvoiceFreeFieldID);
                    table.ForeignKey(
                        name: "FK_InvoiceFreeField_Invoice_InvoiceID",
                        column: x => x.InvoiceID,
                        principalSchema: "CBSAP",
                        principalTable: "Invoice",
                        principalColumn: "InvoiceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceSpareAmount",
                schema: "CBSAP",
                columns: table => new
                {
                    InvoiceSpareAmountID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceID = table.Column<long>(type: "bigint", nullable: true),
                    FieldKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FieldValue = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceSpareAmount", x => x.InvoiceSpareAmountID);
                    table.ForeignKey(
                        name: "FK_InvoiceSpareAmount_Invoice_InvoiceID",
                        column: x => x.InvoiceID,
                        principalSchema: "CBSAP",
                        principalTable: "Invoice",
                        principalColumn: "InvoiceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceFreeField_InvoiceID",
                schema: "CBSAP",
                table: "InvoiceFreeField",
                column: "InvoiceID");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceSpareAmount_InvoiceID",
                schema: "CBSAP",
                table: "InvoiceSpareAmount",
                column: "InvoiceID");
        }
    }
}
