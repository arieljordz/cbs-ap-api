using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InvoiceArchiveModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvoiceArchive",
                schema: "CBSAP",
                columns: table => new
                {
                    InvoiceID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InvoiceDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    MapID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ImageID = table.Column<string>(type: "char(16)", maxLength: 16, nullable: true),
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
                    FreeField1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FreeField2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FreeField3 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SpareAmount1 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SpareAmount2 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SpareAmount3 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    QueueType = table.Column<int>(type: "int", nullable: true),
                    StatusType = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastUpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceArchive", x => x.InvoiceID);
                    table.ForeignKey(
                        name: "FK_InvoiceArchive_EntityProfile_EntityProfileID",
                        column: x => x.EntityProfileID,
                        principalSchema: "CBSAP",
                        principalTable: "EntityProfile",
                        principalColumn: "EntityProfileID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_InvoiceArchive_SupplierInfo_SupplierInfoID",
                        column: x => x.SupplierInfoID,
                        principalSchema: "CBSAP",
                        principalTable: "SupplierInfo",
                        principalColumn: "SupplierInfoID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_InvoiceArchive_TaxCode_TaxCodeID",
                        column: x => x.TaxCodeID,
                        principalSchema: "CBSAP",
                        principalTable: "TaxCode",
                        principalColumn: "TaxCodeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvAllocLineArchive",
                schema: "CBSAP",
                columns: table => new
                {
                    InvAllocLineID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceID = table.Column<long>(type: "bigint", nullable: true),
                    LineNo = table.Column<long>(type: "bigint", nullable: true),
                    PoNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PoLineNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AccountID = table.Column<long>(type: "bigint", nullable: true),
                    LineDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Qty = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    LineNetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LineTaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LineAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TaxCodeID = table.Column<long>(type: "bigint", nullable: true),
                    LineApproved = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastUpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvAllocLineArchive", x => x.InvAllocLineID);
                    table.ForeignKey(
                        name: "FK_InvAllocLineArchive_Account_AccountID",
                        column: x => x.AccountID,
                        principalSchema: "CBSAP",
                        principalTable: "Account",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvAllocLineArchive_InvoiceArchive_InvoiceID",
                        column: x => x.InvoiceID,
                        principalSchema: "CBSAP",
                        principalTable: "InvoiceArchive",
                        principalColumn: "InvoiceID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvAllocLineArchive_TaxCode_TaxCodeID",
                        column: x => x.TaxCodeID,
                        principalSchema: "CBSAP",
                        principalTable: "TaxCode",
                        principalColumn: "TaxCodeID");
                });

            migrationBuilder.CreateTable(
                name: "InvoiceActivityLogArchive",
                schema: "CBSAP",
                columns: table => new
                {
                    ActivityLogID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceID = table.Column<long>(type: "bigint", nullable: false),
                    PreviousStatus = table.Column<int>(type: "int", nullable: true),
                    CurrentStatus = table.Column<int>(type: "int", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Action = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceActivityLogArchive", x => x.ActivityLogID);
                    table.ForeignKey(
                        name: "FK_InvoiceActivityLogArchive_InvoiceArchive_InvoiceID",
                        column: x => x.InvoiceID,
                        principalSchema: "CBSAP",
                        principalTable: "InvoiceArchive",
                        principalColumn: "InvoiceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceAttachnmentArchive",
                schema: "CBSAP",
                columns: table => new
                {
                    InvoiceAttachnmentID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceID = table.Column<long>(type: "bigint", nullable: false),
                    OriginalFileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    StorageFileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FileType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceAttachnmentArchive", x => x.InvoiceAttachnmentID);
                    table.ForeignKey(
                        name: "FK_InvoiceAttachnmentArchive_InvoiceArchive_InvoiceID",
                        column: x => x.InvoiceID,
                        principalSchema: "CBSAP",
                        principalTable: "InvoiceArchive",
                        principalColumn: "InvoiceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceCommentArchive",
                schema: "CBSAP",
                columns: table => new
                {
                    InvoiceCommentID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    InvoiceID = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceCommentArchive", x => x.InvoiceCommentID);
                    table.ForeignKey(
                        name: "FK_InvoiceCommentArchive_InvoiceArchive_InvoiceID",
                        column: x => x.InvoiceID,
                        principalSchema: "CBSAP",
                        principalTable: "InvoiceArchive",
                        principalColumn: "InvoiceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvAllocLineDimensionArchive",
                schema: "CBSAP",
                columns: table => new
                {
                    InvAllocLineDimensionID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvAllocLineID = table.Column<long>(type: "bigint", nullable: true),
                    DimensionKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DimensionValue = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvAllocLineDimensionArchive", x => x.InvAllocLineDimensionID);
                    table.ForeignKey(
                        name: "FK_InvAllocLineDimensionArchive_InvAllocLineArchive_InvAllocLineID",
                        column: x => x.InvAllocLineID,
                        principalSchema: "CBSAP",
                        principalTable: "InvAllocLineArchive",
                        principalColumn: "InvAllocLineID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvAllocLineFreeFieldArchive",
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
                    table.PrimaryKey("PK_InvAllocLineFreeFieldArchive", x => x.InvAllocLineFieldID);
                    table.ForeignKey(
                        name: "FK_InvAllocLineFreeFieldArchive_InvAllocLineArchive_InvAllocLineID",
                        column: x => x.InvAllocLineID,
                        principalSchema: "CBSAP",
                        principalTable: "InvAllocLineArchive",
                        principalColumn: "InvAllocLineID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderMatchTrackingArchive",
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
                    RemainingQty = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
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
                    table.PrimaryKey("PK_PurchaseOrderMatchTrackingArchive", x => x.PurchaseOrderMatchTrackingID);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderMatchTrackingArchive_InvAllocLineArchive_InvAllocLineID",
                        column: x => x.InvAllocLineID,
                        principalSchema: "CBSAP",
                        principalTable: "InvAllocLineArchive",
                        principalColumn: "InvAllocLineID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderMatchTrackingArchive_InvoiceArchive_InvoiceID",
                        column: x => x.InvoiceID,
                        principalSchema: "CBSAP",
                        principalTable: "InvoiceArchive",
                        principalColumn: "InvoiceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderMatchTrackingArchive_PurchaseOrderLine_PurchaseOrderLineID",
                        column: x => x.PurchaseOrderLineID,
                        principalSchema: "CBSAP",
                        principalTable: "PurchaseOrderLine",
                        principalColumn: "PurchaseOrderLineID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderMatchTrackingArchive_PurchaseOrder_PurchaseOrderID",
                        column: x => x.PurchaseOrderID,
                        principalSchema: "CBSAP",
                        principalTable: "PurchaseOrder",
                        principalColumn: "PurchaseOrderID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvAllocLineArchive_AccountID",
                schema: "CBSAP",
                table: "InvAllocLineArchive",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_InvAllocLineArchive_InvoiceID",
                schema: "CBSAP",
                table: "InvAllocLineArchive",
                column: "InvoiceID");

            migrationBuilder.CreateIndex(
                name: "IX_InvAllocLineArchive_TaxCodeID",
                schema: "CBSAP",
                table: "InvAllocLineArchive",
                column: "TaxCodeID");

            migrationBuilder.CreateIndex(
                name: "IX_InvAllocLineDimensionArchive_InvAllocLineID",
                schema: "CBSAP",
                table: "InvAllocLineDimensionArchive",
                column: "InvAllocLineID");

            migrationBuilder.CreateIndex(
                name: "IX_InvAllocLineFreeFieldArchive_InvAllocLineID",
                schema: "CBSAP",
                table: "InvAllocLineFreeFieldArchive",
                column: "InvAllocLineID");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceActivityLogArchive_InvoiceID",
                schema: "CBSAP",
                table: "InvoiceActivityLogArchive",
                column: "InvoiceID");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceArchive_EntityProfileID",
                schema: "CBSAP",
                table: "InvoiceArchive",
                column: "EntityProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceArchive_SupplierInfoID",
                schema: "CBSAP",
                table: "InvoiceArchive",
                column: "SupplierInfoID");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceArchive_TaxCodeID",
                schema: "CBSAP",
                table: "InvoiceArchive",
                column: "TaxCodeID");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceAttachnmentArchive_InvoiceID",
                schema: "CBSAP",
                table: "InvoiceAttachnmentArchive",
                column: "InvoiceID");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceCommentArchive_InvoiceID",
                schema: "CBSAP",
                table: "InvoiceCommentArchive",
                column: "InvoiceID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderMatchTrackingArchive_InvAllocLineID",
                schema: "CBSAP",
                table: "PurchaseOrderMatchTrackingArchive",
                column: "InvAllocLineID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderMatchTrackingArchive_InvoiceID",
                schema: "CBSAP",
                table: "PurchaseOrderMatchTrackingArchive",
                column: "InvoiceID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderMatchTrackingArchive_PurchaseOrderID",
                schema: "CBSAP",
                table: "PurchaseOrderMatchTrackingArchive",
                column: "PurchaseOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderMatchTrackingArchive_PurchaseOrderLineID",
                schema: "CBSAP",
                table: "PurchaseOrderMatchTrackingArchive",
                column: "PurchaseOrderLineID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvAllocLineDimensionArchive",
                schema: "CBSAP");

            migrationBuilder.DropTable(
                name: "InvAllocLineFreeFieldArchive",
                schema: "CBSAP");

            migrationBuilder.DropTable(
                name: "InvoiceActivityLogArchive",
                schema: "CBSAP");

            migrationBuilder.DropTable(
                name: "InvoiceAttachnmentArchive",
                schema: "CBSAP");

            migrationBuilder.DropTable(
                name: "InvoiceCommentArchive",
                schema: "CBSAP");

            migrationBuilder.DropTable(
                name: "PurchaseOrderMatchTrackingArchive",
                schema: "CBSAP");

            migrationBuilder.DropTable(
                name: "InvAllocLineArchive",
                schema: "CBSAP");

            migrationBuilder.DropTable(
                name: "InvoiceArchive",
                schema: "CBSAP");
        }
    }
}
