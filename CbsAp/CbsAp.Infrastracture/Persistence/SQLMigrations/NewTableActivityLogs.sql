BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260316000731_NewTableActivityLog'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260316000731_NewTableActivityLog', N'9.0.4');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260316002957_NewTableActivityLogs'
)
BEGIN
    CREATE TABLE [CBSAP].[ActivityLog] (
        [ActivityID] int NOT NULL IDENTITY,
        [InvoiceID] int NOT NULL,
        [Activity] nvarchar(max) NULL,
        [ActionBy] nvarchar(max) NULL,
        [Module] nvarchar(max) NULL,
        [OldValue] nvarchar(max) NULL,
        [NewValue] nvarchar(max) NULL,
        [TableName] nvarchar(max) NULL,
        [ColumnName] nvarchar(max) NULL,
        [metaDataOld] nvarchar(max) NULL,
        [metaDataNew] nvarchar(max) NULL,
        [MetaData] nvarchar(max) NULL,
        [ActivityDate] datetime2 NULL,
        [CreatedBy] nvarchar(255) NULL,
        [CreatedDate] datetimeoffset NULL,
        [LastUpdatedBy] nvarchar(255) NULL,
        [LastUpdatedDate] datetimeoffset NULL,
        CONSTRAINT [PK_ActivityLog] PRIMARY KEY ([ActivityID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260316002957_NewTableActivityLogs'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260316002957_NewTableActivityLogs', N'9.0.4');
END;

COMMIT;
GO

