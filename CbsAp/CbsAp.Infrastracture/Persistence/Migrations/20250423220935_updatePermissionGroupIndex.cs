using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatePermissionGroupIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_PermissionGroup_PermissionID_OperationID",
                schema: "CBSAP",
                table: "PermissionGroup");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroup_PermissionID_OperationID",
                schema: "CBSAP",
                table: "PermissionGroup",
                columns: new[] { "PermissionID", "OperationID" }
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PermissionGroup_PermissionID_OperationID",
                schema: "CBSAP",
                table: "PermissionGroup");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_PermissionGroup_PermissionID_OperationID",
                schema: "CBSAP",
                table: "PermissionGroup",
                columns: new[] { "PermissionID", "OperationID" });
        }
    }
}
