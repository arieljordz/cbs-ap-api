using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAccountTableForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Account_EntityProfileID",
                schema: "CBSAP",
                table: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Account_TaxCodeID",
                schema: "CBSAP",
                table: "Account");

            migrationBuilder.CreateIndex(
                name: "IX_Account_EntityProfileID",
                schema: "CBSAP",
                table: "Account",
                column: "EntityProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_Account_TaxCodeID",
                schema: "CBSAP",
                table: "Account",
                column: "TaxCodeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Account_EntityProfileID",
                schema: "CBSAP",
                table: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Account_TaxCodeID",
                schema: "CBSAP",
                table: "Account");

            migrationBuilder.CreateIndex(
                name: "IX_Account_EntityProfileID",
                schema: "CBSAP",
                table: "Account",
                column: "EntityProfileID",
                unique: true,
                filter: "[EntityProfileID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Account_TaxCodeID",
                schema: "CBSAP",
                table: "Account",
                column: "TaxCodeID",
                unique: true,
                filter: "[TaxCodeID] IS NOT NULL");
        }
    }
}