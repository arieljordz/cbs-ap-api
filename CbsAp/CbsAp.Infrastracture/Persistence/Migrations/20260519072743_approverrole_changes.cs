using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class approverrole_changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Role_ApprovedUser",
                schema: "CBSAP",
                table: "Invoice");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_UserAccount_ApprovedUser",
                schema: "CBSAP",
                table: "Invoice",
                column: "ApprovedUser",
                principalSchema: "CBSAP",
                principalTable: "UserAccount",
                principalColumn: "UserAccountID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_UserAccount_ApprovedUser",
                schema: "CBSAP",
                table: "Invoice");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Role_ApprovedUser",
                schema: "CBSAP",
                table: "Invoice",
                column: "ApprovedUser",
                principalSchema: "CBSAP",
                principalTable: "Role",
                principalColumn: "RoleID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
