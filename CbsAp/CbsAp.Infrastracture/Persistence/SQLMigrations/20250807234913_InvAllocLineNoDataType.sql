BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250807234913_InvAllocLineNoDataType'
)
BEGIN
    DECLARE @var sysname;
    SELECT @var = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[InvAllocLine]') AND [c].[name] = N'LineNo');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[InvAllocLine] DROP CONSTRAINT [' + @var + '];');
    ALTER TABLE [CBSAP].[InvAllocLine] ALTER COLUMN [LineNo] bigint NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250807234913_InvAllocLineNoDataType'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250807234913_InvAllocLineNoDataType', N'9.0.4');
END;

COMMIT;
GO

