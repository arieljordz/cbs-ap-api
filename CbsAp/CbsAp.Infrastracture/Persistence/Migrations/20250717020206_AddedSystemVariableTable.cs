using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedSystemVariableTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SystemVariable",
                schema: "CBSAP",
                columns: table => new
                {
                    SystemVariableID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: false),
                    Value = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemVariable", x => x.SystemVariableID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemVariable",
                schema: "CBSAP");
        }
    }
}
