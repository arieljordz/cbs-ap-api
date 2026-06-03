using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addnewbankaccounttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PoNo",
                schema: "CBSAP",
                table: "PurchaseOrder",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_PurchaseOrder_PoNo",
                schema: "CBSAP",
                table: "PurchaseOrder",
                column: "PoNo");

            migrationBuilder.CreateTable(
                name: "SupplierBankAccount",
                schema: "CBSAP",
                columns: table => new
                {
                    SupplierBankAccountID = table.Column<long>(type: "bigint", nullable: false),
                    SupplierInfoID = table.Column<long>(type: "bigint", nullable: false),
                    BankAccountNumber = table.Column<string>(type: "NVARCHAR(40)", maxLength: 40, nullable: false),
                    BankName = table.Column<string>(type: "NVARCHAR(40)", maxLength: 40, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastUpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierBankAccount", x => x.SupplierBankAccountID);
                    table.ForeignKey(
                        name: "FK_SupplierBankAccount_SupplierInfo_SupplierBankAccountID",
                        column: x => x.SupplierBankAccountID,
                        principalSchema: "CBSAP",
                        principalTable: "SupplierInfo",
                        principalColumn: "SupplierInfoID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptLine_PurchaseOrderNo",
                schema: "CBSAP",
                table: "GoodsReceiptLine",
                column: "PurchaseOrderNo");

            migrationBuilder.AddForeignKey(
                name: "FK_GoodsReceiptLine_PurchaseOrder_PurchaseOrderNo",
                schema: "CBSAP",
                table: "GoodsReceiptLine",
                column: "PurchaseOrderNo",
                principalSchema: "CBSAP",
                principalTable: "PurchaseOrder",
                principalColumn: "PoNo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoodsReceiptLine_PurchaseOrder_PurchaseOrderNo",
                schema: "CBSAP",
                table: "GoodsReceiptLine");

            migrationBuilder.DropTable(
                name: "SupplierBankAccount",
                schema: "CBSAP");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_PurchaseOrder_PoNo",
                schema: "CBSAP",
                table: "PurchaseOrder");

            migrationBuilder.DropIndex(
                name: "IX_GoodsReceiptLine_PurchaseOrderNo",
                schema: "CBSAP",
                table: "GoodsReceiptLine");

            migrationBuilder.AlterColumn<string>(
                name: "PoNo",
                schema: "CBSAP",
                table: "PurchaseOrder",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}
