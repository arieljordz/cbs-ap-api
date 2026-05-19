BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260504081750_AddInvoiceNetLessThanPOandInvoiceNetGreaterThanPOColumn'
)
BEGIN
    ALTER TABLE [CBSAP].[EntityProfile] ADD [InvoiceNetGreaterThanPO] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
 
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260504081750_AddInvoiceNetLessThanPOandInvoiceNetGreaterThanPOColumn'
)
BEGIN
    ALTER TABLE [CBSAP].[EntityProfile] ADD [InvoiceNetLessThanPO] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
 
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260504081750_AddInvoiceNetLessThanPOandInvoiceNetGreaterThanPOColumn'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260504081750_AddInvoiceNetLessThanPOandInvoiceNetGreaterThanPOColumn', N'9.0.4');
END;
 
COMMIT;
GO