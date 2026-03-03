BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250617004507_AddInvoiceRelatedTables'
)
BEGIN
    CREATE TABLE [CBSAP].[Invoice] (
        [InvoiceID] bigint NOT NULL IDENTITY,
        [InvoiceNo] nvarchar(50) NULL,
        [InvoiceDate] datetimeoffset NULL,
        [MapID] nvarchar(100) NULL,
        [ScanDate] datetimeoffset NULL,
        [EntityProfileID] bigint NULL,
        [SupplierInfoID] bigint NULL,
        [SuppBankAccount] nvarchar(100) NULL,
        [DueDate] datetimeoffset NULL,
        [PoNo] nvarchar(50) NULL,
        [GrNo] nvarchar(50) NULL,
        [Currency] nvarchar(10) NULL,
        [NetAmount] decimal(18,2) NOT NULL,
        [TaxAmount] decimal(18,2) NOT NULL,
        [TotalAmount] decimal(18,2) NOT NULL,
        [TaxCodeID] bigint NOT NULL,
        [PaymentTerm] nvarchar(50) NULL,
        [Note] nvarchar(1000) NULL,
        [ApproverRole] nvarchar(100) NULL,
        [ApprovedUser] nvarchar(100) NULL,
        [QueueType] int NULL,
        [StatusType] int NULL,
        [CreatedBy] nvarchar(255) NULL,
        [CreatedDate] datetimeoffset NULL,
        [LastUpdatedBy] nvarchar(255) NULL,
        [LastUpdatedDate] datetimeoffset NULL,
        CONSTRAINT [PK_Invoice] PRIMARY KEY ([InvoiceID]),
        CONSTRAINT [FK_Invoice_EntityProfile_EntityProfileID] FOREIGN KEY ([EntityProfileID]) REFERENCES [CBSAP].[EntityProfile] ([EntityProfileID]) ON DELETE SET NULL,
        CONSTRAINT [FK_Invoice_SupplierInfo_SupplierInfoID] FOREIGN KEY ([SupplierInfoID]) REFERENCES [CBSAP].[SupplierInfo] ([SupplierInfoID]) ON DELETE SET NULL,
        CONSTRAINT [FK_Invoice_TaxCode_TaxCodeID] FOREIGN KEY ([TaxCodeID]) REFERENCES [CBSAP].[TaxCode] ([TaxCodeID]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250617004507_AddInvoiceRelatedTables'
)
BEGIN
    CREATE TABLE [CBSAP].[InvAllocLine] (
        [InvAllocLineID] bigint NOT NULL IDENTITY,
        [InvoiceID] bigint NULL,
        [LineNo] nvarchar(50) NULL,
        [PoNo] nvarchar(50) NULL,
        [PoLineNo] nvarchar(50) NULL,
        [Account] nvarchar(100) NULL,
        [LineDescription] nvarchar(500) NULL,
        [Qty] decimal(18,4) NOT NULL,
        [LineNetAmount] decimal(18,2) NOT NULL,
        [LineTaxAmount] decimal(18,2) NOT NULL,
        [LineAmount] decimal(18,2) NOT NULL,
        [Currency] nvarchar(10) NULL,
        [TaxCodeID] bigint NOT NULL,
        [LineApproved] nvarchar(20) NULL,
        [Note] nvarchar(1000) NULL,
        [CreatedBy] nvarchar(255) NULL,
        [CreatedDate] datetimeoffset NULL,
        [LastUpdatedBy] nvarchar(255) NULL,
        [LastUpdatedDate] datetimeoffset NULL,
        CONSTRAINT [PK_InvAllocLine] PRIMARY KEY ([InvAllocLineID]),
        CONSTRAINT [FK_InvAllocLine_Invoice_InvoiceID] FOREIGN KEY ([InvoiceID]) REFERENCES [CBSAP].[Invoice] ([InvoiceID]) ON DELETE CASCADE,
        CONSTRAINT [FK_InvAllocLine_TaxCode_TaxCodeID] FOREIGN KEY ([TaxCodeID]) REFERENCES [CBSAP].[TaxCode] ([TaxCodeID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250617004507_AddInvoiceRelatedTables'
)
BEGIN
    CREATE TABLE [CBSAP].[InvoiceFreeField] (
        [InvoiceFreeFieldID] bigint NOT NULL IDENTITY,
        [InvoiceID] bigint NULL,
        [FieldKey] nvarchar(50) NULL,
        [FieldValue] nvarchar(200) NULL,
        CONSTRAINT [PK_InvoiceFreeField] PRIMARY KEY ([InvoiceFreeFieldID]),
        CONSTRAINT [FK_InvoiceFreeField_Invoice_InvoiceID] FOREIGN KEY ([InvoiceID]) REFERENCES [CBSAP].[Invoice] ([InvoiceID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250617004507_AddInvoiceRelatedTables'
)
BEGIN
    CREATE TABLE [CBSAP].[InvoiceSpareAmount] (
        [InvoiceSpareAmountID] bigint NOT NULL IDENTITY,
        [InvoiceID] bigint NULL,
        [FieldKey] nvarchar(50) NULL,
        [FieldValue] nvarchar(200) NULL,
        CONSTRAINT [PK_InvoiceSpareAmount] PRIMARY KEY ([InvoiceSpareAmountID]),
        CONSTRAINT [FK_InvoiceSpareAmount_Invoice_InvoiceID] FOREIGN KEY ([InvoiceID]) REFERENCES [CBSAP].[Invoice] ([InvoiceID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250617004507_AddInvoiceRelatedTables'
)
BEGIN
    CREATE TABLE [CBSAP].[InvAllocLineDimension] (
        [InvAllocLineDimensionID] bigint NOT NULL IDENTITY,
        [InvAllocLineID] bigint NULL,
        [DimensionKey] nvarchar(50) NOT NULL,
        [DimensionValue] nvarchar(200) NOT NULL,
        CONSTRAINT [PK_InvAllocLineDimension] PRIMARY KEY ([InvAllocLineDimensionID]),
        CONSTRAINT [FK_InvAllocLineDimension_InvAllocLine_InvAllocLineID] FOREIGN KEY ([InvAllocLineID]) REFERENCES [CBSAP].[InvAllocLine] ([InvAllocLineID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250617004507_AddInvoiceRelatedTables'
)
BEGIN
    CREATE TABLE [CBSAP].[InvAllocLineFreeField] (
        [InvAllocLineFieldID] bigint NOT NULL IDENTITY,
        [InvAllocLineID] bigint NULL,
        [FieldKey] nvarchar(50) NULL,
        [FieldValue] nvarchar(200) NULL,
        CONSTRAINT [PK_InvAllocLineFreeField] PRIMARY KEY ([InvAllocLineFieldID]),
        CONSTRAINT [FK_InvAllocLineFreeField_InvAllocLine_InvAllocLineID] FOREIGN KEY ([InvAllocLineID]) REFERENCES [CBSAP].[InvAllocLine] ([InvAllocLineID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250617004507_AddInvoiceRelatedTables'
)
BEGIN
    CREATE INDEX [IX_InvAllocLine_InvoiceID] ON [CBSAP].[InvAllocLine] ([InvoiceID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250617004507_AddInvoiceRelatedTables'
)
BEGIN
    CREATE INDEX [IX_InvAllocLine_TaxCodeID] ON [CBSAP].[InvAllocLine] ([TaxCodeID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250617004507_AddInvoiceRelatedTables'
)
BEGIN
    CREATE INDEX [IX_InvAllocLineDimension_InvAllocLineID] ON [CBSAP].[InvAllocLineDimension] ([InvAllocLineID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250617004507_AddInvoiceRelatedTables'
)
BEGIN
    CREATE INDEX [IX_InvAllocLineFreeField_InvAllocLineID] ON [CBSAP].[InvAllocLineFreeField] ([InvAllocLineID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250617004507_AddInvoiceRelatedTables'
)
BEGIN
    CREATE INDEX [IX_Invoice_EntityProfileID] ON [CBSAP].[Invoice] ([EntityProfileID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250617004507_AddInvoiceRelatedTables'
)
BEGIN
    CREATE INDEX [IX_Invoice_SupplierInfoID] ON [CBSAP].[Invoice] ([SupplierInfoID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250617004507_AddInvoiceRelatedTables'
)
BEGIN
    CREATE INDEX [IX_Invoice_TaxCodeID] ON [CBSAP].[Invoice] ([TaxCodeID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250617004507_AddInvoiceRelatedTables'
)
BEGIN
    CREATE INDEX [IX_InvoiceFreeField_InvoiceID] ON [CBSAP].[InvoiceFreeField] ([InvoiceID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250617004507_AddInvoiceRelatedTables'
)
BEGIN
    CREATE INDEX [IX_InvoiceSpareAmount_InvoiceID] ON [CBSAP].[InvoiceSpareAmount] ([InvoiceID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250617004507_AddInvoiceRelatedTables'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250617004507_AddInvoiceRelatedTables', N'9.0.4');
END;

COMMIT;
GO

