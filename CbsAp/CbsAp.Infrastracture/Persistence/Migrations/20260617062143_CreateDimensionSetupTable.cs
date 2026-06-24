using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreateDimensionSetupTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DimensionSetup",
                schema: "CBSAP",
                columns: table => new
                {
                    DimensionSetupId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DimensionSetupName = table.Column<string>(type: "NVARCHAR(150)", maxLength: 150, nullable: true),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false),
                    DimensionNameId = table.Column<long>(type: "bigint", nullable: true),
                    DimensionValueId = table.Column<long>(type: "bigint", nullable: true),
                    Required = table.Column<bool>(type: "bit", nullable: true),
                    Show = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastUpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimensionSetup", x => x.DimensionSetupId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DimensionSetup",
                schema: "CBSAP");
        }
    }
}
