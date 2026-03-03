BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251204073957_AddedGoodsReceipt'
)
BEGIN
    CREATE TABLE [CBSAP].[GoodReceipts] (
        [GoodsReceiptID] bigint NOT NULL IDENTITY,
        [GoodsReceiptNumber] NVARCHAR(50) NOT NULL,
        [DeliveryNote] NVARCHAR(250) NULL,
        [Active] bit NOT NULL,
        [DeliveryDate] DATETIMEOFFSET NULL,
        [EntityProfileID] bigint NOT NULL,
        [SupplierInfoID] bigint NOT NULL,
        [CreatedBy] nvarchar(255) NULL,
        [CreatedDate] datetimeoffset NULL,
        [LastUpdatedBy] nvarchar(255) NULL,
        [LastUpdatedDate] datetimeoffset NULL,
        CONSTRAINT [PK_GoodReceipts] PRIMARY KEY ([GoodsReceiptID]),
        CONSTRAINT [FK_GoodReceipts_EntityProfile_EntityProfileID] FOREIGN KEY ([EntityProfileID]) REFERENCES [CBSAP].[EntityProfile] ([EntityProfileID]) ON DELETE NO ACTION,
        CONSTRAINT [FK_GoodReceipts_SupplierInfo_SupplierInfoID] FOREIGN KEY ([SupplierInfoID]) REFERENCES [CBSAP].[SupplierInfo] ([SupplierInfoID]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251204073957_AddedGoodsReceipt'
)
BEGIN
    CREATE INDEX [IX_GoodReceipts_EntityProfileID] ON [CBSAP].[GoodReceipts] ([EntityProfileID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251204073957_AddedGoodsReceipt'
)
BEGIN
    CREATE INDEX [IX_GoodReceipts_SupplierInfoID] ON [CBSAP].[GoodReceipts] ([SupplierInfoID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251204073957_AddedGoodsReceipt'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251204073957_AddedGoodsReceipt', N'9.0.4');
END;

COMMIT;
GO

