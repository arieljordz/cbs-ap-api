BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260519072743_approverrole_changes'
)
BEGIN
    ALTER TABLE [CBSAP].[Invoice] DROP CONSTRAINT [FK_Invoice_Role_ApprovedUser];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260519072743_approverrole_changes'
)
BEGIN
    ALTER TABLE [CBSAP].[Invoice] ADD CONSTRAINT [FK_Invoice_UserAccount_ApprovedUser] FOREIGN KEY ([ApprovedUser]) REFERENCES [CBSAP].[UserAccount] ([UserAccountID]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260519072743_approverrole_changes'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260519072743_approverrole_changes', N'9.0.4');
END;

COMMIT;
GO

