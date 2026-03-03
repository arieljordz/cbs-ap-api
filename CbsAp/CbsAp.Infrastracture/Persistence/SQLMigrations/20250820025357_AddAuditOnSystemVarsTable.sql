BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250820025357_AddAuditOnSystemVarsTable'
)
BEGIN
    ALTER TABLE [CBSAP].[SystemVariable] ADD [CreatedBy] nvarchar(255) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250820025357_AddAuditOnSystemVarsTable'
)
BEGIN
    ALTER TABLE [CBSAP].[SystemVariable] ADD [CreatedDate] datetimeoffset NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250820025357_AddAuditOnSystemVarsTable'
)
BEGIN
    ALTER TABLE [CBSAP].[SystemVariable] ADD [LastUpdatedBy] nvarchar(255) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250820025357_AddAuditOnSystemVarsTable'
)
BEGIN
    ALTER TABLE [CBSAP].[SystemVariable] ADD [LastUpdatedDate] datetimeoffset NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250820025357_AddAuditOnSystemVarsTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250820025357_AddAuditOnSystemVarsTable', N'9.0.4');
END;

COMMIT;
GO

