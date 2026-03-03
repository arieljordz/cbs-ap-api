using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddKeywordEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Keyword",
                schema: "CBSAP",
                columns: table => new
                {
                    KeywordID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityProfileID = table.Column<long>(type: "bigint", nullable: true),
                    InvoiceRoutingFlowID = table.Column<long>(type: "bigint", nullable: false),
                    KeywordName = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastUpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keyword", x => x.KeywordID);
                    table.ForeignKey(
                        name: "FK_Keyword_EntityProfile_EntityProfileID",
                        column: x => x.EntityProfileID,
                        principalSchema: "CBSAP",
                        principalTable: "EntityProfile",
                        principalColumn: "EntityProfileID");
                    table.ForeignKey(
                        name: "FK_Keyword_InvRoutingFlow_InvoiceRoutingFlowID",
                        column: x => x.InvoiceRoutingFlowID,
                        principalSchema: "CBSAP",
                        principalTable: "InvRoutingFlow",
                        principalColumn: "InvRoutingFlowID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Keyword_EntityProfileID",
                schema: "CBSAP",
                table: "Keyword",
                column: "EntityProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_Keyword_InvoiceRoutingFlowID",
                schema: "CBSAP",
                table: "Keyword",
                column: "InvoiceRoutingFlowID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Keyword",
                schema: "CBSAP");
        }
    }
}
