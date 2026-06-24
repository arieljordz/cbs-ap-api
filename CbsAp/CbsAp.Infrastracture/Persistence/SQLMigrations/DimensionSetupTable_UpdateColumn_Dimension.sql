BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260622022412_DimensionSetupTable_UpdateColumn_Dimension'
)
BEGIN
    DECLARE @var sysname;
    SELECT @var = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[DimensionSetup]') AND [c].[name] = N'DimensionNameId');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[DimensionSetup] DROP CONSTRAINT [' + @var + '];');
    ALTER TABLE [CBSAP].[DimensionSetup] DROP COLUMN [DimensionNameId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260622022412_DimensionSetupTable_UpdateColumn_Dimension'
)
BEGIN
    ALTER TABLE [CBSAP].[DimensionSetup] ADD [DimensionName] NVARCHAR(150) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260622022412_DimensionSetupTable_UpdateColumn_Dimension'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260622022412_DimensionSetupTable_UpdateColumn_Dimension', N'9.0.4');
END;

COMMIT;
GO

