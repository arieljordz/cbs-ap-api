BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512055318_addnewbankaccounttable'
)
BEGIN
    DECLARE @var sysname;
    SELECT @var = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[PurchaseOrder]') AND [c].[name] = N'PoNo');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[PurchaseOrder] DROP CONSTRAINT [' + @var + '];');
    EXEC(N'UPDATE [CBSAP].[PurchaseOrder] SET [PoNo] = N'''' WHERE [PoNo] IS NULL');
    ALTER TABLE [CBSAP].[PurchaseOrder] ALTER COLUMN [PoNo] nvarchar(50) NOT NULL;
    ALTER TABLE [CBSAP].[PurchaseOrder] ADD DEFAULT N'' FOR [PoNo];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512055318_addnewbankaccounttable'
)
BEGIN
    ALTER TABLE [CBSAP].[PurchaseOrder] ADD CONSTRAINT [AK_PurchaseOrder_PoNo] UNIQUE ([PoNo]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512055318_addnewbankaccounttable'
)
BEGIN
    CREATE TABLE [CBSAP].[SupplierBankAccount] (
        [SupplierBankAccountID] bigint NOT NULL IDENTITY,
        [SupplierInfoID] bigint NOT NULL,
        [BankAccountNumber] NVARCHAR(40) NOT NULL,
        [BankName] NVARCHAR(40) NULL,
        [IsActive] bit NOT NULL,
        [CreatedBy] nvarchar(255) NULL,
        [CreatedDate] datetimeoffset NULL,
        [LastUpdatedBy] nvarchar(255) NULL,
        [LastUpdatedDate] datetimeoffset NULL,
        CONSTRAINT [PK_SupplierBankAccount] PRIMARY KEY ([SupplierBankAccountID]),
        CONSTRAINT [FK_SupplierBankAccount_SupplierInfo_SupplierBankAccountID] FOREIGN KEY ([SupplierBankAccountID]) REFERENCES [CBSAP].[SupplierInfo] ([SupplierInfoID]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512055318_addnewbankaccounttable'
)
BEGIN
    CREATE INDEX [IX_GoodsReceiptLine_PurchaseOrderNo] ON [CBSAP].[GoodsReceiptLine] ([PurchaseOrderNo]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512055318_addnewbankaccounttable'
)
BEGIN
    ALTER TABLE [CBSAP].[GoodsReceiptLine] ADD CONSTRAINT [FK_GoodsReceiptLine_PurchaseOrder_PurchaseOrderNo] FOREIGN KEY ([PurchaseOrderNo]) REFERENCES [CBSAP].[PurchaseOrder] ([PoNo]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512055318_addnewbankaccounttable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260512055318_addnewbankaccounttable', N'9.0.4');
END;

COMMIT;
GO

