using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class recreate_supplierbankaccounttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupplierBankAccount",
                schema: "CBSAP",
                columns: table => new
                {
                    SupplierBankAccountID = table.Column<long>(type: "bigint", nullable: false)
                                                 .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierInfoID = table.Column<long>(type: "bigint", nullable: false),
                    BankAccountNumber = table.Column<string>(type: "NVARCHAR(40)", maxLength: 40, nullable: false),
                    BankName = table.Column<string>(type: "NVARCHAR(40)", maxLength: 40, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastUpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierBankAccount", x => x.SupplierBankAccountID);
                    table.ForeignKey(
                        name: "FK_SupplierBankAccount_SupplierInfo_SupplierBankAccountID",
                        column: x => x.SupplierBankAccountID,
                        principalSchema: "CBSAP",
                        principalTable: "SupplierInfo",
                        principalColumn: "SupplierInfoID",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupplierBankAccount",
                schema: "CBSAP");
        }
    }
}
