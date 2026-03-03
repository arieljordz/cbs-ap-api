BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251212025651_AddLockedByAndLockedDateInInvoice'
)
BEGIN
    ALTER TABLE [CBSAP].[Invoice] ADD [LockedBy] nvarchar(100) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251212025651_AddLockedByAndLockedDateInInvoice'
)
BEGIN
    ALTER TABLE [CBSAP].[Invoice] ADD [LockedDate] datetimeoffset NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251212025651_AddLockedByAndLockedDateInInvoice'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251212025651_AddLockedByAndLockedDateInInvoice', N'9.0.4');
END;

COMMIT;
GO

