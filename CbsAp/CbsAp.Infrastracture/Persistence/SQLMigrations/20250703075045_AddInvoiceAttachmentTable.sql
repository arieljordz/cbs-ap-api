BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250703075045_AddInvoiceAttachmentTable'
)
BEGIN
    CREATE TABLE [CBSAP].[InvoiceAttachnment] (
        [InvoiceAttachnmentID] bigint NOT NULL IDENTITY,
        [InvoiceID] bigint NOT NULL,
        [OriginalFileName] nvarchar(255) NULL,
        [StorageFileName] nvarchar(255) NULL,
        [FileType] nvarchar(50) NULL,
        [CreatedBy] nvarchar(255) NULL,
        [CreatedDate] datetimeoffset NULL,
        CONSTRAINT [PK_InvoiceAttachnment] PRIMARY KEY ([InvoiceAttachnmentID]),
        CONSTRAINT [FK_InvoiceAttachnment_Invoice_InvoiceID] FOREIGN KEY ([InvoiceID]) REFERENCES [CBSAP].[Invoice] ([InvoiceID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250703075045_AddInvoiceAttachmentTable'
)
BEGIN
    CREATE INDEX [IX_InvoiceAttachnment_InvoiceID] ON [CBSAP].[InvoiceAttachnment] ([InvoiceID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250703075045_AddInvoiceAttachmentTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250703075045_AddInvoiceAttachmentTable', N'9.0.4');
END;

COMMIT;
GO

