BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260504025300_20260504124800_Invoice_TaxCodeID_nullable'
)
BEGIN
    DECLARE @var sysname;
    SELECT @var = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[InvoiceArchive]') AND [c].[name] = N'TaxCodeID');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[InvoiceArchive] DROP CONSTRAINT [' + @var + '];');
    ALTER TABLE [CBSAP].[InvoiceArchive] ALTER COLUMN [TaxCodeID] bigint NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260504025300_20260504124800_Invoice_TaxCodeID_nullable'
)
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[Invoice]') AND [c].[name] = N'TaxCodeID');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[Invoice] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [CBSAP].[Invoice] ALTER COLUMN [TaxCodeID] bigint NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260504025300_20260504124800_Invoice_TaxCodeID_nullable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260504025300_20260504124800_Invoice_TaxCodeID_nullable', N'9.0.4');
END;

COMMIT;
GO

