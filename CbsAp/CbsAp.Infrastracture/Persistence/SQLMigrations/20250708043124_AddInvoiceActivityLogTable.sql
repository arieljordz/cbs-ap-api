BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250708043124_AddInvoiceActivityLogTable'
)
BEGIN
    CREATE TABLE [CBSAP].[InvoiceActivityLog] (
        [ActivityLogID] bigint NOT NULL IDENTITY,
        [InvoiceID] bigint NOT NULL,
        [PreviousStatus] int NULL,
        [CurrentStatus] int NULL,
        [Reason] nvarchar(255) NULL,
        [CreatedBy] nvarchar(255) NULL,
        [CreatedDate] datetimeoffset NULL,
        CONSTRAINT [PK_InvoiceActivityLog] PRIMARY KEY ([ActivityLogID]),
        CONSTRAINT [FK_InvoiceActivityLog_Invoice_InvoiceID] FOREIGN KEY ([InvoiceID]) REFERENCES [CBSAP].[Invoice] ([InvoiceID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250708043124_AddInvoiceActivityLogTable'
)
BEGIN
    CREATE INDEX [IX_InvoiceActivityLog_InvoiceID] ON [CBSAP].[InvoiceActivityLog] ([InvoiceID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250708043124_AddInvoiceActivityLogTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250708043124_AddInvoiceActivityLogTable', N'9.0.4');
END;

COMMIT;
GO

