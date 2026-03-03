BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250706234404_ALTERInvoiceAttachmentTableFileType'
)
BEGIN
    DECLARE @var sysname;
    SELECT @var = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[InvoiceAttachnment]') AND [c].[name] = N'FileType');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[InvoiceAttachnment] DROP CONSTRAINT [' + @var + '];');
    ALTER TABLE [CBSAP].[InvoiceAttachnment] ALTER COLUMN [FileType] nvarchar(255) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250706234404_ALTERInvoiceAttachmentTableFileType'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250706234404_ALTERInvoiceAttachmentTableFileType', N'9.0.4');
END;

COMMIT;
GO

