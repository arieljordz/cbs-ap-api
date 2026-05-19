BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260413015551_UpdateUserAndApprovedRoleColumn'
)
BEGIN
    DECLARE @var sysname;
    SELECT @var = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[InvoiceArchive]') AND [c].[name] = N'ApproverRole');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[InvoiceArchive] DROP CONSTRAINT [' + @var + '];');
    ALTER TABLE [CBSAP].[InvoiceArchive] ALTER COLUMN [ApproverRole] bigint NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260413015551_UpdateUserAndApprovedRoleColumn'
)
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[InvoiceArchive]') AND [c].[name] = N'ApprovedUser');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[InvoiceArchive] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [CBSAP].[InvoiceArchive] ALTER COLUMN [ApprovedUser] bigint NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260413015551_UpdateUserAndApprovedRoleColumn'
)
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[Invoice]') AND [c].[name] = N'ApproverRole');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[Invoice] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [CBSAP].[Invoice] ALTER COLUMN [ApproverRole] bigint NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260413015551_UpdateUserAndApprovedRoleColumn'
)
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[Invoice]') AND [c].[name] = N'ApprovedUser');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[Invoice] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [CBSAP].[Invoice] ALTER COLUMN [ApprovedUser] bigint NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260413015551_UpdateUserAndApprovedRoleColumn'
)
BEGIN
    CREATE INDEX [IX_InvoiceArchive_ApprovedUser] ON [CBSAP].[InvoiceArchive] ([ApprovedUser]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260413015551_UpdateUserAndApprovedRoleColumn'
)
BEGIN
    CREATE INDEX [IX_InvoiceArchive_ApproverRole] ON [CBSAP].[InvoiceArchive] ([ApproverRole]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260413015551_UpdateUserAndApprovedRoleColumn'
)
BEGIN
    CREATE INDEX [IX_Invoice_ApprovedUser] ON [CBSAP].[Invoice] ([ApprovedUser]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260413015551_UpdateUserAndApprovedRoleColumn'
)
BEGIN
    CREATE INDEX [IX_Invoice_ApproverRole] ON [CBSAP].[Invoice] ([ApproverRole]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260413015551_UpdateUserAndApprovedRoleColumn'
)
BEGIN
    ALTER TABLE [CBSAP].[Invoice] ADD CONSTRAINT [FK_Invoice_Role_ApprovedUser] FOREIGN KEY ([ApprovedUser]) REFERENCES [CBSAP].[Role] ([RoleID]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260413015551_UpdateUserAndApprovedRoleColumn'
)
BEGIN
    ALTER TABLE [CBSAP].[Invoice] ADD CONSTRAINT [FK_Invoice_Role_ApproverRole] FOREIGN KEY ([ApproverRole]) REFERENCES [CBSAP].[Role] ([RoleID]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260413015551_UpdateUserAndApprovedRoleColumn'
)
BEGIN
    ALTER TABLE [CBSAP].[InvoiceArchive] ADD CONSTRAINT [FK_InvoiceArchive_Role_ApprovedUser] FOREIGN KEY ([ApprovedUser]) REFERENCES [CBSAP].[Role] ([RoleID]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260413015551_UpdateUserAndApprovedRoleColumn'
)
BEGIN
    ALTER TABLE [CBSAP].[InvoiceArchive] ADD CONSTRAINT [FK_InvoiceArchive_Role_ApproverRole] FOREIGN KEY ([ApproverRole]) REFERENCES [CBSAP].[Role] ([RoleID]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260413015551_UpdateUserAndApprovedRoleColumn'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260413015551_UpdateUserAndApprovedRoleColumn', N'9.0.4');
END;

COMMIT;
GO

