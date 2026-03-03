using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddKeywordIDandSupplierInfoIDInvInfoLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "KeywordID",
                schema: "CBSAP",
                table: "InvInfoRoutingLevel",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SupplierInfoID",
                schema: "CBSAP",
                table: "InvInfoRoutingLevel",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvInfoRoutingLevel_KeywordID",
                schema: "CBSAP",
                table: "InvInfoRoutingLevel",
                column: "KeywordID");

            migrationBuilder.CreateIndex(
                name: "IX_InvInfoRoutingLevel_SupplierInfoID",
                schema: "CBSAP",
                table: "InvInfoRoutingLevel",
                column: "SupplierInfoID");

            migrationBuilder.AddForeignKey(
                name: "FK_InvInfoRoutingLevel_Keyword_KeywordID",
                schema: "CBSAP",
                table: "InvInfoRoutingLevel",
                column: "KeywordID",
                principalSchema: "CBSAP",
                principalTable: "Keyword",
                principalColumn: "KeywordID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvInfoRoutingLevel_SupplierInfo_SupplierInfoID",
                schema: "CBSAP",
                table: "InvInfoRoutingLevel",
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
                name: "FK_InvInfoRoutingLevel_Keyword_KeywordID",
                schema: "CBSAP",
                table: "InvInfoRoutingLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_InvInfoRoutingLevel_SupplierInfo_SupplierInfoID",
                schema: "CBSAP",
                table: "InvInfoRoutingLevel");

            migrationBuilder.DropIndex(
                name: "IX_InvInfoRoutingLevel_KeywordID",
                schema: "CBSAP",
                table: "InvInfoRoutingLevel");

            migrationBuilder.DropIndex(
                name: "IX_InvInfoRoutingLevel_SupplierInfoID",
                schema: "CBSAP",
                table: "InvInfoRoutingLevel");

            migrationBuilder.DropColumn(
                name: "KeywordID",
                schema: "CBSAP",
                table: "InvInfoRoutingLevel");

            migrationBuilder.DropColumn(
                name: "SupplierInfoID",
                schema: "CBSAP",
                table: "InvInfoRoutingLevel");
        }
    }
}
