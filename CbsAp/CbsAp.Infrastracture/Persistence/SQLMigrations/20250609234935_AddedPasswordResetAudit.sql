BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250609234935_AddedPasswordResetAudit'
)
BEGIN
    ALTER TABLE [CBSAP].[RoleEntity] DROP CONSTRAINT [FK_RoleEntity_EntityProfile_EntityProfileID];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250609234935_AddedPasswordResetAudit'
)
BEGIN
    ALTER TABLE [CBSAP].[RoleEntity] DROP CONSTRAINT [FK_RoleEntity_Role_RoleID];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250609234935_AddedPasswordResetAudit'
)
BEGIN
    DECLARE @var sysname;
    SELECT @var = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[Role]') AND [c].[name] = N'RoleName');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[Role] DROP CONSTRAINT [' + @var + '];');
    EXEC(N'UPDATE [CBSAP].[Role] SET [RoleName] = N'''' WHERE [RoleName] IS NULL');
    ALTER TABLE [CBSAP].[Role] ALTER COLUMN [RoleName] NVARCHAR(200) NOT NULL;
    ALTER TABLE [CBSAP].[Role] ADD DEFAULT N'' FOR [RoleName];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250609234935_AddedPasswordResetAudit'
)
BEGIN
    CREATE TABLE [CBSAP].[PasswordResetAudits] (
        [PasswordResetAuditID] bigint NOT NULL IDENTITY,
        [UserAccountID] bigint NOT NULL,
        [IsSuccessfulLoginAfterReset] bit NOT NULL,
        [CreatedBy] nvarchar(max) NULL,
        [CreatedDate] datetimeoffset NOT NULL,
        [LastUpdatedBy] nvarchar(max) NULL,
        [LastUpdatedDate] datetimeoffset NULL,
        CONSTRAINT [PK_PasswordResetAudits] PRIMARY KEY ([PasswordResetAuditID]),
        CONSTRAINT [FK_PasswordResetAudits_UserAccount_UserAccountID] FOREIGN KEY ([UserAccountID]) REFERENCES [CBSAP].[UserAccount] ([UserAccountID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250609234935_AddedPasswordResetAudit'
)
BEGIN
    CREATE INDEX [IX_PasswordResetAudits_UserAccountID_CreatedDate] ON [CBSAP].[PasswordResetAudits] ([UserAccountID], [CreatedDate]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250609234935_AddedPasswordResetAudit'
)
BEGIN
    ALTER TABLE [CBSAP].[RoleEntity] ADD CONSTRAINT [FK_RoleEntity_EntityProfile_EntityProfileID] FOREIGN KEY ([EntityProfileID]) REFERENCES [CBSAP].[EntityProfile] ([EntityProfileID]) ON DELETE CASCADE;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250609234935_AddedPasswordResetAudit'
)
BEGIN
    ALTER TABLE [CBSAP].[RoleEntity] ADD CONSTRAINT [FK_RoleEntity_Role_RoleID] FOREIGN KEY ([RoleID]) REFERENCES [CBSAP].[Role] ([RoleID]) ON DELETE CASCADE;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250609234935_AddedPasswordResetAudit'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250609234935_AddedPasswordResetAudit', N'9.0.4');
END;

COMMIT;
GO

