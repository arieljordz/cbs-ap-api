BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250714012444_AddActionColumnInInvoiceActivityLogTable'
)
BEGIN
    ALTER TABLE [CBSAP].[InvoiceActivityLog] ADD [Action] int NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250714012444_AddActionColumnInInvoiceActivityLogTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250714012444_AddActionColumnInInvoiceActivityLogTable', N'9.0.4');
END;

COMMIT;
GO

