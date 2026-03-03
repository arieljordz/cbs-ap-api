BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219040510_AddSupplierNoInPurchaseOrder'
)
BEGIN
    ALTER TABLE [CBSAP].[PurchaseOrder] ADD [SupplierNo] nvarchar(40) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219040510_AddSupplierNoInPurchaseOrder'
)
BEGIN
    DECLARE @var sysname;
    SELECT @var = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[Invoice]') AND [c].[name] = N'LockedBy');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[Invoice] DROP CONSTRAINT [' + @var + '];');
    ALTER TABLE [CBSAP].[Invoice] ALTER COLUMN [LockedBy] nvarchar(100) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219040510_AddSupplierNoInPurchaseOrder'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251219040510_AddSupplierNoInPurchaseOrder', N'9.0.4');
END;

COMMIT;
GO

