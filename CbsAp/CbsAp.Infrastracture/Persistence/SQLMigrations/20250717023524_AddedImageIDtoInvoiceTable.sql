BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250717023524_AddedImageIDtoInvoiceTable'
)
BEGIN
    ALTER TABLE [CBSAP].[Invoice] ADD [ImageID] char(16) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250717023524_AddedImageIDtoInvoiceTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250717023524_AddedImageIDtoInvoiceTable', N'9.0.4');
END;

COMMIT;
GO

