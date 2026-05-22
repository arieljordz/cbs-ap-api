using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class fkchangessupplierbankaccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplierBankAccount_SupplierInfo_SupplierBankAccountID",
                schema: "CBSAP",
                table: "SupplierBankAccount");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierBankAccount_SupplierInfoID",
                schema: "CBSAP",
                table: "SupplierBankAccount",
                column: "SupplierInfoID");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierBankAccount_SupplierInfo_SupplierInfoID",
                schema: "CBSAP",
                table: "SupplierBankAccount",
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
                name: "FK_SupplierBankAccount_SupplierInfo_SupplierInfoID",
                schema: "CBSAP",
                table: "SupplierBankAccount");

            migrationBuilder.DropIndex(
                name: "IX_SupplierBankAccount_SupplierInfoID",
                schema: "CBSAP",
                table: "SupplierBankAccount");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierBankAccount_SupplierInfo_SupplierBankAccountID",
                schema: "CBSAP",
                table: "SupplierBankAccount",
                column: "SupplierBankAccountID",
                principalSchema: "CBSAP",
                principalTable: "SupplierInfo",
                principalColumn: "SupplierInfoID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
