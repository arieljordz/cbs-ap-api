BEGIN TRANSACTION;
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260616072750_AddTriggerToPoandPoLineTable', N'9.0.4');

ALTER TABLE [CBSAP].[PurchaseOrder] ADD [MatchStatus] int NOT NULL DEFAULT 0;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260617080744_AddMatchStatusColumnToPurchaseOrder', N'9.0.4');

COMMIT;
GO

