using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedInvInfoRoutingLevelTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "InvRoutingFlowID",
                schema: "CBSAP",
                table: "Invoice",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InvInfoRoutingLevel",
                schema: "CBSAP",
                columns: table => new
                {
                    InvInfoRoutingLevelID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvRoutingFlowID = table.Column<long>(type: "bigint", nullable: true),
                    InvoiceID = table.Column<long>(type: "bigint", nullable: true),
                    RoleID = table.Column<long>(type: "bigint", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvInfoRoutingLevel", x => x.InvInfoRoutingLevelID);
                    table.ForeignKey(
                        name: "FK_InvInfoRoutingLevel_InvRoutingFlow_InvRoutingFlowID",
                        column: x => x.InvRoutingFlowID,
                        principalSchema: "CBSAP",
                        principalTable: "InvRoutingFlow",
                        principalColumn: "InvRoutingFlowID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvInfoRoutingLevel_Invoice_InvoiceID",
                        column: x => x.InvoiceID,
                        principalSchema: "CBSAP",
                        principalTable: "Invoice",
                        principalColumn: "InvoiceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvInfoRoutingLevel_Role_RoleID",
                        column: x => x.RoleID,
                        principalSchema: "CBSAP",
                        principalTable: "Role",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvInfoRoutingLevel_InvoiceID",
                schema: "CBSAP",
                table: "InvInfoRoutingLevel",
                column: "InvoiceID");

            migrationBuilder.CreateIndex(
                name: "IX_InvInfoRoutingLevel_InvRoutingFlowID",
                schema: "CBSAP",
                table: "InvInfoRoutingLevel",
                column: "InvRoutingFlowID");

            migrationBuilder.CreateIndex(
                name: "IX_InvInfoRoutingLevel_RoleID",
                schema: "CBSAP",
                table: "InvInfoRoutingLevel",
                column: "RoleID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvInfoRoutingLevel",
                schema: "CBSAP");

            migrationBuilder.DropColumn(
                name: "InvRoutingFlowID",
                schema: "CBSAP",
                table: "Invoice");
        }
    }
}
