using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedKeywordInInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Active",
                schema: "CBSAP",
                table: "Dimension",
                newName: "IsActive");

            migrationBuilder.AlterColumn<string>(
                name: "KeywordName",
                schema: "CBSAP",
                table: "Keyword",
                type: "NVARCHAR(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "NVARCHAR(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "KeywordID",
                schema: "CBSAP",
                table: "InvoiceArchive",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "KeywordID",
                schema: "CBSAP",
                table: "Invoice",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceArchive_KeywordID",
                schema: "CBSAP",
                table: "InvoiceArchive",
                column: "KeywordID");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_KeywordID",
                schema: "CBSAP",
                table: "Invoice",
                column: "KeywordID");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Keyword_KeywordID",
                schema: "CBSAP",
                table: "Invoice",
                column: "KeywordID",
                principalSchema: "CBSAP",
                principalTable: "Keyword",
                principalColumn: "KeywordID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceArchive_Keyword_KeywordID",
                schema: "CBSAP",
                table: "InvoiceArchive",
                column: "KeywordID",
                principalSchema: "CBSAP",
                principalTable: "Keyword",
                principalColumn: "KeywordID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Keyword_KeywordID",
                schema: "CBSAP",
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceArchive_Keyword_KeywordID",
                schema: "CBSAP",
                table: "InvoiceArchive");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceArchive_KeywordID",
                schema: "CBSAP",
                table: "InvoiceArchive");

            migrationBuilder.DropIndex(
                name: "IX_Invoice_KeywordID",
                schema: "CBSAP",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "KeywordID",
                schema: "CBSAP",
                table: "InvoiceArchive");

            migrationBuilder.DropColumn(
                name: "KeywordID",
                schema: "CBSAP",
                table: "Invoice");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                schema: "CBSAP",
                table: "Dimension",
                newName: "Active");

            migrationBuilder.AlterColumn<string>(
                name: "KeywordName",
                schema: "CBSAP",
                table: "Keyword",
                type: "NVARCHAR(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(100)",
                oldMaxLength: 100);
        }
    }
}
