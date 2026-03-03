using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPOTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AccountName",
                schema: "CBSAP",
                table: "Account",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Dimension1",
                schema: "CBSAP",
                table: "Account",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dimension2",
                schema: "CBSAP",
                table: "Account",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dimension3",
                schema: "CBSAP",
                table: "Account",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dimension4",
                schema: "CBSAP",
                table: "Account",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dimension5",
                schema: "CBSAP",
                table: "Account",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dimension6",
                schema: "CBSAP",
                table: "Account",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dimension7",
                schema: "CBSAP",
                table: "Account",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dimension8",
                schema: "CBSAP",
                table: "Account",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EntityProfileID",
                schema: "CBSAP",
                table: "Account",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FreeField1",
                schema: "CBSAP",
                table: "Account",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FreeField2",
                schema: "CBSAP",
                table: "Account",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FreeField3",
                schema: "CBSAP",
                table: "Account",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTaxCodeMandatory",
                schema: "CBSAP",
                table: "Account",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TaxCodeID",
                schema: "CBSAP",
                table: "Account",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxCodeName",
                schema: "CBSAP",
                table: "Account",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PurchaseOrder",
                schema: "CBSAP",
                columns: table => new
                {
                    PurchaseOrderID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PoNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntityProfileID = table.Column<long>(type: "bigint", nullable: true),
                    SupplierInfoID = table.Column<long>(type: "bigint", nullable: true),
                    SupplierID = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    NetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    MatchReference1 = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    MatchReference2 = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FreeField1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FreeField2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FreeField3 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastUpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrder", x => x.PurchaseOrderID);
                    table.ForeignKey(
                        name: "FK_PurchaseOrder_EntityProfile_EntityProfileID",
                        column: x => x.EntityProfileID,
                        principalSchema: "CBSAP",
                        principalTable: "EntityProfile",
                        principalColumn: "EntityProfileID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrder_SupplierInfo_SupplierInfoID",
                        column: x => x.SupplierInfoID,
                        principalSchema: "CBSAP",
                        principalTable: "SupplierInfo",
                        principalColumn: "SupplierInfoID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderLine",
                schema: "CBSAP",
                columns: table => new
                {
                    PurchaseOrderLineID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseOrderID = table.Column<long>(type: "bigint", nullable: false),
                    TaxCodeID = table.Column<long>(type: "bigint", nullable: true),
                    AccountID = table.Column<long>(type: "bigint", nullable: true),
                    LineNo = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    Qty = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    InvoicedPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    NetAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    Item = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    FreeField1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FreeField2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FreeField3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpareAmount1 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SpareAmount2 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SpareAmount3 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FullyInvoiced = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastUpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderLine", x => x.PurchaseOrderLineID);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLine_Account_AccountID",
                        column: x => x.AccountID,
                        principalSchema: "CBSAP",
                        principalTable: "Account",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLine_PurchaseOrder_PurchaseOrderID",
                        column: x => x.PurchaseOrderID,
                        principalSchema: "CBSAP",
                        principalTable: "PurchaseOrder",
                        principalColumn: "PurchaseOrderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLine_TaxCode_TaxCodeID",
                        column: x => x.TaxCodeID,
                        principalSchema: "CBSAP",
                        principalTable: "TaxCode",
                        principalColumn: "TaxCodeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_EntityProfileID",
                schema: "CBSAP",
                table: "Account",
                column: "EntityProfileID",
                unique: true,
                filter: "[EntityProfileID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Account_TaxCodeID",
                schema: "CBSAP",
                table: "Account",
                column: "TaxCodeID",
                unique: true,
                filter: "[TaxCodeID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrder_EntityProfileID",
                schema: "CBSAP",
                table: "PurchaseOrder",
                column: "EntityProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrder_SupplierInfoID",
                schema: "CBSAP",
                table: "PurchaseOrder",
                column: "SupplierInfoID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLine_AccountID",
                schema: "CBSAP",
                table: "PurchaseOrderLine",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLine_PurchaseOrderID",
                schema: "CBSAP",
                table: "PurchaseOrderLine",
                column: "PurchaseOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLine_TaxCodeID",
                schema: "CBSAP",
                table: "PurchaseOrderLine",
                column: "TaxCodeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_EntityProfile_EntityProfileID",
                schema: "CBSAP",
                table: "Account",
                column: "EntityProfileID",
                principalSchema: "CBSAP",
                principalTable: "EntityProfile",
                principalColumn: "EntityProfileID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Account_TaxCode_TaxCodeID",
                schema: "CBSAP",
                table: "Account",
                column: "TaxCodeID",
                principalSchema: "CBSAP",
                principalTable: "TaxCode",
                principalColumn: "TaxCodeID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_EntityProfile_EntityProfileID",
                schema: "CBSAP",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Account_TaxCode_TaxCodeID",
                schema: "CBSAP",
                table: "Account");

            migrationBuilder.DropTable(
                name: "PurchaseOrderLine",
                schema: "CBSAP");

            migrationBuilder.DropTable(
                name: "PurchaseOrder",
                schema: "CBSAP");

            migrationBuilder.DropIndex(
                name: "IX_Account_EntityProfileID",
                schema: "CBSAP",
                table: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Account_TaxCodeID",
                schema: "CBSAP",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Dimension1",
                schema: "CBSAP",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Dimension2",
                schema: "CBSAP",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Dimension3",
                schema: "CBSAP",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Dimension4",
                schema: "CBSAP",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Dimension5",
                schema: "CBSAP",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Dimension6",
                schema: "CBSAP",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Dimension7",
                schema: "CBSAP",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Dimension8",
                schema: "CBSAP",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "EntityProfileID",
                schema: "CBSAP",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "FreeField1",
                schema: "CBSAP",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "FreeField2",
                schema: "CBSAP",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "FreeField3",
                schema: "CBSAP",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "IsTaxCodeMandatory",
                schema: "CBSAP",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "TaxCodeID",
                schema: "CBSAP",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "TaxCodeName",
                schema: "CBSAP",
                table: "Account");

            migrationBuilder.AlterColumn<string>(
                name: "AccountName",
                schema: "CBSAP",
                table: "Account",
                type: "NVARCHAR(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
