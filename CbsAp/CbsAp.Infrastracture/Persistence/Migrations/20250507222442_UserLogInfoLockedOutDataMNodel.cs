using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UserLogInfoLockedOutDataMNodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FailedLoginAttempts",
                schema: "CBSAP",
                table: "UserLogInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsLockedOut",
                schema: "CBSAP",
                table: "UserLogInfo",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastLoginDateTime",
                schema: "CBSAP",
                table: "UserLogInfo",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEndUtc",
                schema: "CBSAP",
                table: "UserLogInfo",
                type: "datetimeoffset",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FailedLoginAttempts",
                schema: "CBSAP",
                table: "UserLogInfo");

            migrationBuilder.DropColumn(
                name: "IsLockedOut",
                schema: "CBSAP",
                table: "UserLogInfo");

            migrationBuilder.DropColumn(
                name: "LastLoginDateTime",
                schema: "CBSAP",
                table: "UserLogInfo");

            migrationBuilder.DropColumn(
                name: "LockoutEndUtc",
                schema: "CBSAP",
                table: "UserLogInfo");
        }
    }
}
