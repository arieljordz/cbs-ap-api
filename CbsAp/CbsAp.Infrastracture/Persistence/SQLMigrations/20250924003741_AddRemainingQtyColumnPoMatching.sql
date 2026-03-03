BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250924003741_AddRemainingQtyColumnPoMatching'
)
BEGIN
    ALTER TABLE [CBSAP].[PurchaseOrderMatchTracking] ADD [RemainingQty] decimal(18,4) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250924003741_AddRemainingQtyColumnPoMatching'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250924003741_AddRemainingQtyColumnPoMatching', N'9.0.4');
END;

COMMIT;
GO

