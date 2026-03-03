BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250521050743_UpdateTaxRateDataType'
)
BEGIN
    DECLARE @var sysname;
    SELECT @var = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[TaxCode]') AND [c].[name] = N'TaxRate');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[TaxCode] DROP CONSTRAINT [' + @var + '];');
    ALTER TABLE [CBSAP].[TaxCode] ALTER COLUMN [TaxRate] decimal(18,2) NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250521050743_UpdateTaxRateDataType'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250521050743_UpdateTaxRateDataType', N'9.0.4');
END;

COMMIT;
GO

