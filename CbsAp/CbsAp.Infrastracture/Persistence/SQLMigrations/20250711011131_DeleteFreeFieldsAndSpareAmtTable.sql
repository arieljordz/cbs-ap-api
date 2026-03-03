BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250711011131_DeleteFreeFieldsAndSpareAmtTable'
)
BEGIN
    DROP TABLE [CBSAP].[InvoiceFreeField];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250711011131_DeleteFreeFieldsAndSpareAmtTable'
)
BEGIN
    DROP TABLE [CBSAP].[InvoiceSpareAmount];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250711011131_DeleteFreeFieldsAndSpareAmtTable'
)
BEGIN
    ALTER TABLE [CBSAP].[Invoice] ADD [FreeField1] nvarchar(200) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250711011131_DeleteFreeFieldsAndSpareAmtTable'
)
BEGIN
    ALTER TABLE [CBSAP].[Invoice] ADD [FreeField2] nvarchar(200) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250711011131_DeleteFreeFieldsAndSpareAmtTable'
)
BEGIN
    ALTER TABLE [CBSAP].[Invoice] ADD [FreeField3] nvarchar(200) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250711011131_DeleteFreeFieldsAndSpareAmtTable'
)
BEGIN
    ALTER TABLE [CBSAP].[Invoice] ADD [SpareAmount1] decimal(18,2) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250711011131_DeleteFreeFieldsAndSpareAmtTable'
)
BEGIN
    ALTER TABLE [CBSAP].[Invoice] ADD [SpareAmount2] decimal(18,2) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250711011131_DeleteFreeFieldsAndSpareAmtTable'
)
BEGIN
    ALTER TABLE [CBSAP].[Invoice] ADD [SpareAmount3] decimal(18,2) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250711011131_DeleteFreeFieldsAndSpareAmtTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250711011131_DeleteFreeFieldsAndSpareAmtTable', N'9.0.4');
END;

COMMIT;
GO

