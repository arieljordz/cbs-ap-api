BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250526012321_AddAuditFieldsOnCreateInvroutingFlowTable'
)
BEGIN
    ALTER TABLE [CBSAP].[InvRoutingFlow] ADD [CreatedBy] nvarchar(255) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250526012321_AddAuditFieldsOnCreateInvroutingFlowTable'
)
BEGIN
    ALTER TABLE [CBSAP].[InvRoutingFlow] ADD [CreatedDate] datetimeoffset NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250526012321_AddAuditFieldsOnCreateInvroutingFlowTable'
)
BEGIN
    ALTER TABLE [CBSAP].[InvRoutingFlow] ADD [LastUpdatedBy] nvarchar(255) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250526012321_AddAuditFieldsOnCreateInvroutingFlowTable'
)
BEGIN
    ALTER TABLE [CBSAP].[InvRoutingFlow] ADD [LastUpdatedDate] datetimeoffset NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250526012321_AddAuditFieldsOnCreateInvroutingFlowTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250526012321_AddAuditFieldsOnCreateInvroutingFlowTable', N'9.0.4');
END;

COMMIT;
GO

