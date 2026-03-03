using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddInvoiceRelatedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invoice",
                schema: "CBSAP",
                columns: table => new
                {
                    InvoiceID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InvoiceDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    MapID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ScanDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    EntityProfileID = table.Column<long>(type: "bigint", nullable: true),
                    SupplierInfoID = table.Column<long>(type: "bigint", nullable: true),
                    SuppBankAccount = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DueDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PoNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GrNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    NetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxCodeID = table.Column<long>(type: "bigint", nullable: false),
                    PaymentTerm = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ApproverRole = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApprovedUser = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    QueueType = table.Column<int>(type: "int", nullable: true),
                    StatusType = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastUpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.InvoiceID);
                    table.ForeignKey(
                        name: "FK_Invoice_EntityProfile_EntityProfileID",
                        column: x => x.EntityProfileID,
                        principalSchema: "CBSAP",
                        principalTable: "EntityProfile",
                        principalColumn: "EntityProfileID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Invoice_SupplierInfo_SupplierInfoID",
                        column: x => x.SupplierInfoID,
                        principalSchema: "CBSAP",
                        principalTable: "SupplierInfo",
                        principalColumn: "SupplierInfoID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Invoice_TaxCode_TaxCodeID",
                        column: x => x.TaxCodeID,
                        principalSchema: "CBSAP",
                        principalTable: "TaxCode",
                        principalColumn: "TaxCodeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvAllocLine",
                schema: "CBSAP",
                columns: table => new
                {
                    InvAllocLineID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceID = table.Column<long>(type: "bigint", nullable: true),
                    LineNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PoNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PoLineNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Account = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LineDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Qty = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    LineNetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LineTaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LineAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TaxCodeID = table.Column<long>(type: "bigint", nullable: false),
                    LineApproved = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastUpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvAllocLine", x => x.InvAllocLineID);
                    table.ForeignKey(
                        name: "FK_InvAllocLine_Invoice_InvoiceID",
                        column: x => x.InvoiceID,
                        principalSchema: "CBSAP",
                        principalTable: "Invoice",
                        principalColumn: "InvoiceID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvAllocLine_TaxCode_TaxCodeID",
                        column: x => x.TaxCodeID,
                        principalSchema: "CBSAP",
                        principalTable: "TaxCode",
                        principalColumn: "TaxCodeID",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "InvAllocLineDimension",
                schema: "CBSAP",
                columns: table => new
                {
                    InvAllocLineDimensionID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvAllocLineID = table.Column<long>(type: "bigint", nullable: true),
                    DimensionKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DimensionValue = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvAllocLineDimension", x => x.InvAllocLineDimensionID);
                    table.ForeignKey(
                        name: "FK_InvAllocLineDimension_InvAllocLine_InvAllocLineID",
                        column: x => x.InvAllocLineID,
                        principalSchema: "CBSAP",
                        principalTable: "InvAllocLine",
                        principalColumn: "InvAllocLineID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvAllocLineFreeField",
                schema: "CBSAP",
                columns: table => new
                {
                    InvAllocLineFieldID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvAllocLineID = table.Column<long>(type: "bigint", nullable: true),
                    FieldKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FieldValue = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvAllocLineFreeField", x => x.InvAllocLineFieldID);
                    table.ForeignKey(
                        name: "FK_InvAllocLineFreeField_InvAllocLine_InvAllocLineID",
                        column: x => x.InvAllocLineID,
                        principalSchema: "CBSAP",
                        principalTable: "InvAllocLine",
                        principalColumn: "InvAllocLineID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvAllocLine_InvoiceID",
                schema: "CBSAP",
                table: "InvAllocLine",
                column: "InvoiceID");

            migrationBuilder.CreateIndex(
                name: "IX_InvAllocLine_TaxCodeID",
                schema: "CBSAP",
                table: "InvAllocLine",
                column: "TaxCodeID");

            migrationBuilder.CreateIndex(
                name: "IX_InvAllocLineDimension_InvAllocLineID",
                schema: "CBSAP",
                table: "InvAllocLineDimension",
                column: "InvAllocLineID");

            migrationBuilder.CreateIndex(
                name: "IX_InvAllocLineFreeField_InvAllocLineID",
                schema: "CBSAP",
                table: "InvAllocLineFreeField",
                column: "InvAllocLineID");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_EntityProfileID",
                schema: "CBSAP",
                table: "Invoice",
                column: "EntityProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_SupplierInfoID",
                schema: "CBSAP",
                table: "Invoice",
                column: "SupplierInfoID");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_TaxCodeID",
                schema: "CBSAP",
                table: "Invoice",
                column: "TaxCodeID");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvAllocLineDimension",
                schema: "CBSAP");

            migrationBuilder.DropTable(
                name: "InvAllocLineFreeField",
                schema: "CBSAP");

            migrationBuilder.DropTable(
                name: "InvoiceFreeField",
                schema: "CBSAP");

            migrationBuilder.DropTable(
                name: "InvoiceSpareAmount",
                schema: "CBSAP");

            migrationBuilder.DropTable(
                name: "InvAllocLine",
                schema: "CBSAP");

            migrationBuilder.DropTable(
                name: "Invoice",
                schema: "CBSAP");
        }
    }
}