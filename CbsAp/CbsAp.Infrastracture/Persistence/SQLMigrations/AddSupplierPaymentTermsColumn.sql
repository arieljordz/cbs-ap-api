BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250515021916_AddSupplierpaymentTermsColumn'
)
BEGIN
    ALTER TABLE [CBSAP].[SupplierInfo] ADD [PaymentTerms] NVARCHAR(4) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250515021916_AddSupplierpaymentTermsColumn'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250515021916_AddSupplierpaymentTermsColumn', N'8.0.14');
END;
GO

COMMIT;
GO

