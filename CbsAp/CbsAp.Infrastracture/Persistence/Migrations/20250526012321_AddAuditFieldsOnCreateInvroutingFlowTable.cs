using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditFieldsOnCreateInvroutingFlowTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "CBSAP",
                table: "InvRoutingFlow",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                schema: "CBSAP",
                table: "InvRoutingFlow",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                schema: "CBSAP",
                table: "InvRoutingFlow",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastUpdatedDate",
                schema: "CBSAP",
                table: "InvRoutingFlow",
                type: "datetimeoffset",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "CBSAP",
                table: "InvRoutingFlow");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "CBSAP",
                table: "InvRoutingFlow");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                schema: "CBSAP",
                table: "InvRoutingFlow");

            migrationBuilder.DropColumn(
                name: "LastUpdatedDate",
                schema: "CBSAP",
                table: "InvRoutingFlow");
        }
    }
}
