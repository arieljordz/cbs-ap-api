using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserAndApprovedRoleColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ApproverRole",
                schema: "CBSAP",
                table: "InvoiceArchive",
                type: "bigint",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ApprovedUser",
                schema: "CBSAP",
                table: "InvoiceArchive",
                type: "bigint",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ApproverRole",
                schema: "CBSAP",
                table: "Invoice",
                type: "bigint",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ApprovedUser",
                schema: "CBSAP",
                table: "Invoice",
                type: "bigint",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceArchive_ApprovedUser",
                schema: "CBSAP",
                table: "InvoiceArchive",
                column: "ApprovedUser");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceArchive_ApproverRole",
                schema: "CBSAP",
                table: "InvoiceArchive",
                column: "ApproverRole");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_ApprovedUser",
                schema: "CBSAP",
                table: "Invoice",
                column: "ApprovedUser");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_ApproverRole",
                schema: "CBSAP",
                table: "Invoice",
                column: "ApproverRole");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Role_ApprovedUser",
                schema: "CBSAP",
                table: "Invoice",
                column: "ApprovedUser",
                principalSchema: "CBSAP",
                principalTable: "Role",
                principalColumn: "RoleID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Role_ApproverRole",
                schema: "CBSAP",
                table: "Invoice",
                column: "ApproverRole",
                principalSchema: "CBSAP",
                principalTable: "Role",
                principalColumn: "RoleID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceArchive_Role_ApprovedUser",
                schema: "CBSAP",
                table: "InvoiceArchive",
                column: "ApprovedUser",
                principalSchema: "CBSAP",
                principalTable: "Role",
                principalColumn: "RoleID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceArchive_Role_ApproverRole",
                schema: "CBSAP",
                table: "InvoiceArchive",
                column: "ApproverRole",
                principalSchema: "CBSAP",
                principalTable: "Role",
                principalColumn: "RoleID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Role_ApprovedUser",
                schema: "CBSAP",
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Role_ApproverRole",
                schema: "CBSAP",
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceArchive_Role_ApprovedUser",
                schema: "CBSAP",
                table: "InvoiceArchive");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceArchive_Role_ApproverRole",
                schema: "CBSAP",
                table: "InvoiceArchive");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceArchive_ApprovedUser",
                schema: "CBSAP",
                table: "InvoiceArchive");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceArchive_ApproverRole",
                schema: "CBSAP",
                table: "InvoiceArchive");

            migrationBuilder.DropIndex(
                name: "IX_Invoice_ApprovedUser",
                schema: "CBSAP",
                table: "Invoice");

            migrationBuilder.DropIndex(
                name: "IX_Invoice_ApproverRole",
                schema: "CBSAP",
                table: "Invoice");

            migrationBuilder.AlterColumn<string>(
                name: "ApproverRole",
                schema: "CBSAP",
                table: "InvoiceArchive",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApprovedUser",
                schema: "CBSAP",
                table: "InvoiceArchive",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApproverRole",
                schema: "CBSAP",
                table: "Invoice",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApprovedUser",
                schema: "CBSAP",
                table: "Invoice",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
