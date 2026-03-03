using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLineManagertoRoleManagerColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LineManager1",
                schema: "CBSAP",
                table: "Role",
                newName: "RoleManager1");

            migrationBuilder.RenameColumn(
                name: "LineManager2",
                schema: "CBSAP",
                table: "Role",
                newName: "RoleManager2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
               name: "LineManager1",
               schema: "CBSAP",
               table: "Role",
               newName: "RoleManager1");

            migrationBuilder.RenameColumn(
                name: "LineManager2",
                schema: "CBSAP",
                table: "Role",
                newName: "RoleManager2");
        }
    }
}
