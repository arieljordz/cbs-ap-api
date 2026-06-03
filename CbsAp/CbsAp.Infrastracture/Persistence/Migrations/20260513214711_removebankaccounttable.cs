using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class removebankaccounttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupplierBankAccount",
                schema: "CBSAP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupplierBankAccount",
                schema: "CBSAP",
                columns: table => new
                {
                    BankAccountID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankAccountNumber = table.Column<string>(type: "NVARCHAR(40)", maxLength: 40, nullable: false),
                    BankName = table.Column<string>(type: "NVARCHAR(40)", maxLength: 40, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastUpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    SupplierBankAccountID = table.Column<long>(type: "bigint", nullable: false),
                    SupplierInfoID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierBankAccount", x => x.BankAccountID);
                    table.ForeignKey(
                        name: "FK_SupplierBankAccount_SupplierInfo_BankAccountID",
                        column: x => x.BankAccountID,
                        principalSchema: "CBSAP",
                        principalTable: "SupplierInfo",
                        principalColumn: "SupplierInfoID",
                        onDelete: ReferentialAction.Restrict);
                });
        }
    }
}
