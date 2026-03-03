using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOperationIdColumnMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ControlElement_Operation_OperationID",
                schema: "CBSAP",
                table: "ControlElement");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuItem_Menu_MenuID",
                schema: "CBSAP",
                table: "MenuItem");

            migrationBuilder.AddColumn<long>(
                name: "OperationID",
                schema: "CBSAP",
                table: "Menu",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Menu_OperationID",
                schema: "CBSAP",
                table: "Menu",
                column: "OperationID");

            migrationBuilder.AddForeignKey(
                name: "FK_ControlElement_Operation_OperationID",
                schema: "CBSAP",
                table: "ControlElement",
                column: "OperationID",
                principalSchema: "CBSAP",
                principalTable: "Operation",
                principalColumn: "OperationID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Menu_Operation_OperationID",
                schema: "CBSAP",
                table: "Menu",
                column: "OperationID",
                principalSchema: "CBSAP",
                principalTable: "Operation",
                principalColumn: "OperationID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItem_Menu_MenuID",
                schema: "CBSAP",
                table: "MenuItem",
                column: "MenuID",
                principalSchema: "CBSAP",
                principalTable: "Menu",
                principalColumn: "MenuID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ControlElement_Operation_OperationID",
                schema: "CBSAP",
                table: "ControlElement");

            migrationBuilder.DropForeignKey(
                name: "FK_Menu_Operation_OperationID",
                schema: "CBSAP",
                table: "Menu");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuItem_Menu_MenuID",
                schema: "CBSAP",
                table: "MenuItem");

            migrationBuilder.DropIndex(
                name: "IX_Menu_OperationID",
                schema: "CBSAP",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "OperationID",
                schema: "CBSAP",
                table: "Menu");

            migrationBuilder.AddForeignKey(
                name: "FK_ControlElement_Operation_OperationID",
                schema: "CBSAP",
                table: "ControlElement",
                column: "OperationID",
                principalSchema: "CBSAP",
                principalTable: "Operation",
                principalColumn: "OperationID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItem_Menu_MenuID",
                schema: "CBSAP",
                table: "MenuItem",
                column: "MenuID",
                principalSchema: "CBSAP",
                principalTable: "Menu",
                principalColumn: "MenuID");
        }
    }
}
