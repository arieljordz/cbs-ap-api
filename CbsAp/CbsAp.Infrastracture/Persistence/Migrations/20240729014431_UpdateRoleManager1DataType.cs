using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoleManager1DataType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Role_RoleManager1",
                schema: "CBSAP",
                table: "Role",
                column: "RoleManager1");

            migrationBuilder.AddForeignKey(
                name: "FK_Role_Role_RoleManager1",
                schema: "CBSAP",
                table: "Role",
                column: "RoleManager1",
                principalSchema: "CBSAP",
                principalTable: "Role",
                principalColumn: "RoleID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Role_Role_RoleManager1",
                schema: "CBSAP",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_Role_RoleManager1",
                schema: "CBSAP",
                table: "Role");
        }
    }
}
