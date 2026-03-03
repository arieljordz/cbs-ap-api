BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251002005312_UpdateAccountTableForeignKey'
)
BEGIN
    DROP INDEX [IX_Account_EntityProfileID] ON [CBSAP].[Account];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251002005312_UpdateAccountTableForeignKey'
)
BEGIN
    DROP INDEX [IX_Account_TaxCodeID] ON [CBSAP].[Account];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251002005312_UpdateAccountTableForeignKey'
)
BEGIN
    CREATE INDEX [IX_Account_EntityProfileID] ON [CBSAP].[Account] ([EntityProfileID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251002005312_UpdateAccountTableForeignKey'
)
BEGIN
    CREATE INDEX [IX_Account_TaxCodeID] ON [CBSAP].[Account] ([TaxCodeID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251002005312_UpdateAccountTableForeignKey'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251002005312_UpdateAccountTableForeignKey', N'9.0.4');
END;

COMMIT;
GO

