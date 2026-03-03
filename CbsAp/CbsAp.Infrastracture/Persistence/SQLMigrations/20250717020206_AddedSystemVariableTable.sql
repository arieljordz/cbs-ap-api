BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250717020206_AddedSystemVariableTable'
)
BEGIN
    CREATE TABLE [CBSAP].[SystemVariable] (
        [SystemVariableID] bigint NOT NULL IDENTITY,
        [Name] NVARCHAR(500) NOT NULL,
        [Value] NVARCHAR(500) NULL,
        [Description] NVARCHAR(500) NOT NULL,
        CONSTRAINT [PK_SystemVariable] PRIMARY KEY ([SystemVariableID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250717020206_AddedSystemVariableTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250717020206_AddedSystemVariableTable', N'9.0.4');
END;

COMMIT;
GO

