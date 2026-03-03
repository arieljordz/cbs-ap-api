BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250514042330_AddSupplierInfoTable'
)
BEGIN
    CREATE TABLE [CBSAP].[SupplierInfo] (
        [SupplierInfoID] BIGINT IDENTITY(1,1) NOT NULL,
        [SupplierID] NVARCHAR(40) NOT NULL,
        [SupplierTaxID] NVARCHAR(40) NULL,
        [EntityProfileID] bigint NULL,
        [SupplierName] NVARCHAR(200) NOT NULL,
        [IsActive] bit NOT NULL,
        [Telephone] NVARCHAR(30) NULL,
        [EmailAddress] NVARCHAR(90) NULL,
        [Contact] NVARCHAR(90) NULL,
        [AddressLine1] NVARCHAR(90) NULL,
        [AddressLine2] NVARCHAR(90) NULL,
        [AddressLine3] NVARCHAR(90) NULL,
        [AddressLine4] NVARCHAR(90) NULL,
        [AddressLine5] NVARCHAR(90) NULL,
        [AddressLine6] NVARCHAR(90) NULL,
        [AccountID] bigint NULL,
        [TaxCodeID] bigint NULL,
        [Currency] NVARCHAR(3) NULL,
        [InvRoutingFlowID] bigint NULL,
        [FreeField1] NVARCHAR(90) NULL,
        [FreeField2] nvarchar(90) NULL,
        [FreeField3] NVARCHAR(90) NULL,
        [Notes] NVARCHAR(400) NULL,
        [CreatedBy] nvarchar(255) NULL,
        [CreatedDate] datetimeoffset NULL,
        [LastUpdatedBy] nvarchar(255) NULL,
        [LastUpdatedDate] datetimeoffset NULL,
        CONSTRAINT [PK_SupplierInfo] PRIMARY KEY ([SupplierInfoID]),
        CONSTRAINT [FK_SupplierInfo_Account_AccountID] FOREIGN KEY ([AccountID]) REFERENCES [CBSAP].[Account] ([AccountID]) ON DELETE NO ACTION,
        CONSTRAINT [FK_SupplierInfo_EntityProfile_EntityProfileID] FOREIGN KEY ([EntityProfileID]) REFERENCES [CBSAP].[EntityProfile] ([EntityProfileID]) ON DELETE NO ACTION,
        CONSTRAINT [FK_SupplierInfo_InvRoutingFlow_InvRoutingFlowID] FOREIGN KEY ([InvRoutingFlowID]) REFERENCES [CBSAP].[InvRoutingFlow] ([InvRoutingFlowID]) ON DELETE NO ACTION,
        CONSTRAINT [FK_SupplierInfo_TaxCode_TaxCodeID] FOREIGN KEY ([TaxCodeID]) REFERENCES [CBSAP].[TaxCode] ([TaxCodeID]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250514042330_AddSupplierInfoTable'
)
BEGIN
    CREATE INDEX [IX_SupplierInfo_AccountID] ON [CBSAP].[SupplierInfo] ([AccountID]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250514042330_AddSupplierInfoTable'
)
BEGIN
    CREATE INDEX [IX_SupplierInfo_EntityProfileID] ON [CBSAP].[SupplierInfo] ([EntityProfileID]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250514042330_AddSupplierInfoTable'
)
BEGIN
    CREATE INDEX [IX_SupplierInfo_InvRoutingFlowID] ON [CBSAP].[SupplierInfo] ([InvRoutingFlowID]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250514042330_AddSupplierInfoTable'
)
BEGIN
    CREATE INDEX [IX_SupplierInfo_TaxCodeID] ON [CBSAP].[SupplierInfo] ([TaxCodeID]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250514042330_AddSupplierInfoTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250514042330_AddSupplierInfoTable', N'8.0.14');
END;
GO

COMMIT;
GO

