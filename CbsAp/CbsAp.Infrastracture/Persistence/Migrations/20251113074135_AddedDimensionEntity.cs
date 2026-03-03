using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedDimensionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dimension",
                schema: "CBSAP",
                columns: table => new
                {
                    DimensionID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Dimension = table.Column<string>(type: "NVARCHAR(15)", maxLength: 15, nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    FreeField1 = table.Column<string>(type: "NVARCHAR(90)", maxLength: 90, nullable: true),
                    FreeField2 = table.Column<string>(type: "NVARCHAR(90)", maxLength: 90, nullable: true),
                    FreeField3 = table.Column<string>(type: "NVARCHAR(90)", maxLength: 90, nullable: true),
                    EntityProfileID = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastUpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dimension", x => x.DimensionID);
                    table.ForeignKey(
                        name: "FK_Dimension_EntityProfile_EntityProfileID",
                        column: x => x.EntityProfileID,
                        principalSchema: "CBSAP",
                        principalTable: "EntityProfile",
                        principalColumn: "EntityProfileID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dimension_EntityProfileID",
                schema: "CBSAP",
                table: "Dimension",
                column: "EntityProfileID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dimension",
                schema: "CBSAP");
        }
    }
}
