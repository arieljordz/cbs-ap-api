BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251204222038_AddKeywordIDandSupplierInfoIDInvInfoLevel'
)
BEGIN
    ALTER TABLE [CBSAP].[InvInfoRoutingLevel] ADD [KeywordID] bigint NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251204222038_AddKeywordIDandSupplierInfoIDInvInfoLevel'
)
BEGIN
    ALTER TABLE [CBSAP].[InvInfoRoutingLevel] ADD [SupplierInfoID] bigint NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251204222038_AddKeywordIDandSupplierInfoIDInvInfoLevel'
)
BEGIN
    CREATE INDEX [IX_InvInfoRoutingLevel_KeywordID] ON [CBSAP].[InvInfoRoutingLevel] ([KeywordID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251204222038_AddKeywordIDandSupplierInfoIDInvInfoLevel'
)
BEGIN
    CREATE INDEX [IX_InvInfoRoutingLevel_SupplierInfoID] ON [CBSAP].[InvInfoRoutingLevel] ([SupplierInfoID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251204222038_AddKeywordIDandSupplierInfoIDInvInfoLevel'
)
BEGIN
    ALTER TABLE [CBSAP].[InvInfoRoutingLevel] ADD CONSTRAINT [FK_InvInfoRoutingLevel_Keyword_KeywordID] FOREIGN KEY ([KeywordID]) REFERENCES [CBSAP].[Keyword] ([KeywordID]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251204222038_AddKeywordIDandSupplierInfoIDInvInfoLevel'
)
BEGIN
    ALTER TABLE [CBSAP].[InvInfoRoutingLevel] ADD CONSTRAINT [FK_InvInfoRoutingLevel_SupplierInfo_SupplierInfoID] FOREIGN KEY ([SupplierInfoID]) REFERENCES [CBSAP].[SupplierInfo] ([SupplierInfoID]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251204222038_AddKeywordIDandSupplierInfoIDInvInfoLevel'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251204222038_AddKeywordIDandSupplierInfoIDInvInfoLevel', N'9.0.4');
END;

COMMIT;
GO

