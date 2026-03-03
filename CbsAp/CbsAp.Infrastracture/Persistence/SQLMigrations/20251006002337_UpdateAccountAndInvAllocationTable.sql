BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251006002337_UpdateAccountAndInvAllocationTable'
)
BEGIN
    DECLARE @var sysname;
    SELECT @var = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[InvAllocLine]') AND [c].[name] = N'Account');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[InvAllocLine] DROP CONSTRAINT [' + @var + '];');
    ALTER TABLE [CBSAP].[InvAllocLine] DROP COLUMN [Account];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251006002337_UpdateAccountAndInvAllocationTable'
)
BEGIN
    ALTER TABLE [CBSAP].[InvAllocLine] ADD [AccountID] bigint NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251006002337_UpdateAccountAndInvAllocationTable'
)
BEGIN
    CREATE INDEX [IX_InvAllocLine_AccountID] ON [CBSAP].[InvAllocLine] ([AccountID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251006002337_UpdateAccountAndInvAllocationTable'
)
BEGIN
    ALTER TABLE [CBSAP].[InvAllocLine] ADD CONSTRAINT [FK_InvAllocLine_Account_AccountID] FOREIGN KEY ([AccountID]) REFERENCES [CBSAP].[Account] ([AccountID]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251006002337_UpdateAccountAndInvAllocationTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251006002337_UpdateAccountAndInvAllocationTable', N'9.0.4');
END;

COMMIT;
GO

