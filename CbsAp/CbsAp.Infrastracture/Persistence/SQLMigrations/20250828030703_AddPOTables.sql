BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250828030703_AddPOTables'
)
BEGIN
    DECLARE @var sysname;
    SELECT @var = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[Account]') AND [c].[name] = N'AccountName');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[Account] DROP CONSTRAINT [' + @var + '];');
    ALTER TABLE [CBSAP].[Account] ALTER COLUMN [AccountName] nvarchar(50) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250828030703_AddPOTables'
)
BEGIN
    ALTER TABLE [CBSAP].[Account] ADD [Dimension1] nvarchar(15) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250828030703_AddPOTables'
)
BEGIN
    ALTER TABLE [CBSAP].[Account] ADD [Dimension2] nvarchar(15) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250828030703_AddPOTables'
)
BEGIN
    ALTER TABLE [CBSAP].[Account] ADD [Dimension3] nvarchar(15) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250828030703_AddPOTables'
)
BEGIN
    ALTER TABLE [CBSAP].[Account] ADD [Dimension4] nvarchar(15) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250828030703_AddPOTables'
)
BEGIN
    ALTER TABLE [CBSAP].[Account] ADD [Dimension5] nvarchar(15) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250828030703_AddPOTables'
)
BEGIN
    ALTER TABLE [CBSAP].[Account] ADD [Dimension6] nvarchar(15) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250828030703_AddPOTables'
)
BEGIN
    ALTER TABLE [CBSAP].[Account] ADD [Dimension7] nvarchar(15) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250828030703_AddPOTables'
)
BEGIN
    ALTER TABLE [CBSAP].[Account] ADD [Dimension8] nvarchar(15) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250828030703_AddPOTables'
)
BEGIN
    ALTER TABLE [CBSAP].[Account] ADD [EntityProfileID] bigint NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250828030703_AddPOTables'
)
BEGIN
    ALTER TABLE [CBSAP].[Account] ADD [FreeField1] nvarchar(15) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250828030703_AddPOTables'
)
BEGIN
    ALTER TABLE [CBSAP].[Account] ADD [FreeField2] nvarchar(15) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250828030703_AddPOTables'
)
BEGIN
    ALTER TABLE [CBSAP].[Account] ADD [FreeField3] nvarchar(15) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250828030703_AddPOTables'
)
BEGIN
    ALTER TABLE [CBSAP].[Account] ADD [IsTaxCodeMandatory] bit NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250828030703_AddPOTables'
)
BEGIN
    ALTER TABLE [CBSAP].[Account] ADD [TaxCodeID] bigint NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250828030703_AddPOTables'
)
BEGIN
    ALTER TABLE [CBSAP].[Account] ADD [TaxCodeName] nvarchar(100) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250828030703_AddPOTables'
)
BEGIN
    CREATE TABLE [CBSAP].[PurchaseOrder] (
        [PurchaseOrderID] bigint NOT NULL IDENTITY,
        [PoNo] nvarchar(50) NULL,
        [PurchaseDate] datetime2 NOT NULL,
        [EntityProfileID] bigint NULL,
        [SupplierInfoID] bigint NULL,
        [SupplierID] nvarchar(40) NULL,
        [Currency] nvarchar(3) NULL,
        [NetAmount] decimal(18,2) NULL,
        [TaxAmount] decimal(18,2) NULL,
        [TotalAmount] decimal(18,2) NULL,
        [IsActive] bit NOT NULL,
        [MatchReference1] nvarchar(90) NULL,
        [MatchReference2] nvarchar(90) NULL,
        [Note] nvarchar(1000) NULL,
        [FreeField1] nvarchar(200) NULL,
        [FreeField2] nvarchar(200) NULL,
        [FreeField3] nvarchar(200) NULL,
        [CreatedBy] nvarchar(255) NULL,
        [CreatedDate] datetimeoffset NULL,
        [LastUpdatedBy] nvarchar(255) NULL,
        [LastUpdatedDate] datetimeoffset NULL,
        CONSTRAINT [PK_PurchaseOrder] PRIMARY KEY ([PurchaseOrderID]),
        CONSTRAINT [FK_PurchaseOrder_EntityProfile_EntityProfileID] FOREIGN KEY ([EntityProfileID]) REFERENCES [CBSAP].[EntityProfile] ([EntityProfileID]) ON DELETE NO ACTION,
        CONSTRAINT [FK_PurchaseOrder_SupplierInfo_SupplierInfoID] FOREIGN KEY ([SupplierInfoID]) REFERENCES [CBSAP].[SupplierInfo] ([SupplierInfoID]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250828030703_AddPOTables'
)
BEGIN
    CREATE TABLE [CBSAP].[PurchaseOrderLine] (
        [PurchaseOrderLineID] bigint NOT NULL IDENTITY,
        [PurchaseOrderID] bigint NOT NULL,
        [TaxCodeID] bigint NULL,
        [AccountID] bigint NULL,
        [LineNo] bigint NULL,
        [Description] nvarchar(90) NULL,
        [Qty] decimal(18,4) NOT NULL,
        [Unit] nvarchar(12) NULL,
        [Price] decimal(18,4) NULL,
        [InvoicedPrice] decimal(18,4) NULL,
        [Amount] decimal(18,4) NULL,
        [NetAmount] decimal(18,4) NULL,
        [TaxAmount] decimal(18,4) NULL,
        [Item] nvarchar(90) NULL,
        [IsActive] bit NULL,
        [FreeField1] nvarchar(max) NULL,
        [FreeField2] nvarchar(max) NULL,
        [FreeField3] nvarchar(max) NULL,
        [SpareAmount1] decimal(18,2) NULL,
        [SpareAmount2] decimal(18,2) NULL,
        [SpareAmount3] decimal(18,2) NULL,
        [FullyInvoiced] bit NULL,
        [CreatedBy] nvarchar(255) NULL,
        [CreatedDate] datetimeoffset NULL,
        [LastUpdatedBy] nvarchar(255) NULL,
        [LastUpdatedDate] datetimeoffset NULL,
        CONSTRAINT [PK_PurchaseOrderLine] PRIMARY KEY ([PurchaseOrderLineID]),
        CONSTRAINT [FK_PurchaseOrderLine_Account_AccountID] FOREIGN KEY ([AccountID]) REFERENCES [CBSAP].[Account] ([AccountID]) ON DELETE NO ACTION,
        CONSTRAINT [FK_PurchaseOrderLine_PurchaseOrder_PurchaseOrderID] FOREIGN KEY ([PurchaseOrderID]) REFERENCES [CBSAP].[PurchaseOrder] ([PurchaseOrderID]) ON DELETE CASCADE,
        CONSTRAINT [FK_PurchaseOrderLine_TaxCode_TaxCodeID] FOREIGN KEY ([TaxCodeID]) REFERENCES [CBSAP].[TaxCode] ([TaxCodeID]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250828030703_AddPOTables'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Account_EntityProfileID] ON [CBSAP].[Account] ([EntityProfileID]) WHERE [EntityProfileID] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250828030703_AddPOTables'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Account_TaxCodeID] ON [CBSAP].[Account] ([TaxCodeID]) WHERE [TaxCodeID] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250828030703_AddPOTables'
)
BEGIN
    CREATE INDEX [IX_PurchaseOrder_EntityProfileID] ON [CBSAP].[PurchaseOrder] ([EntityProfileID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250828030703_AddPOTables'
)
BEGIN
    CREATE INDEX [IX_PurchaseOrder_SupplierInfoID] ON [CBSAP].[PurchaseOrder] ([SupplierInfoID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250828030703_AddPOTables'
)
BEGIN
    CREATE INDEX [IX_PurchaseOrderLine_AccountID] ON [CBSAP].[PurchaseOrderLine] ([AccountID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250828030703_AddPOTables'
)
BEGIN
    CREATE INDEX [IX_PurchaseOrderLine_PurchaseOrderID] ON [CBSAP].[PurchaseOrderLine] ([PurchaseOrderID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250828030703_AddPOTables'
)
BEGIN
    CREATE INDEX [IX_PurchaseOrderLine_TaxCodeID] ON [CBSAP].[PurchaseOrderLine] ([TaxCodeID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250828030703_AddPOTables'
)
BEGIN
    ALTER TABLE [CBSAP].[Account] ADD CONSTRAINT [FK_Account_EntityProfile_EntityProfileID] FOREIGN KEY ([EntityProfileID]) REFERENCES [CBSAP].[EntityProfile] ([EntityProfileID]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250828030703_AddPOTables'
)
BEGIN
    ALTER TABLE [CBSAP].[Account] ADD CONSTRAINT [FK_Account_TaxCode_TaxCodeID] FOREIGN KEY ([TaxCodeID]) REFERENCES [CBSAP].[TaxCode] ([TaxCodeID]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250828030703_AddPOTables'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250828030703_AddPOTables', N'9.0.4');
END;

COMMIT;
GO

