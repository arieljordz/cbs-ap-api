using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoleManager1andRoleManager2DataType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "RoleManager2",
                schema: "CBSAP",
                table: "Role",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "RoleManager1",
                schema: "CBSAP",
                table: "Role",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Role_RoleManager2",
                schema: "CBSAP",
                table: "Role",
                column: "RoleManager2");

            migrationBuilder.AddForeignKey(
                name: "FK_Role_Role_RoleManager2",
                schema: "CBSAP",
                table: "Role",
                column: "RoleManager2",
                principalSchema: "CBSAP",
                principalTable: "Role",
                principalColumn: "RoleID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Role_Role_RoleManager2",
                schema: "CBSAP",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_Role_RoleManager2",
                schema: "CBSAP",
                table: "Role");

            migrationBuilder.AlterColumn<string>(
                name: "RoleManager2",
                schema: "CBSAP",
                table: "Role",
                type: "NVARCHAR(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RoleManager1",
                schema: "CBSAP",
                table: "Role",
                type: "NVARCHAR(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
