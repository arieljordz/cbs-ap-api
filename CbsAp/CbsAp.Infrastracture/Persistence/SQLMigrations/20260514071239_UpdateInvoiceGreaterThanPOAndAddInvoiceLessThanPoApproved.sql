BEGIN TRANSACTION;
EXEC sp_rename N'[CBSAP].[EntityProfile].[InvoiceNetLessThanPO]', N'InvoiceNetLessThanPOException', 'COLUMN';

EXEC sp_rename N'[CBSAP].[EntityProfile].[InvoiceNetGreaterThanPO]', N'InvoiceNetLessThanPOApproved', 'COLUMN';

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[PurchaseOrder]') AND [c].[name] = N'PoNo');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[PurchaseOrder] DROP CONSTRAINT [' + @var + '];');
UPDATE [CBSAP].[PurchaseOrder] SET [PoNo] = N'' WHERE [PoNo] IS NULL;
ALTER TABLE [CBSAP].[PurchaseOrder] ALTER COLUMN [PoNo] nvarchar(50) NOT NULL;
ALTER TABLE [CBSAP].[PurchaseOrder] ADD DEFAULT N'' FOR [PoNo];

ALTER TABLE [CBSAP].[EntityProfile] ADD [InvoiceNetGreaterThanPOApproved] bit NOT NULL DEFAULT CAST(0 AS bit);

--ALTER TABLE [CBSAP].[PurchaseOrder] ADD CONSTRAINT [AK_PurchaseOrder_PoNo] UNIQUE ([PoNo]);

-- CREATE INDEX [IX_GoodsReceiptLine_PurchaseOrderNo] ON [CBSAP].[GoodsReceiptLine] ([PurchaseOrderNo]);

-- ALTER TABLE [CBSAP].[GoodsReceiptLine] ADD CONSTRAINT [FK_GoodsReceiptLine_PurchaseOrder_PurchaseOrderNo] FOREIGN KEY ([PurchaseOrderNo]) REFERENCES [CBSAP].[PurchaseOrder] ([PoNo]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260514071239_UpdateInvoiceGreaterThanPOAndAddInvoiceLessThanPoApproved', N'9.0.4');

COMMIT;
GO

