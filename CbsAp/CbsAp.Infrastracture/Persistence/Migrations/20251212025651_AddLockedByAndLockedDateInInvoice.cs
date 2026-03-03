using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLockedByAndLockedDateInInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LockedBy",
                schema: "CBSAP",
                table: "Invoice",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockedDate",
                schema: "CBSAP",
                table: "Invoice",
                type: "datetimeoffset",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LockedBy",
                schema: "CBSAP",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "LockedDate",
                schema: "CBSAP",
                table: "Invoice");
        }
    }
}
