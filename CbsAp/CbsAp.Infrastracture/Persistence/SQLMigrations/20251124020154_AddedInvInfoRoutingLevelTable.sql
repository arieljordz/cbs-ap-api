BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251124020154_AddedInvInfoRoutingLevelTable'
)
BEGIN
    ALTER TABLE [CBSAP].[Invoice] ADD [InvRoutingFlowID] bigint NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251124020154_AddedInvInfoRoutingLevelTable'
)
BEGIN
    CREATE TABLE [CBSAP].[InvInfoRoutingLevel] (
        [InvInfoRoutingLevelID] bigint NOT NULL IDENTITY,
        [InvRoutingFlowID] bigint NULL,
        [InvoiceID] bigint NULL,
        [RoleID] bigint NOT NULL,
        [Level] int NOT NULL,
        CONSTRAINT [PK_InvInfoRoutingLevel] PRIMARY KEY ([InvInfoRoutingLevelID]),
        CONSTRAINT [FK_InvInfoRoutingLevel_InvRoutingFlow_InvRoutingFlowID] FOREIGN KEY ([InvRoutingFlowID]) REFERENCES [CBSAP].[InvRoutingFlow] ([InvRoutingFlowID]) ON DELETE NO ACTION,
        CONSTRAINT [FK_InvInfoRoutingLevel_Invoice_InvoiceID] FOREIGN KEY ([InvoiceID]) REFERENCES [CBSAP].[Invoice] ([InvoiceID]) ON DELETE NO ACTION,
        CONSTRAINT [FK_InvInfoRoutingLevel_Role_RoleID] FOREIGN KEY ([RoleID]) REFERENCES [CBSAP].[Role] ([RoleID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251124020154_AddedInvInfoRoutingLevelTable'
)
BEGIN
    CREATE INDEX [IX_InvInfoRoutingLevel_InvoiceID] ON [CBSAP].[InvInfoRoutingLevel] ([InvoiceID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251124020154_AddedInvInfoRoutingLevelTable'
)
BEGIN
    CREATE INDEX [IX_InvInfoRoutingLevel_InvRoutingFlowID] ON [CBSAP].[InvInfoRoutingLevel] ([InvRoutingFlowID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251124020154_AddedInvInfoRoutingLevelTable'
)
BEGIN
    CREATE INDEX [IX_InvInfoRoutingLevel_RoleID] ON [CBSAP].[InvInfoRoutingLevel] ([RoleID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251124020154_AddedInvInfoRoutingLevelTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251124020154_AddedInvInfoRoutingLevelTable', N'9.0.4');
END;

COMMIT;
GO

