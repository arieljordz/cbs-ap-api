BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260304035210_AddInvFlowStatuses'
)
BEGIN
    ALTER TABLE [CBSAP].[InvInfoRoutingLevel] ADD [InvFlowStatus] int NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260304035210_AddInvFlowStatuses'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260304035210_AddInvFlowStatuses', N'9.0.4');
END;

COMMIT;
GO

