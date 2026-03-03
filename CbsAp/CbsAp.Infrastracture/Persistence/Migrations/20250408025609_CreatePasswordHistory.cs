using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreatePasswordHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PasswordHistory",
                schema: "CBSAP",
                columns: table => new
                {
                    PasswordHistoryID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserAccountID = table.Column<long>(type: "bigint", nullable: false),
                    PasswordHash = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: true),
                    PasswordSalt = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordHistory", x => x.PasswordHistoryID);
                    table.ForeignKey(
                        name: "FK_PasswordHistory_UserAccount_UserAccountID",
                        column: x => x.UserAccountID,
                        principalSchema: "CBSAP",
                        principalTable: "UserAccount",
                        principalColumn: "UserAccountID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PasswordHistory_UserAccountID",
                schema: "CBSAP",
                table: "PasswordHistory",
                column: "UserAccountID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PasswordHistory",
                schema: "CBSAP");
        }
    }
}
