using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSupplierInfoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupplierInfo",
                schema: "CBSAP",
                columns: table => new
                {
                    SupplierInfoID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierID = table.Column<string>(type: "NVARCHAR(40)", maxLength: 40, nullable: false),
                    SupplierTaxID = table.Column<string>(type: "NVARCHAR(40)", maxLength: 40, nullable: true),
                    EntityProfileID = table.Column<long>(type: "bigint", nullable: true),
                    SupplierName = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Telephone = table.Column<string>(type: "NVARCHAR(30)", maxLength: 30, nullable: true),
                    EmailAddress = table.Column<string>(type: "NVARCHAR(90)", maxLength: 90, nullable: true),
                    Contact = table.Column<string>(type: "NVARCHAR(90)", maxLength: 90, nullable: true),
                    AddressLine1 = table.Column<string>(type: "NVARCHAR(90)", maxLength: 90, nullable: true),
                    AddressLine2 = table.Column<string>(type: "NVARCHAR(90)", maxLength: 90, nullable: true),
                    AddressLine3 = table.Column<string>(type: "NVARCHAR(90)", maxLength: 90, nullable: true),
                    AddressLine4 = table.Column<string>(type: "NVARCHAR(90)", maxLength: 90, nullable: true),
                    AddressLine5 = table.Column<string>(type: "NVARCHAR(90)", maxLength: 90, nullable: true),
                    AddressLine6 = table.Column<string>(type: "NVARCHAR(90)", maxLength: 90, nullable: true),
                    AccountID = table.Column<long>(type: "bigint", nullable: true),
                    TaxCodeID = table.Column<long>(type: "bigint", nullable: true),
                    Currency = table.Column<string>(type: "NVARCHAR(3)", maxLength: 3, nullable: true),
                    InvRoutingFlowID = table.Column<long>(type: "bigint", nullable: true),
                    FreeField1 = table.Column<string>(type: "NVARCHAR(90)", maxLength: 90, nullable: true),
                    FreeField2 = table.Column<string>(type: "nvarchar(90)", nullable: true),
                    FreeField3 = table.Column<string>(type: "NVARCHAR(90)", maxLength: 90, nullable: true),
                    Notes = table.Column<string>(type: "NVARCHAR(400)", maxLength: 400, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastUpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierInfo", x => x.SupplierInfoID);
                    table.ForeignKey(
                        name: "FK_SupplierInfo_Account_AccountID",
                        column: x => x.AccountID,
                        principalSchema: "CBSAP",
                        principalTable: "Account",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupplierInfo_EntityProfile_EntityProfileID",
                        column: x => x.EntityProfileID,
                        principalSchema: "CBSAP",
                        principalTable: "EntityProfile",
                        principalColumn: "EntityProfileID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupplierInfo_InvRoutingFlow_InvRoutingFlowID",
                        column: x => x.InvRoutingFlowID,
                        principalSchema: "CBSAP",
                        principalTable: "InvRoutingFlow",
                        principalColumn: "InvRoutingFlowID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupplierInfo_TaxCode_TaxCodeID",
                        column: x => x.TaxCodeID,
                        principalSchema: "CBSAP",
                        principalTable: "TaxCode",
                        principalColumn: "TaxCodeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupplierInfo_AccountID",
                schema: "CBSAP",
                table: "SupplierInfo",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierInfo_EntityProfileID",
                schema: "CBSAP",
                table: "SupplierInfo",
                column: "EntityProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierInfo_InvRoutingFlowID",
                schema: "CBSAP",
                table: "SupplierInfo",
                column: "InvRoutingFlowID");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierInfo_TaxCodeID",
                schema: "CBSAP",
                table: "SupplierInfo",
                column: "TaxCodeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupplierInfo",
                schema: "CBSAP");
        }
    }
}
