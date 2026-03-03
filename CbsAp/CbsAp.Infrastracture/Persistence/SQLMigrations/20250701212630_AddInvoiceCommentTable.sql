BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250701212630_AddInvoiceCommentTable'
)
BEGIN
    CREATE TABLE [CBSAP].[InvoiceComment] (
        [InvoiceCommentID] bigint NOT NULL IDENTITY,
        [Comment] nvarchar(255) NULL,
        [InvoiceID] bigint NOT NULL,
        [CreatedBy] nvarchar(255) NULL,
        [CreatedDate] datetimeoffset NULL,
        CONSTRAINT [PK_InvoiceComment] PRIMARY KEY ([InvoiceCommentID]),
        CONSTRAINT [FK_InvoiceComment_Invoice_InvoiceID] FOREIGN KEY ([InvoiceID]) REFERENCES [CBSAP].[Invoice] ([InvoiceID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250701212630_AddInvoiceCommentTable'
)
BEGIN
    CREATE INDEX [IX_InvoiceComment_InvoiceID] ON [CBSAP].[InvoiceComment] ([InvoiceID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250701212630_AddInvoiceCommentTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250701212630_AddInvoiceCommentTable', N'9.0.4');
END;

COMMIT;
GO

