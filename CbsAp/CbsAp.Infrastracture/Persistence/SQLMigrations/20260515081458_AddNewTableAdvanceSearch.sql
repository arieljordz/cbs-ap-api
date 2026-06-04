BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260515081458_AddNewTableAdvanceSearch'
)
BEGIN
    /****** Object:  Table [CBSAP].[AdvanceSearch]    Script Date: 15/05/2026 5:51:34 PM ******/
                    DROP TABLE IF EXISTS [CBSAP].[AdvanceSearch]
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260515081458_AddNewTableAdvanceSearch'
)
BEGIN
                    /****** Object:  Table [CBSAP].[AdvanceSearch]    Script Date: 15/05/2026 5:51:34 PM ******/
                    SET ANSI_NULLS ON
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260515081458_AddNewTableAdvanceSearch'
)
BEGIN
                    SET QUOTED_IDENTIFIER ON
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260515081458_AddNewTableAdvanceSearch'
)
BEGIN
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260515081458_AddNewTableAdvanceSearch'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260515081458_AddNewTableAdvanceSearch', N'9.0.4');
END;

COMMIT;
GO

