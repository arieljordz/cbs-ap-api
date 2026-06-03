BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513221602_fkchangessupplierbankaccount'
)
BEGIN
    ALTER TABLE [CBSAP].[SupplierBankAccount] DROP CONSTRAINT [FK_SupplierBankAccount_SupplierInfo_SupplierBankAccountID];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513221602_fkchangessupplierbankaccount'
)
BEGIN
    CREATE INDEX [IX_SupplierBankAccount_SupplierInfoID] ON [CBSAP].[SupplierBankAccount] ([SupplierInfoID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513221602_fkchangessupplierbankaccount'
)
BEGIN
    ALTER TABLE [CBSAP].[SupplierBankAccount] ADD CONSTRAINT [FK_SupplierBankAccount_SupplierInfo_SupplierInfoID] FOREIGN KEY ([SupplierInfoID]) REFERENCES [CBSAP].[SupplierInfo] ([SupplierInfoID]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513221602_fkchangessupplierbankaccount'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260513221602_fkchangessupplierbankaccount', N'9.0.4');
END;

COMMIT;
GO

