using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbsAp.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNewTableAdvanceSearch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"/****** Object:  Table [CBSAP].[AdvanceSearch]    Script Date: 15/05/2026 5:51:34 PM ******/
                DROP TABLE IF EXISTS [CBSAP].[AdvanceSearch]
                GO
                /****** Object:  Table [CBSAP].[AdvanceSearch]    Script Date: 15/05/2026 5:51:34 PM ******/
                SET ANSI_NULLS ON
                GO
                SET QUOTED_IDENTIFIER ON
                GO
                CREATE TABLE [CBSAP].[AdvanceSearch](
                	[AdvanceSearchId] [bigint] IDENTITY(1,1) NOT NULL,
                	[UserId] [nvarchar](150) NOT NULL,
                	[JsonFilter] [nvarchar](max) NULL,
                	[FormName] [nvarchar](150) NULL,
                	[CreatedBy] [nvarchar](255) NULL,
                	[CreatedDate] [datetimeoffset](7) NULL,
                	[LastUpdatedBy] [nvarchar](255) NULL,
                	[LastUpdatedDate] [datetimeoffset](7) NULL,
                 CONSTRAINT [PK_AdvanceSearch] PRIMARY KEY CLUSTERED 
                (
                	[AdvanceSearchId] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
                GO
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
