BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513215803_recreate_supplierbankaccounttable'
)
BEGIN
    CREATE TABLE [CBSAP].[SupplierBankAccount] (
        [SupplierBankAccountID] bigint NOT NULL IDENTITY,
        [SupplierInfoID] bigint NOT NULL,
        [BankAccountNumber] NVARCHAR(40) NOT NULL,
        [BankName] NVARCHAR(40) NULL,
        [IsActive] bit NOT NULL,
        [CreatedBy] nvarchar(255) NULL,
        [CreatedDate] datetimeoffset NULL,
        [LastUpdatedBy] nvarchar(255) NULL,
        [LastUpdatedDate] datetimeoffset NULL,
        CONSTRAINT [PK_SupplierBankAccount] PRIMARY KEY ([SupplierBankAccountID]),
        CONSTRAINT [FK_SupplierBankAccount_SupplierInfo_SupplierBankAccountID] FOREIGN KEY ([SupplierBankAccountID]) REFERENCES [CBSAP].[SupplierInfo] ([SupplierInfoID]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513215803_recreate_supplierbankaccounttable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260513215803_recreate_supplierbankaccounttable', N'9.0.4');
END;

COMMIT;
GO

