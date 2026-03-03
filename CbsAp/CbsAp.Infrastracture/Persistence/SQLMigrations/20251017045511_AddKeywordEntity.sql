BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251017045511_AddKeywordEntity'
)
BEGIN
    CREATE TABLE [CBSAP].[Keyword] (
        [KeywordID] bigint NOT NULL IDENTITY,
        [EntityProfileID] bigint NULL,
        [InvoiceRoutingFlowID] bigint NOT NULL,
        [KeywordName] NVARCHAR(100) NULL,
        [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
        [CreatedBy] nvarchar(255) NULL,
        [CreatedDate] datetimeoffset NULL,
        [LastUpdatedBy] nvarchar(255) NULL,
        [LastUpdatedDate] datetimeoffset NULL,
        CONSTRAINT [PK_Keyword] PRIMARY KEY ([KeywordID]),
        CONSTRAINT [FK_Keyword_EntityProfile_EntityProfileID] FOREIGN KEY ([EntityProfileID]) REFERENCES [CBSAP].[EntityProfile] ([EntityProfileID]),
        CONSTRAINT [FK_Keyword_InvRoutingFlow_InvoiceRoutingFlowID] FOREIGN KEY ([InvoiceRoutingFlowID]) REFERENCES [CBSAP].[InvRoutingFlow] ([InvRoutingFlowID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251017045511_AddKeywordEntity'
)
BEGIN
    CREATE INDEX [IX_Keyword_EntityProfileID] ON [CBSAP].[Keyword] ([EntityProfileID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251017045511_AddKeywordEntity'
)
BEGIN
    CREATE INDEX [IX_Keyword_InvoiceRoutingFlowID] ON [CBSAP].[Keyword] ([InvoiceRoutingFlowID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251017045511_AddKeywordEntity'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251017045511_AddKeywordEntity', N'9.0.4');
END;

COMMIT;
GO

