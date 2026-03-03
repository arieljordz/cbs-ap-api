using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchaseOrderMatchTrackingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SupplierID",
                schema: "CBSAP",
                table: "PurchaseOrder",
                newName: "SupplierTaxID");

            migrationBuilder.AlterColumn<decimal>(
                name: "TaxAmount",
                schema: "CBSAP",
                table: "PurchaseOrderLine",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                schema: "CBSAP",
                table: "PurchaseOrderLine",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "NetAmount",
                schema: "CBSAP",
                table: "PurchaseOrderLine",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "InvoicedPrice",
                schema: "CBSAP",
                table: "PurchaseOrderLine",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                schema: "CBSAP",
                table: "PurchaseOrderLine",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PurchaseOrderMatchTracking",
                schema: "CBSAP",
                columns: table => new
                {
                    PurchaseOrderMatchTrackingID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseOrderLineID = table.Column<long>(type: "bigint", nullable: false),
                    PurchaseOrderID = table.Column<long>(type: "bigint", nullable: false),
                    InvoiceID = table.Column<long>(type: "bigint", nullable: true),
                    InvAllocLineID = table.Column<long>(type: "bigint", nullable: true),
                    Account = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Qty = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MatchingDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsSystemMatching = table.Column<bool>(type: "bit", nullable: false),
                    MatchingStatus = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastUpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderMatchTracking", x => x.PurchaseOrderMatchTrackingID);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderMatchTracking_InvAllocLine_InvAllocLineID",
                        column: x => x.InvAllocLineID,
                        principalSchema: "CBSAP",
                        principalTable: "InvAllocLine",
                        principalColumn: "InvAllocLineID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderMatchTracking_Invoice_InvoiceID",
                        column: x => x.InvoiceID,
                        principalSchema: "CBSAP",
                        principalTable: "Invoice",
                        principalColumn: "InvoiceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderMatchTracking_PurchaseOrderLine_PurchaseOrderLineID",
                        column: x => x.PurchaseOrderLineID,
                        principalSchema: "CBSAP",
                        principalTable: "PurchaseOrderLine",
                        principalColumn: "PurchaseOrderLineID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderMatchTracking_PurchaseOrder_PurchaseOrderID",
                        column: x => x.PurchaseOrderID,
                        principalSchema: "CBSAP",
                        principalTable: "PurchaseOrder",
                        principalColumn: "PurchaseOrderID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderMatchTracking_InvAllocLineID",
                schema: "CBSAP",
                table: "PurchaseOrderMatchTracking",
                column: "InvAllocLineID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderMatchTracking_InvoiceID",
                schema: "CBSAP",
                table: "PurchaseOrderMatchTracking",
                column: "InvoiceID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderMatchTracking_PurchaseOrderID",
                schema: "CBSAP",
                table: "PurchaseOrderMatchTracking",
                column: "PurchaseOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderMatchTracking_PurchaseOrderLineID",
                schema: "CBSAP",
                table: "PurchaseOrderMatchTracking",
                column: "PurchaseOrderLineID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseOrderMatchTracking",
                schema: "CBSAP");

            migrationBuilder.RenameColumn(
                name: "SupplierTaxID",
                schema: "CBSAP",
                table: "PurchaseOrder",
                newName: "SupplierID");

            migrationBuilder.AlterColumn<decimal>(
                name: "TaxAmount",
                schema: "CBSAP",
                table: "PurchaseOrderLine",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                schema: "CBSAP",
                table: "PurchaseOrderLine",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "NetAmount",
                schema: "CBSAP",
                table: "PurchaseOrderLine",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "InvoicedPrice",
                schema: "CBSAP",
                table: "PurchaseOrderLine",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                schema: "CBSAP",
                table: "PurchaseOrderLine",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }
    }
}
