BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128023623_AddedKeywordInInvoice'
)
BEGIN
    EXEC sp_rename N'[CBSAP].[Dimension].[Active]', N'IsActive', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128023623_AddedKeywordInInvoice'
)
BEGIN
    DECLARE @var sysname;
    SELECT @var = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[Keyword]') AND [c].[name] = N'KeywordName');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[Keyword] DROP CONSTRAINT [' + @var + '];');
    EXEC(N'UPDATE [CBSAP].[Keyword] SET [KeywordName] = N'''' WHERE [KeywordName] IS NULL');
    ALTER TABLE [CBSAP].[Keyword] ALTER COLUMN [KeywordName] NVARCHAR(100) NOT NULL;
    ALTER TABLE [CBSAP].[Keyword] ADD DEFAULT N'' FOR [KeywordName];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128023623_AddedKeywordInInvoice'
)
BEGIN
    ALTER TABLE [CBSAP].[InvoiceArchive] ADD [KeywordID] bigint NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128023623_AddedKeywordInInvoice'
)
BEGIN
    ALTER TABLE [CBSAP].[Invoice] ADD [KeywordID] bigint NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128023623_AddedKeywordInInvoice'
)
BEGIN
    CREATE INDEX [IX_InvoiceArchive_KeywordID] ON [CBSAP].[InvoiceArchive] ([KeywordID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128023623_AddedKeywordInInvoice'
)
BEGIN
    CREATE INDEX [IX_Invoice_KeywordID] ON [CBSAP].[Invoice] ([KeywordID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128023623_AddedKeywordInInvoice'
)
BEGIN
    ALTER TABLE [CBSAP].[Invoice] ADD CONSTRAINT [FK_Invoice_Keyword_KeywordID] FOREIGN KEY ([KeywordID]) REFERENCES [CBSAP].[Keyword] ([KeywordID]) ON DELETE SET NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128023623_AddedKeywordInInvoice'
)
BEGIN
    ALTER TABLE [CBSAP].[InvoiceArchive] ADD CONSTRAINT [FK_InvoiceArchive_Keyword_KeywordID] FOREIGN KEY ([KeywordID]) REFERENCES [CBSAP].[Keyword] ([KeywordID]) ON DELETE SET NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128023623_AddedKeywordInInvoice'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251128023623_AddedKeywordInInvoice', N'9.0.4');
END;

COMMIT;
GO

