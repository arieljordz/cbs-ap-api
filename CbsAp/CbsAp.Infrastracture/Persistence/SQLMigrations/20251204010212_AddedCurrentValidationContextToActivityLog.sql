BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251204010212_AddedCurrentValidationContextToActivityLog'
)
BEGIN
    ALTER TABLE [CBSAP].[InvoiceActivityLog] ADD [IsCurrentValidationContext] bit NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251204010212_AddedCurrentValidationContextToActivityLog'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251204010212_AddedCurrentValidationContextToActivityLog', N'9.0.4');
END;

COMMIT;
GO

