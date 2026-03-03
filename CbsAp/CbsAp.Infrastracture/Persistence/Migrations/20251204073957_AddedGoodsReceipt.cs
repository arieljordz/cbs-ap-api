using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedGoodsReceipt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GoodReceipts",
                schema: "CBSAP",
                columns: table => new
                {
                    GoodsReceiptID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoodsReceiptNumber = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    DeliveryNote = table.Column<string>(type: "NVARCHAR(250)", maxLength: 250, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    DeliveryDate = table.Column<DateTimeOffset>(type: "DATETIMEOFFSET", nullable: true),
                    EntityProfileID = table.Column<long>(type: "bigint", nullable: false),
                    SupplierInfoID = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastUpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodReceipts", x => x.GoodsReceiptID);
                    table.ForeignKey(
                        name: "FK_GoodReceipts_EntityProfile_EntityProfileID",
                        column: x => x.EntityProfileID,
                        principalSchema: "CBSAP",
                        principalTable: "EntityProfile",
                        principalColumn: "EntityProfileID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GoodReceipts_SupplierInfo_SupplierInfoID",
                        column: x => x.SupplierInfoID,
                        principalSchema: "CBSAP",
                        principalTable: "SupplierInfo",
                        principalColumn: "SupplierInfoID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GoodReceipts_EntityProfileID",
                schema: "CBSAP",
                table: "GoodReceipts",
                column: "EntityProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_GoodReceipts_SupplierInfoID",
                schema: "CBSAP",
                table: "GoodReceipts",
                column: "SupplierInfoID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoodReceipts",
                schema: "CBSAP");
        }
    }
}
