BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250525231655_AddRoutingFlowTable'
)
BEGIN
    DECLARE @var sysname;
    SELECT @var = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[InvRoutingFlow]') AND [c].[name] = N'InvRoutingFlowName');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[InvRoutingFlow] DROP CONSTRAINT [' + @var + '];');
    ALTER TABLE [CBSAP].[InvRoutingFlow] ALTER COLUMN [InvRoutingFlowName] NVARCHAR(30) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250525231655_AddRoutingFlowTable'
)
BEGIN
    ALTER TABLE [CBSAP].[InvRoutingFlow] ADD [EntityProfileID] bigint NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250525231655_AddRoutingFlowTable'
)
BEGIN
    ALTER TABLE [CBSAP].[InvRoutingFlow] ADD [MatchReference] nvarchar(100) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250525231655_AddRoutingFlowTable'
)
BEGIN
    ALTER TABLE [CBSAP].[InvRoutingFlow] ADD [SupplierInfoID] bigint NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250525231655_AddRoutingFlowTable'
)
BEGIN
    CREATE TABLE [CBSAP].[InvRoutingFlowLevels] (
        [InvRoutingFlowLevelID] bigint NOT NULL IDENTITY,
        [InvRoutingFlowID] bigint NOT NULL,
        [RoleID] bigint NOT NULL,
        [Level] int NOT NULL,
        CONSTRAINT [PK_InvRoutingFlowLevels] PRIMARY KEY ([InvRoutingFlowLevelID]),
        CONSTRAINT [FK_InvRoutingFlowLevels_InvRoutingFlow_InvRoutingFlowID] FOREIGN KEY ([InvRoutingFlowID]) REFERENCES [CBSAP].[InvRoutingFlow] ([InvRoutingFlowID]) ON DELETE CASCADE,
        CONSTRAINT [FK_InvRoutingFlowLevels_Role_RoleID] FOREIGN KEY ([RoleID]) REFERENCES [CBSAP].[Role] ([RoleID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250525231655_AddRoutingFlowTable'
)
BEGIN
    CREATE INDEX [IX_InvRoutingFlow_EntityProfileID] ON [CBSAP].[InvRoutingFlow] ([EntityProfileID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250525231655_AddRoutingFlowTable'
)
BEGIN
    CREATE INDEX [IX_InvRoutingFlow_SupplierInfoID] ON [CBSAP].[InvRoutingFlow] ([SupplierInfoID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250525231655_AddRoutingFlowTable'
)
BEGIN
    CREATE INDEX [IX_InvRoutingFlowLevels_InvRoutingFlowID] ON [CBSAP].[InvRoutingFlowLevels] ([InvRoutingFlowID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250525231655_AddRoutingFlowTable'
)
BEGIN
    CREATE INDEX [IX_InvRoutingFlowLevels_RoleID] ON [CBSAP].[InvRoutingFlowLevels] ([RoleID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250525231655_AddRoutingFlowTable'
)
BEGIN
    ALTER TABLE [CBSAP].[InvRoutingFlow] ADD CONSTRAINT [FK_InvRoutingFlow_EntityProfile_EntityProfileID] FOREIGN KEY ([EntityProfileID]) REFERENCES [CBSAP].[EntityProfile] ([EntityProfileID]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250525231655_AddRoutingFlowTable'
)
BEGIN
    ALTER TABLE [CBSAP].[InvRoutingFlow] ADD CONSTRAINT [FK_InvRoutingFlow_SupplierInfo_SupplierInfoID] FOREIGN KEY ([SupplierInfoID]) REFERENCES [CBSAP].[SupplierInfo] ([SupplierInfoID]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250525231655_AddRoutingFlowTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250525231655_AddRoutingFlowTable', N'9.0.4');
END;

COMMIT;
GO

