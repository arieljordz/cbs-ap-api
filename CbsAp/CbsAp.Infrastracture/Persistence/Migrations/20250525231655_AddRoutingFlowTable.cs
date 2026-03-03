using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRoutingFlowTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "InvRoutingFlowName",
                schema: "CBSAP",
                table: "InvRoutingFlow",
                type: "NVARCHAR(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(30)",
                oldMaxLength: 30);

            migrationBuilder.AddColumn<long>(
                name: "EntityProfileID",
                schema: "CBSAP",
                table: "InvRoutingFlow",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchReference",
                schema: "CBSAP",
                table: "InvRoutingFlow",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SupplierInfoID",
                schema: "CBSAP",
                table: "InvRoutingFlow",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InvRoutingFlowLevels",
                schema: "CBSAP",
                columns: table => new
                {
                    InvRoutingFlowLevelID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvRoutingFlowID = table.Column<long>(type: "bigint", nullable: false),
                    RoleID = table.Column<long>(type: "bigint", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvRoutingFlowLevels", x => x.InvRoutingFlowLevelID);
                    table.ForeignKey(
                        name: "FK_InvRoutingFlowLevels_InvRoutingFlow_InvRoutingFlowID",
                        column: x => x.InvRoutingFlowID,
                        principalSchema: "CBSAP",
                        principalTable: "InvRoutingFlow",
                        principalColumn: "InvRoutingFlowID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvRoutingFlowLevels_Role_RoleID",
                        column: x => x.RoleID,
                        principalSchema: "CBSAP",
                        principalTable: "Role",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvRoutingFlow_EntityProfileID",
                schema: "CBSAP",
                table: "InvRoutingFlow",
                column: "EntityProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_InvRoutingFlow_SupplierInfoID",
                schema: "CBSAP",
                table: "InvRoutingFlow",
                column: "SupplierInfoID");

            migrationBuilder.CreateIndex(
                name: "IX_InvRoutingFlowLevels_InvRoutingFlowID",
                schema: "CBSAP",
                table: "InvRoutingFlowLevels",
                column: "InvRoutingFlowID");

            migrationBuilder.CreateIndex(
                name: "IX_InvRoutingFlowLevels_RoleID",
                schema: "CBSAP",
                table: "InvRoutingFlowLevels",
                column: "RoleID");

            migrationBuilder.AddForeignKey(
                name: "FK_InvRoutingFlow_EntityProfile_EntityProfileID",
                schema: "CBSAP",
                table: "InvRoutingFlow",
                column: "EntityProfileID",
                principalSchema: "CBSAP",
                principalTable: "EntityProfile",
                principalColumn: "EntityProfileID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvRoutingFlow_SupplierInfo_SupplierInfoID",
                schema: "CBSAP",
                table: "InvRoutingFlow",
                column: "SupplierInfoID",
                principalSchema: "CBSAP",
                principalTable: "SupplierInfo",
                principalColumn: "SupplierInfoID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvRoutingFlow_EntityProfile_EntityProfileID",
                schema: "CBSAP",
                table: "InvRoutingFlow");

            migrationBuilder.DropForeignKey(
                name: "FK_InvRoutingFlow_SupplierInfo_SupplierInfoID",
                schema: "CBSAP",
                table: "InvRoutingFlow");

            migrationBuilder.DropTable(
                name: "InvRoutingFlowLevels",
                schema: "CBSAP");

            migrationBuilder.DropIndex(
                name: "IX_InvRoutingFlow_EntityProfileID",
                schema: "CBSAP",
                table: "InvRoutingFlow");

            migrationBuilder.DropIndex(
                name: "IX_InvRoutingFlow_SupplierInfoID",
                schema: "CBSAP",
                table: "InvRoutingFlow");

            migrationBuilder.DropColumn(
                name: "EntityProfileID",
                schema: "CBSAP",
                table: "InvRoutingFlow");

            migrationBuilder.DropColumn(
                name: "MatchReference",
                schema: "CBSAP",
                table: "InvRoutingFlow");

            migrationBuilder.DropColumn(
                name: "SupplierInfoID",
                schema: "CBSAP",
                table: "InvRoutingFlow");

            migrationBuilder.AlterColumn<string>(
                name: "InvRoutingFlowName",
                schema: "CBSAP",
                table: "InvRoutingFlow",
                type: "NVARCHAR(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "NVARCHAR(30)",
                oldMaxLength: 30,
                oldNullable: true);
        }
    }
}
