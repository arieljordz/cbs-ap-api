using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePermissionGroupKeysConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissionGroup_PermissionGroup_PermissionID",
                schema: "CBSAP",
                table: "RolePermissionGroup");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_PermissionGroup_PermissionID",
                schema: "CBSAP",
                table: "PermissionGroup");

            migrationBuilder.AddColumn<long>(
                name: "PermissionGroupID",
                schema: "CBSAP",
                table: "RolePermissionGroup",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionGroup_PermissionGroupID",
                schema: "CBSAP",
                table: "RolePermissionGroup",
                column: "PermissionGroupID");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissionGroup_PermissionGroup_PermissionGroupID",
                schema: "CBSAP",
                table: "RolePermissionGroup",
                column: "PermissionGroupID",
                principalSchema: "CBSAP",
                principalTable: "PermissionGroup",
                principalColumn: "PermissionGroupID");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissionGroup_Permission_PermissionID",
                schema: "CBSAP",
                table: "RolePermissionGroup",
                column: "PermissionID",
                principalSchema: "CBSAP",
                principalTable: "Permission",
                principalColumn: "PermissionID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissionGroup_PermissionGroup_PermissionGroupID",
                schema: "CBSAP",
                table: "RolePermissionGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissionGroup_Permission_PermissionID",
                schema: "CBSAP",
                table: "RolePermissionGroup");

            migrationBuilder.DropIndex(
                name: "IX_RolePermissionGroup_PermissionGroupID",
                schema: "CBSAP",
                table: "RolePermissionGroup");

            migrationBuilder.DropColumn(
                name: "PermissionGroupID",
                schema: "CBSAP",
                table: "RolePermissionGroup");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_PermissionGroup_PermissionID",
                schema: "CBSAP",
                table: "PermissionGroup",
                column: "PermissionID");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissionGroup_PermissionGroup_PermissionID",
                schema: "CBSAP",
                table: "RolePermissionGroup",
                column: "PermissionID",
                principalSchema: "CBSAP",
                principalTable: "PermissionGroup",
                principalColumn: "PermissionID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
