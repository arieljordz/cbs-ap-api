using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInvoiceGreaterThanPOAndAddInvoiceLessThanPoApproved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InvoiceNetLessThanPO",
                schema: "CBSAP",
                table: "EntityProfile",
                newName: "InvoiceNetLessThanPOException");

            migrationBuilder.RenameColumn(
                name: "InvoiceNetGreaterThanPO",
                schema: "CBSAP",
                table: "EntityProfile",
                newName: "InvoiceNetLessThanPOApproved");

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

            migrationBuilder.AddColumn<bool>(
                name: "InvoiceNetGreaterThanPOApproved",
                schema: "CBSAP",
                table: "EntityProfile",
                type: "bit",
                nullable: false,
                defaultValue: false);

            //migrationBuilder.AddUniqueConstraint(
            //    name: "AK_PurchaseOrder_PoNo",
            //    schema: "CBSAP",
            //    table: "PurchaseOrder",
            //    column: "PoNo");

            //migrationBuilder.CreateIndex(
            //    name: "IX_GoodsReceiptLine_PurchaseOrderNo",
            //    schema: "CBSAP",
            //    table: "GoodsReceiptLine",
            //    column: "PurchaseOrderNo");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_GoodsReceiptLine_PurchaseOrder_PurchaseOrderNo",
            //    schema: "CBSAP",
            //    table: "GoodsReceiptLine",
            //    column: "PurchaseOrderNo",
            //    principalSchema: "CBSAP",
            //    principalTable: "PurchaseOrder",
            //    principalColumn: "PoNo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoodsReceiptLine_PurchaseOrder_PurchaseOrderNo",
                schema: "CBSAP",
                table: "GoodsReceiptLine");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_PurchaseOrder_PoNo",
                schema: "CBSAP",
                table: "PurchaseOrder");

            migrationBuilder.DropIndex(
                name: "IX_GoodsReceiptLine_PurchaseOrderNo",
                schema: "CBSAP",
                table: "GoodsReceiptLine");

            migrationBuilder.DropColumn(
                name: "InvoiceNetGreaterThanPOApproved",
                schema: "CBSAP",
                table: "EntityProfile");

            migrationBuilder.RenameColumn(
                name: "InvoiceNetLessThanPOException",
                schema: "CBSAP",
                table: "EntityProfile",
                newName: "InvoiceNetLessThanPO");

            migrationBuilder.RenameColumn(
                name: "InvoiceNetLessThanPOApproved",
                schema: "CBSAP",
                table: "EntityProfile",
                newName: "InvoiceNetGreaterThanPO");

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
