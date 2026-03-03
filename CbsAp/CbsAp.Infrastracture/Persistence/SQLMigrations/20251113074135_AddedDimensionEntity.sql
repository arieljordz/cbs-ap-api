BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251113074135_AddedDimensionEntity'
)
BEGIN
    CREATE TABLE [CBSAP].[Dimension] (
        [DimensionID] bigint NOT NULL IDENTITY,
        [Dimension] NVARCHAR(15) NOT NULL,
        [Name] NVARCHAR(50) NOT NULL,
        [Active] bit NOT NULL,
        [FreeField1] NVARCHAR(90) NULL,
        [FreeField2] NVARCHAR(90) NULL,
        [FreeField3] NVARCHAR(90) NULL,
        [EntityProfileID] bigint NOT NULL,
        [CreatedBy] nvarchar(255) NULL,
        [CreatedDate] datetimeoffset NULL,
        [LastUpdatedBy] nvarchar(255) NULL,
        [LastUpdatedDate] datetimeoffset NULL,
        CONSTRAINT [PK_Dimension] PRIMARY KEY ([DimensionID]),
        CONSTRAINT [FK_Dimension_EntityProfile_EntityProfileID] FOREIGN KEY ([EntityProfileID]) REFERENCES [CBSAP].[EntityProfile] ([EntityProfileID]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251113074135_AddedDimensionEntity'
)
BEGIN
    CREATE INDEX [IX_Dimension_EntityProfileID] ON [CBSAP].[Dimension] ([EntityProfileID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251113074135_AddedDimensionEntity'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251113074135_AddedDimensionEntity', N'9.0.4');
END;

COMMIT;
GO

