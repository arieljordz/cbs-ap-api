using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SetTaxCodeIdNullableOnAllocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvAllocLine_TaxCode_TaxCodeID",
                schema: "CBSAP",
                table: "InvAllocLine");

            migrationBuilder.AlterColumn<long>(
                name: "TaxCodeID",
                schema: "CBSAP",
                table: "InvAllocLine",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_InvAllocLine_TaxCode_TaxCodeID",
                schema: "CBSAP",
                table: "InvAllocLine",
                column: "TaxCodeID",
                principalSchema: "CBSAP",
                principalTable: "TaxCode",
                principalColumn: "TaxCodeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvAllocLine_TaxCode_TaxCodeID",
                schema: "CBSAP",
                table: "InvAllocLine");

            migrationBuilder.AlterColumn<long>(
                name: "TaxCodeID",
                schema: "CBSAP",
                table: "InvAllocLine",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InvAllocLine_TaxCode_TaxCodeID",
                schema: "CBSAP",
                table: "InvAllocLine",
                column: "TaxCodeID",
                principalSchema: "CBSAP",
                principalTable: "TaxCode",
                principalColumn: "TaxCodeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
