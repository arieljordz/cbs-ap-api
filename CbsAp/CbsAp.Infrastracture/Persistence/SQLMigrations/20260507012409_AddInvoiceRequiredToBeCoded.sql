BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507012409_AddInvoiceRequiredToBeCoded'
)
BEGIN
    ALTER TABLE [CBSAP].[EntityProfile] ADD [InvoiceRequiredToBeCoded] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260507012409_AddInvoiceRequiredToBeCoded'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260507012409_AddInvoiceRequiredToBeCoded', N'9.0.4');
END;

COMMIT;
GO

