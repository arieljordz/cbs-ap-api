using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnnecessaryUserLogInfoColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaximuPasswordAge",
                schema: "CBSAP",
                table: "UserLogInfo");

            migrationBuilder.DropColumn(
                name: "MaximumLogInAttemp",
                schema: "CBSAP",
                table: "UserLogInfo");

            migrationBuilder.DropColumn(
                name: "MinimumPasswordAge",
                schema: "CBSAP",
                table: "UserLogInfo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaximuPasswordAge",
                schema: "CBSAP",
                table: "UserLogInfo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaximumLogInAttemp",
                schema: "CBSAP",
                table: "UserLogInfo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinimumPasswordAge",
                schema: "CBSAP",
                table: "UserLogInfo",
                type: "int",
                nullable: true);
        }
    }
}
