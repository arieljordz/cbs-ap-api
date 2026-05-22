IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260505020507_20260505094445_AddInvoiceAttachmentColumn'
)
BEGIN
    ALTER TABLE [CBSAP].[InvoiceAttachment] ADD [UploadedBy] nvarchar(max) NULL;
END;
 
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260505020507_20260505094445_AddInvoiceAttachmentColumn'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260505020507_20260505094445_AddInvoiceAttachmentColumn', N'9.0.4');
END;
 
COMMIT;
GO