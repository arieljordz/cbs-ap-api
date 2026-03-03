using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePermissionGroupKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionGroup_Permission_PermissionID",
                schema: "CBSAP",
                table: "PermissionGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissionGroup_PermissionGroup_PermissionID",
                schema: "CBSAP",
                table: "RolePermissionGroup");

           

            migrationBuilder.AddUniqueConstraint(
                name: "AK_PermissionGroup_PermissionID",
                schema: "CBSAP",
                table: "PermissionGroup",
                column: "PermissionID");


            migrationBuilder.AddForeignKey(
                name: "FK_PermissionGroup_Permission_PermissionID",
                schema: "CBSAP",
                table: "PermissionGroup",
                column: "PermissionID",
                principalSchema: "CBSAP",
                principalTable: "Permission",
                principalColumn: "PermissionID",
                onDelete: ReferentialAction.Cascade);


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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionGroup_Permission_PermissionID",
                schema: "CBSAP",
                table: "PermissionGroup");


            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissionGroup_PermissionGroup_PermissionID",
                schema: "CBSAP",
                table: "RolePermissionGroup");

           

            migrationBuilder.DropUniqueConstraint(
                name: "AK_PermissionGroup_PermissionID",
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

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissionGroup_PermissionGroup_PermissionID",
                schema: "CBSAP",
                table: "RolePermissionGroup",
                column: "PermissionID",
                principalSchema: "CBSAP",
                principalTable: "PermissionGroup",
                principalColumn: "PermissionGroupID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
