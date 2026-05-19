BEGIN TRANSACTION;
ALTER TABLE [CBSAP].[PurchaseOrderMatchTracking] ADD [GoodsReceiptLineID] bigint NOT NULL DEFAULT CAST(0 AS bigint);

ALTER TABLE [CBSAP].[PurchaseOrderLine] ADD [DeliveryStatus] int NOT NULL DEFAULT 0;

ALTER TABLE [CBSAP].[PurchaseOrderLine] ADD [InvoiceStatus] int NOT NULL DEFAULT 0;

ALTER TABLE [CBSAP].[PurchaseOrder] ADD [PurchaseOrderMatchType] int NOT NULL DEFAULT 0;

CREATE TABLE [CBSAP].[GoodsReceiptLine] (
    [GoodsReceiptLineID] bigint NOT NULL IDENTITY,
    [GoodsReceiptID] bigint NOT NULL,
    [LineNo] int NOT NULL,
    [Qty] decimal(18,4) NOT NULL,
    [Amount] decimal(18,4) NOT NULL,
    [SupplierNo] NVARCHAR(50) NULL,
    [PurchaseOrderNo] NVARCHAR(50) NULL,
    [ReceiptNo] NVARCHAR(50) NULL,
    [FreeField1] nvarchar(200) NULL,
    [FreeField2] nvarchar(200) NULL,
    [FreeField3] nvarchar(200) NULL,
    [InvoiceStatus] int NOT NULL,
    [CreatedBy] nvarchar(255) NULL,
    [CreatedDate] datetimeoffset NULL,
    [LastUpdatedBy] nvarchar(255) NULL,
    [LastUpdatedDate] datetimeoffset NULL,
    CONSTRAINT [PK_GoodsReceiptLine] PRIMARY KEY ([GoodsReceiptLineID]),
    CONSTRAINT [FK_GoodsReceiptLine_GoodReceipts_GoodsReceiptID] FOREIGN KEY ([GoodsReceiptID]) REFERENCES [CBSAP].[GoodReceipts] ([GoodsReceiptID]) ON DELETE CASCADE
);

CREATE INDEX [IX_GoodsReceiptLine_GoodsReceiptID] ON [CBSAP].[GoodsReceiptLine] ([GoodsReceiptID]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260416050931_AddGoodsReceiptTable', N'9.0.4');

COMMIT;
GO

