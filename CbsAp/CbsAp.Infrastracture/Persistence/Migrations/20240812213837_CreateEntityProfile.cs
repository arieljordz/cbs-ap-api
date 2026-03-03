using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreateEntityProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntityProfile",
                schema: "CBSAP",
                columns: table => new
                {
                    EntityProfileID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityName = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: false),
                    EntityCode = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    SytemOwnerEmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxID = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    ERPFinanceSystem = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastUpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityProfile", x => x.EntityProfileID);
                });

            migrationBuilder.CreateTable(
                name: "RoleEntity",
                schema: "CBSAP",
                columns: table => new
                {
                    RoleEntityID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityProfileID = table.Column<long>(type: "bigint", nullable: false),
                    RoleID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleEntity", x => x.RoleEntityID);
                    table.UniqueConstraint("AK_RoleEntity_RoleID_EntityProfileID", x => new { x.RoleID, x.EntityProfileID });
                    table.ForeignKey(
                        name: "FK_RoleEntity_EntityProfile_EntityProfileID",
                        column: x => x.EntityProfileID,
                        principalSchema: "CBSAP",
                        principalTable: "EntityProfile",
                        principalColumn: "EntityProfileID");
                    table.ForeignKey(
                        name: "FK_RoleEntity_Role_RoleID",
                        column: x => x.RoleID,
                        principalSchema: "CBSAP",
                        principalTable: "Role",
                        principalColumn: "RoleID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleEntity_EntityProfileID",
                schema: "CBSAP",
                table: "RoleEntity",
                column: "EntityProfileID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleEntity",
                schema: "CBSAP");

            migrationBuilder.DropTable(
                name: "EntityProfile",
                schema: "CBSAP");
        }
    }
}
