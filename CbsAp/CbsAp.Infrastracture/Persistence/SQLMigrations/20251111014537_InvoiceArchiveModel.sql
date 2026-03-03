BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251111014537_InvoiceArchiveModel'
)
BEGIN
    CREATE TABLE [CBSAP].[InvoiceArchive] (
        [InvoiceID] bigint NOT NULL IDENTITY,
        [InvoiceNo] nvarchar(50) NULL,
        [InvoiceDate] datetimeoffset NULL,
        [MapID] nvarchar(100) NULL,
        [ImageID] char(16) NULL,
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
        [FreeField1] nvarchar(200) NULL,
        [FreeField2] nvarchar(200) NULL,
        [FreeField3] nvarchar(200) NULL,
        [SpareAmount1] decimal(18,2) NULL,
        [SpareAmount2] decimal(18,2) NULL,
        [SpareAmount3] decimal(18,2) NULL,
        [QueueType] int NULL,
        [StatusType] int NULL,
        [CreatedBy] nvarchar(255) NULL,
        [CreatedDate] datetimeoffset NULL,
        [LastUpdatedBy] nvarchar(255) NULL,
        [LastUpdatedDate] datetimeoffset NULL,
        CONSTRAINT [PK_InvoiceArchive] PRIMARY KEY ([InvoiceID]),
        CONSTRAINT [FK_InvoiceArchive_EntityProfile_EntityProfileID] FOREIGN KEY ([EntityProfileID]) REFERENCES [CBSAP].[EntityProfile] ([EntityProfileID]) ON DELETE SET NULL,
        CONSTRAINT [FK_InvoiceArchive_SupplierInfo_SupplierInfoID] FOREIGN KEY ([SupplierInfoID]) REFERENCES [CBSAP].[SupplierInfo] ([SupplierInfoID]) ON DELETE SET NULL,
        CONSTRAINT [FK_InvoiceArchive_TaxCode_TaxCodeID] FOREIGN KEY ([TaxCodeID]) REFERENCES [CBSAP].[TaxCode] ([TaxCodeID]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251111014537_InvoiceArchiveModel'
)
BEGIN
    CREATE TABLE [CBSAP].[InvAllocLineArchive] (
        [InvAllocLineID] bigint NOT NULL IDENTITY,
        [InvoiceID] bigint NULL,
        [LineNo] bigint NULL,
        [PoNo] nvarchar(50) NULL,
        [PoLineNo] nvarchar(50) NULL,
        [AccountID] bigint NULL,
        [LineDescription] nvarchar(500) NULL,
        [Qty] decimal(18,4) NOT NULL,
        [LineNetAmount] decimal(18,2) NOT NULL,
        [LineTaxAmount] decimal(18,2) NOT NULL,
        [LineAmount] decimal(18,2) NOT NULL,
        [Currency] nvarchar(10) NULL,
        [TaxCodeID] bigint NULL,
        [LineApproved] nvarchar(20) NULL,
        [Note] nvarchar(1000) NULL,
        [CreatedBy] nvarchar(255) NULL,
        [CreatedDate] datetimeoffset NULL,
        [LastUpdatedBy] nvarchar(255) NULL,
        [LastUpdatedDate] datetimeoffset NULL,
        CONSTRAINT [PK_InvAllocLineArchive] PRIMARY KEY ([InvAllocLineID]),
        CONSTRAINT [FK_InvAllocLineArchive_Account_AccountID] FOREIGN KEY ([AccountID]) REFERENCES [CBSAP].[Account] ([AccountID]) ON DELETE NO ACTION,
        CONSTRAINT [FK_InvAllocLineArchive_InvoiceArchive_InvoiceID] FOREIGN KEY ([InvoiceID]) REFERENCES [CBSAP].[InvoiceArchive] ([InvoiceID]) ON DELETE CASCADE,
        CONSTRAINT [FK_InvAllocLineArchive_TaxCode_TaxCodeID] FOREIGN KEY ([TaxCodeID]) REFERENCES [CBSAP].[TaxCode] ([TaxCodeID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251111014537_InvoiceArchiveModel'
)
BEGIN
    CREATE TABLE [CBSAP].[InvoiceActivityLogArchive] (
        [ActivityLogID] bigint NOT NULL IDENTITY,
        [InvoiceID] bigint NOT NULL,
        [PreviousStatus] int NULL,
        [CurrentStatus] int NULL,
        [Reason] nvarchar(1000) NULL,
        [Action] int NULL,
        [CreatedBy] nvarchar(255) NULL,
        [CreatedDate] datetimeoffset NULL,
        CONSTRAINT [PK_InvoiceActivityLogArchive] PRIMARY KEY ([ActivityLogID]),
        CONSTRAINT [FK_InvoiceActivityLogArchive_InvoiceArchive_InvoiceID] FOREIGN KEY ([InvoiceID]) REFERENCES [CBSAP].[InvoiceArchive] ([InvoiceID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251111014537_InvoiceArchiveModel'
)
BEGIN
    CREATE TABLE [CBSAP].[InvoiceAttachnmentArchive] (
        [InvoiceAttachnmentID] bigint NOT NULL IDENTITY,
        [InvoiceID] bigint NOT NULL,
        [OriginalFileName] nvarchar(255) NULL,
        [StorageFileName] nvarchar(255) NULL,
        [FileType] nvarchar(255) NULL,
        [CreatedBy] nvarchar(255) NULL,
        [CreatedDate] datetimeoffset NULL,
        CONSTRAINT [PK_InvoiceAttachnmentArchive] PRIMARY KEY ([InvoiceAttachnmentID]),
        CONSTRAINT [FK_InvoiceAttachnmentArchive_InvoiceArchive_InvoiceID] FOREIGN KEY ([InvoiceID]) REFERENCES [CBSAP].[InvoiceArchive] ([InvoiceID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251111014537_InvoiceArchiveModel'
)
BEGIN
    CREATE TABLE [CBSAP].[InvoiceCommentArchive] (
        [InvoiceCommentID] bigint NOT NULL IDENTITY,
        [Comment] nvarchar(1000) NULL,
        [InvoiceID] bigint NOT NULL,
        [CreatedBy] nvarchar(255) NULL,
        [CreatedDate] datetimeoffset NULL,
        CONSTRAINT [PK_InvoiceCommentArchive] PRIMARY KEY ([InvoiceCommentID]),
        CONSTRAINT [FK_InvoiceCommentArchive_InvoiceArchive_InvoiceID] FOREIGN KEY ([InvoiceID]) REFERENCES [CBSAP].[InvoiceArchive] ([InvoiceID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251111014537_InvoiceArchiveModel'
)
BEGIN
    CREATE TABLE [CBSAP].[InvAllocLineDimensionArchive] (
        [InvAllocLineDimensionID] bigint NOT NULL IDENTITY,
        [InvAllocLineID] bigint NULL,
        [DimensionKey] nvarchar(100) NOT NULL,
        [DimensionValue] nvarchar(200) NOT NULL,
        CONSTRAINT [PK_InvAllocLineDimensionArchive] PRIMARY KEY ([InvAllocLineDimensionID]),
        CONSTRAINT [FK_InvAllocLineDimensionArchive_InvAllocLineArchive_InvAllocLineID] FOREIGN KEY ([InvAllocLineID]) REFERENCES [CBSAP].[InvAllocLineArchive] ([InvAllocLineID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251111014537_InvoiceArchiveModel'
)
BEGIN
    CREATE TABLE [CBSAP].[InvAllocLineFreeFieldArchive] (
        [InvAllocLineFieldID] bigint NOT NULL IDENTITY,
        [InvAllocLineID] bigint NULL,
        [FieldKey] nvarchar(50) NULL,
        [FieldValue] nvarchar(200) NULL,
        CONSTRAINT [PK_InvAllocLineFreeFieldArchive] PRIMARY KEY ([InvAllocLineFieldID]),
        CONSTRAINT [FK_InvAllocLineFreeFieldArchive_InvAllocLineArchive_InvAllocLineID] FOREIGN KEY ([InvAllocLineID]) REFERENCES [CBSAP].[InvAllocLineArchive] ([InvAllocLineID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251111014537_InvoiceArchiveModel'
)
BEGIN
    CREATE TABLE [CBSAP].[PurchaseOrderMatchTrackingArchive] (
        [PurchaseOrderMatchTrackingID] bigint NOT NULL IDENTITY,
        [PurchaseOrderLineID] bigint NOT NULL,
        [PurchaseOrderID] bigint NOT NULL,
        [InvoiceID] bigint NULL,
        [InvAllocLineID] bigint NULL,
        [Account] nvarchar(40) NULL,
        [Qty] decimal(18,4) NOT NULL,
        [RemainingQty] decimal(18,4) NULL,
        [Amount] decimal(18,2) NULL,
        [NetAmount] decimal(18,2) NULL,
        [TaxAmount] decimal(18,2) NULL,
        [MatchingDate] datetimeoffset NOT NULL,
        [IsSystemMatching] bit NOT NULL,
        [MatchingStatus] int NULL,
        [CreatedBy] nvarchar(255) NULL,
        [CreatedDate] datetimeoffset NULL,
        [LastUpdatedBy] nvarchar(255) NULL,
        [LastUpdatedDate] datetimeoffset NULL,
        CONSTRAINT [PK_PurchaseOrderMatchTrackingArchive] PRIMARY KEY ([PurchaseOrderMatchTrackingID]),
        CONSTRAINT [FK_PurchaseOrderMatchTrackingArchive_InvAllocLineArchive_InvAllocLineID] FOREIGN KEY ([InvAllocLineID]) REFERENCES [CBSAP].[InvAllocLineArchive] ([InvAllocLineID]) ON DELETE NO ACTION,
        CONSTRAINT [FK_PurchaseOrderMatchTrackingArchive_InvoiceArchive_InvoiceID] FOREIGN KEY ([InvoiceID]) REFERENCES [CBSAP].[InvoiceArchive] ([InvoiceID]) ON DELETE NO ACTION,
        CONSTRAINT [FK_PurchaseOrderMatchTrackingArchive_PurchaseOrderLine_PurchaseOrderLineID] FOREIGN KEY ([PurchaseOrderLineID]) REFERENCES [CBSAP].[PurchaseOrderLine] ([PurchaseOrderLineID]) ON DELETE NO ACTION,
        CONSTRAINT [FK_PurchaseOrderMatchTrackingArchive_PurchaseOrder_PurchaseOrderID] FOREIGN KEY ([PurchaseOrderID]) REFERENCES [CBSAP].[PurchaseOrder] ([PurchaseOrderID]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251111014537_InvoiceArchiveModel'
)
BEGIN
    CREATE INDEX [IX_InvAllocLineArchive_AccountID] ON [CBSAP].[InvAllocLineArchive] ([AccountID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251111014537_InvoiceArchiveModel'
)
BEGIN
    CREATE INDEX [IX_InvAllocLineArchive_InvoiceID] ON [CBSAP].[InvAllocLineArchive] ([InvoiceID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251111014537_InvoiceArchiveModel'
)
BEGIN
    CREATE INDEX [IX_InvAllocLineArchive_TaxCodeID] ON [CBSAP].[InvAllocLineArchive] ([TaxCodeID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251111014537_InvoiceArchiveModel'
)
BEGIN
    CREATE INDEX [IX_InvAllocLineDimensionArchive_InvAllocLineID] ON [CBSAP].[InvAllocLineDimensionArchive] ([InvAllocLineID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251111014537_InvoiceArchiveModel'
)
BEGIN
    CREATE INDEX [IX_InvAllocLineFreeFieldArchive_InvAllocLineID] ON [CBSAP].[InvAllocLineFreeFieldArchive] ([InvAllocLineID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251111014537_InvoiceArchiveModel'
)
BEGIN
    CREATE INDEX [IX_InvoiceActivityLogArchive_InvoiceID] ON [CBSAP].[InvoiceActivityLogArchive] ([InvoiceID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251111014537_InvoiceArchiveModel'
)
BEGIN
    CREATE INDEX [IX_InvoiceArchive_EntityProfileID] ON [CBSAP].[InvoiceArchive] ([EntityProfileID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251111014537_InvoiceArchiveModel'
)
BEGIN
    CREATE INDEX [IX_InvoiceArchive_SupplierInfoID] ON [CBSAP].[InvoiceArchive] ([SupplierInfoID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251111014537_InvoiceArchiveModel'
)
BEGIN
    CREATE INDEX [IX_InvoiceArchive_TaxCodeID] ON [CBSAP].[InvoiceArchive] ([TaxCodeID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251111014537_InvoiceArchiveModel'
)
BEGIN
    CREATE INDEX [IX_InvoiceAttachnmentArchive_InvoiceID] ON [CBSAP].[InvoiceAttachnmentArchive] ([InvoiceID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251111014537_InvoiceArchiveModel'
)
BEGIN
    CREATE INDEX [IX_InvoiceCommentArchive_InvoiceID] ON [CBSAP].[InvoiceCommentArchive] ([InvoiceID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251111014537_InvoiceArchiveModel'
)
BEGIN
    CREATE INDEX [IX_PurchaseOrderMatchTrackingArchive_InvAllocLineID] ON [CBSAP].[PurchaseOrderMatchTrackingArchive] ([InvAllocLineID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251111014537_InvoiceArchiveModel'
)
BEGIN
    CREATE INDEX [IX_PurchaseOrderMatchTrackingArchive_InvoiceID] ON [CBSAP].[PurchaseOrderMatchTrackingArchive] ([InvoiceID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251111014537_InvoiceArchiveModel'
)
BEGIN
    CREATE INDEX [IX_PurchaseOrderMatchTrackingArchive_PurchaseOrderID] ON [CBSAP].[PurchaseOrderMatchTrackingArchive] ([PurchaseOrderID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251111014537_InvoiceArchiveModel'
)
BEGIN
    CREATE INDEX [IX_PurchaseOrderMatchTrackingArchive_PurchaseOrderLineID] ON [CBSAP].[PurchaseOrderMatchTrackingArchive] ([PurchaseOrderLineID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251111014537_InvoiceArchiveModel'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251111014537_InvoiceArchiveModel', N'9.0.4');
END;

COMMIT;
GO

