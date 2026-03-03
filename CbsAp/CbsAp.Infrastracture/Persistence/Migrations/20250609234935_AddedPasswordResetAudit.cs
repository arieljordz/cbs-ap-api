using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedPasswordResetAudit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleEntity_EntityProfile_EntityProfileID",
                schema: "CBSAP",
                table: "RoleEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleEntity_Role_RoleID",
                schema: "CBSAP",
                table: "RoleEntity");

            migrationBuilder.AlterColumn<string>(
                name: "RoleName",
                schema: "CBSAP",
                table: "Role",
                type: "NVARCHAR(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "NVARCHAR(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PasswordResetAudits",
                schema: "CBSAP",
                columns: table => new
                {
                    PasswordResetAuditID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserAccountID = table.Column<long>(type: "bigint", nullable: false),
                    IsSuccessfulLoginAfterReset = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetAudits", x => x.PasswordResetAuditID);
                    table.ForeignKey(
                        name: "FK_PasswordResetAudits_UserAccount_UserAccountID",
                        column: x => x.UserAccountID,
                        principalSchema: "CBSAP",
                        principalTable: "UserAccount",
                        principalColumn: "UserAccountID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetAudits_UserAccountID_CreatedDate",
                schema: "CBSAP",
                table: "PasswordResetAudits",
                columns: new[] { "UserAccountID", "CreatedDate" });

            migrationBuilder.AddForeignKey(
                name: "FK_RoleEntity_EntityProfile_EntityProfileID",
                schema: "CBSAP",
                table: "RoleEntity",
                column: "EntityProfileID",
                principalSchema: "CBSAP",
                principalTable: "EntityProfile",
                principalColumn: "EntityProfileID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleEntity_Role_RoleID",
                schema: "CBSAP",
                table: "RoleEntity",
                column: "RoleID",
                principalSchema: "CBSAP",
                principalTable: "Role",
                principalColumn: "RoleID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleEntity_EntityProfile_EntityProfileID",
                schema: "CBSAP",
                table: "RoleEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleEntity_Role_RoleID",
                schema: "CBSAP",
                table: "RoleEntity");

            migrationBuilder.DropTable(
                name: "PasswordResetAudits",
                schema: "CBSAP");

            migrationBuilder.AlterColumn<string>(
                name: "RoleName",
                schema: "CBSAP",
                table: "Role",
                type: "NVARCHAR(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(200)",
                oldMaxLength: 200);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleEntity_EntityProfile_EntityProfileID",
                schema: "CBSAP",
                table: "RoleEntity",
                column: "EntityProfileID",
                principalSchema: "CBSAP",
                principalTable: "EntityProfile",
                principalColumn: "EntityProfileID");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleEntity_Role_RoleID",
                schema: "CBSAP",
                table: "RoleEntity",
                column: "RoleID",
                principalSchema: "CBSAP",
                principalTable: "Role",
                principalColumn: "RoleID");
        }
    }
}
