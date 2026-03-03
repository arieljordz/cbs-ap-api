BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250917030938_AddPurchaseOrderMatchTrackingTable'
)
BEGIN
    EXEC sp_rename N'[CBSAP].[PurchaseOrder].[SupplierID]', N'SupplierTaxID', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250917030938_AddPurchaseOrderMatchTrackingTable'
)
BEGIN
    DECLARE @var sysname;
    SELECT @var = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[PurchaseOrderLine]') AND [c].[name] = N'TaxAmount');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[PurchaseOrderLine] DROP CONSTRAINT [' + @var + '];');
    ALTER TABLE [CBSAP].[PurchaseOrderLine] ALTER COLUMN [TaxAmount] decimal(18,2) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250917030938_AddPurchaseOrderMatchTrackingTable'
)
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[PurchaseOrderLine]') AND [c].[name] = N'Price');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[PurchaseOrderLine] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [CBSAP].[PurchaseOrderLine] ALTER COLUMN [Price] decimal(18,2) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250917030938_AddPurchaseOrderMatchTrackingTable'
)
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[PurchaseOrderLine]') AND [c].[name] = N'NetAmount');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[PurchaseOrderLine] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [CBSAP].[PurchaseOrderLine] ALTER COLUMN [NetAmount] decimal(18,2) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250917030938_AddPurchaseOrderMatchTrackingTable'
)
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[PurchaseOrderLine]') AND [c].[name] = N'InvoicedPrice');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[PurchaseOrderLine] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [CBSAP].[PurchaseOrderLine] ALTER COLUMN [InvoicedPrice] decimal(18,2) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250917030938_AddPurchaseOrderMatchTrackingTable'
)
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[PurchaseOrderLine]') AND [c].[name] = N'Amount');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[PurchaseOrderLine] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [CBSAP].[PurchaseOrderLine] ALTER COLUMN [Amount] decimal(18,2) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250917030938_AddPurchaseOrderMatchTrackingTable'
)
BEGIN
    CREATE TABLE [CBSAP].[PurchaseOrderMatchTracking] (
        [PurchaseOrderMatchTrackingID] bigint NOT NULL IDENTITY,
        [PurchaseOrderLineID] bigint NOT NULL,
        [PurchaseOrderID] bigint NOT NULL,
        [InvoiceID] bigint NULL,
        [InvAllocLineID] bigint NULL,
        [Account] nvarchar(40) NULL,
        [Qty] decimal(18,4) NOT NULL,
        [Amount] decimal(18,2) NULL,
        [NetAmount] decimal(18,2) NULL,
        [TaxAmount] decimal(18,2) NULL,
        [MatchingDate] datetimeoffset NOT NULL,
        [IsSystemMatching] bit NOT NULL,
        [MatchingStatus] int NULL,
        [CreatedBy] nvarchar(255) NULL,
        [CreatedDate] datetimeoffset NULL,
        [LastUpdatedBy] nvarchar(255) NULL,
        [LastUpdatedDate] datetimeoffset NULL,
        CONSTRAINT [PK_PurchaseOrderMatchTracking] PRIMARY KEY ([PurchaseOrderMatchTrackingID]),
        CONSTRAINT [FK_PurchaseOrderMatchTracking_InvAllocLine_InvAllocLineID] FOREIGN KEY ([InvAllocLineID]) REFERENCES [CBSAP].[InvAllocLine] ([InvAllocLineID]) ON DELETE NO ACTION,
        CONSTRAINT [FK_PurchaseOrderMatchTracking_Invoice_InvoiceID] FOREIGN KEY ([InvoiceID]) REFERENCES [CBSAP].[Invoice] ([InvoiceID]) ON DELETE NO ACTION,
        CONSTRAINT [FK_PurchaseOrderMatchTracking_PurchaseOrderLine_PurchaseOrderLineID] FOREIGN KEY ([PurchaseOrderLineID]) REFERENCES [CBSAP].[PurchaseOrderLine] ([PurchaseOrderLineID]) ON DELETE NO ACTION,
        CONSTRAINT [FK_PurchaseOrderMatchTracking_PurchaseOrder_PurchaseOrderID] FOREIGN KEY ([PurchaseOrderID]) REFERENCES [CBSAP].[PurchaseOrder] ([PurchaseOrderID]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250917030938_AddPurchaseOrderMatchTrackingTable'
)
BEGIN
    CREATE INDEX [IX_PurchaseOrderMatchTracking_InvAllocLineID] ON [CBSAP].[PurchaseOrderMatchTracking] ([InvAllocLineID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250917030938_AddPurchaseOrderMatchTrackingTable'
)
BEGIN
    CREATE INDEX [IX_PurchaseOrderMatchTracking_InvoiceID] ON [CBSAP].[PurchaseOrderMatchTracking] ([InvoiceID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250917030938_AddPurchaseOrderMatchTrackingTable'
)
BEGIN
    CREATE INDEX [IX_PurchaseOrderMatchTracking_PurchaseOrderID] ON [CBSAP].[PurchaseOrderMatchTracking] ([PurchaseOrderID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250917030938_AddPurchaseOrderMatchTrackingTable'
)
BEGIN
    CREATE INDEX [IX_PurchaseOrderMatchTracking_PurchaseOrderLineID] ON [CBSAP].[PurchaseOrderMatchTracking] ([PurchaseOrderLineID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250917030938_AddPurchaseOrderMatchTrackingTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250917030938_AddPurchaseOrderMatchTrackingTable', N'9.0.4');
END;

COMMIT;
GO

