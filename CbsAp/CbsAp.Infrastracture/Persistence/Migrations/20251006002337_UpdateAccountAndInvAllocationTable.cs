using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAccountAndInvAllocationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Account",
                schema: "CBSAP",
                table: "InvAllocLine");

            migrationBuilder.AddColumn<long>(
                name: "AccountID",
                schema: "CBSAP",
                table: "InvAllocLine",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvAllocLine_AccountID",
                schema: "CBSAP",
                table: "InvAllocLine",
                column: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_InvAllocLine_Account_AccountID",
                schema: "CBSAP",
                table: "InvAllocLine",
                column: "AccountID",
                principalSchema: "CBSAP",
                principalTable: "Account",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvAllocLine_Account_AccountID",
                schema: "CBSAP",
                table: "InvAllocLine");

            migrationBuilder.DropIndex(
                name: "IX_InvAllocLine_AccountID",
                schema: "CBSAP",
                table: "InvAllocLine");

            migrationBuilder.DropColumn(
                name: "AccountID",
                schema: "CBSAP",
                table: "InvAllocLine");

            migrationBuilder.AddColumn<string>(
                name: "Account",
                schema: "CBSAP",
                table: "InvAllocLine",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}