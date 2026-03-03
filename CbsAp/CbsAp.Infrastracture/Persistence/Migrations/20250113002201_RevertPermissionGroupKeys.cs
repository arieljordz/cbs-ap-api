using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RevertPermissionGroupKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionGroup_Permission_PermissionID",
                schema: "CBSAP",
                table: "PermissionGroup");

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionGroup_Permission_PermissionID",
                schema: "CBSAP",
                table: "PermissionGroup",
                column: "PermissionID",
                principalSchema: "CBSAP",
                principalTable: "Permission",
                principalColumn: "PermissionID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionGroup_Permission_PermissionID",
                schema: "CBSAP",
                table: "PermissionGroup");

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionGroup_Permission_PermissionID",
                schema: "CBSAP",
                table: "PermissionGroup",
                column: "PermissionID",
                principalSchema: "CBSAP",
                principalTable: "Permission",
                principalColumn: "PermissionID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
