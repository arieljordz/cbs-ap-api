using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddInvoiceActivityLogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvoiceActivityLog",
                schema: "CBSAP",
                columns: table => new
                {
                    ActivityLogID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceID = table.Column<long>(type: "bigint", nullable: false),
                    PreviousStatus = table.Column<int>(type: "int", nullable: true),
                    CurrentStatus = table.Column<int>(type: "int", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceActivityLog", x => x.ActivityLogID);
                    table.ForeignKey(
                        name: "FK_InvoiceActivityLog_Invoice_InvoiceID",
                        column: x => x.InvoiceID,
                        principalSchema: "CBSAP",
                        principalTable: "Invoice",
                        principalColumn: "InvoiceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceActivityLog_InvoiceID",
                schema: "CBSAP",
                table: "InvoiceActivityLog",
                column: "InvoiceID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceActivityLog",
                schema: "CBSAP");
        }
    }
}
