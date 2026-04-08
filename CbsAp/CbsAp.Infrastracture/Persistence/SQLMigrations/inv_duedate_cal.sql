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

COMMIT;
GO

