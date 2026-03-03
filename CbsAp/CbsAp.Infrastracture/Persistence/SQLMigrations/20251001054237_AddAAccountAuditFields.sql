BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001054237_AddAAccountAuditFields'
)
BEGIN
    ALTER TABLE [CBSAP].[Account] ADD [CreatedBy] nvarchar(255) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001054237_AddAAccountAuditFields'
)
BEGIN
    ALTER TABLE [CBSAP].[Account] ADD [CreatedDate] datetimeoffset NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001054237_AddAAccountAuditFields'
)
BEGIN
    ALTER TABLE [CBSAP].[Account] ADD [LastUpdatedBy] nvarchar(255) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001054237_AddAAccountAuditFields'
)
BEGIN
    ALTER TABLE [CBSAP].[Account] ADD [LastUpdatedDate] datetimeoffset NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001054237_AddAAccountAuditFields'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251001054237_AddAAccountAuditFields', N'9.0.4');
END;

COMMIT;
GO

