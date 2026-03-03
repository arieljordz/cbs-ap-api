using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNewIsPasswordRecoveryTokenUsedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<bool>(
                name: "IsPasswordRecoveryTokenUsed",
                schema: "CBSAP",
                table: "UserLogInfo",
                type: "bit",
                nullable: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
         
            migrationBuilder.DropColumn(
                name: "IsPasswordRecoveryTokenUsed",
                schema: "CBSAP",
                table: "UserLogInfo");

        }
    }
}
