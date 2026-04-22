BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260406021725_202606041207_AddDueDateCalculation'
)
BEGIN
    ALTER TABLE [CBSAP].[EntityProfile] ADD [InvDueDateCalculation] int NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260406021725_202606041207_AddDueDateCalculation'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260406021725_202606041207_AddDueDateCalculation', N'9.0.4');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410123432_202606041209_AddAutomaticGoodsDelivered'
)
BEGIN
    ALTER TABLE [CBSAP].[EntityProfile] ADD [AutomaticGoodsDelivered] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260410123432_202606041209_AddAutomaticGoodsDelivered'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260410123432_202606041209_AddAutomaticGoodsDelivered', N'9.0.4');
END;

COMMIT;
GO

