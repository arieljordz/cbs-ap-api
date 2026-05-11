using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddGoodsReceiptTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "GoodsReceiptLineID",
                schema: "CBSAP",
                table: "PurchaseOrderMatchTracking",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryStatus",
                schema: "CBSAP",
                table: "PurchaseOrderLine",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InvoiceStatus",
                schema: "CBSAP",
                table: "PurchaseOrderLine",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PurchaseOrderMatchType",
                schema: "CBSAP",
                table: "PurchaseOrder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GoodsReceiptLine",
                schema: "CBSAP",
                columns: table => new
                {
                    GoodsReceiptLineID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoodsReceiptID = table.Column<long>(type: "bigint", nullable: false),
                    LineNo = table.Column<int>(type: "int", nullable: false),
                    Qty = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    SupplierNo = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    PurchaseOrderNo = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    ReceiptNo = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    FreeField1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FreeField2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FreeField3 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    InvoiceStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastUpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsReceiptLine", x => x.GoodsReceiptLineID);
                    table.ForeignKey(
                        name: "FK_GoodsReceiptLine_GoodReceipts_GoodsReceiptID",
                        column: x => x.GoodsReceiptID,
                        principalSchema: "CBSAP",
                        principalTable: "GoodReceipts",
                        principalColumn: "GoodsReceiptID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptLine_GoodsReceiptID",
                schema: "CBSAP",
                table: "GoodsReceiptLine",
                column: "GoodsReceiptID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoodsReceiptLine",
                schema: "CBSAP");

            migrationBuilder.DropColumn(
                name: "GoodsReceiptLineID",
                schema: "CBSAP",
                table: "PurchaseOrderMatchTracking");

            migrationBuilder.DropColumn(
                name: "DeliveryStatus",
                schema: "CBSAP",
                table: "PurchaseOrderLine");

            migrationBuilder.DropColumn(
                name: "InvoiceStatus",
                schema: "CBSAP",
                table: "PurchaseOrderLine");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderMatchType",
                schema: "CBSAP",
                table: "PurchaseOrder");
        }
    }
}
