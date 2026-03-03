BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250514005732_AddAccountTable'
)
BEGIN
    CREATE TABLE [CBSAP].[Account] (
        [AccountID] bigint NOT NULL IDENTITY,
        [AccountName] NVARCHAR(50) NOT NULL,
        [IsActive] bit NOT NULL,
       
        CONSTRAINT [PK_Account] PRIMARY KEY ([AccountID])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250514005732_AddAccountTable'
)
BEGIN
    CREATE TABLE [CBSAP].[InvRoutingFlow] (
        [InvRoutingFlowID] bigint NOT NULL IDENTITY,
        [InvRoutingFlowName] NVARCHAR(30) NOT NULL,
        [IsActive] bit NOT NULL,
      
        CONSTRAINT [PK_InvRoutingFlow] PRIMARY KEY ([InvRoutingFlowID])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250514005732_AddAccountTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250514005732_AddAccountTable', N'8.0.14');
END;
GO

COMMIT;
GO

