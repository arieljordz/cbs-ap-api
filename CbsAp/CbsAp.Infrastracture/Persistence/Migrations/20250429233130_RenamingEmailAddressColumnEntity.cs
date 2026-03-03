using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenamingEmailAddressColumnEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.Sql(
       "EXEC sp_rename 'CBSAP.EntityProfile.SytemOwnerEmailAddress', 'EmailAddress', 'COLUMN';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.Sql(
      "EXEC sp_rename 'CBSAP.EntityProfile.EmailAddress', 'SytemOwnerEmailAddress', 'COLUMN';");
        }
    }
}
