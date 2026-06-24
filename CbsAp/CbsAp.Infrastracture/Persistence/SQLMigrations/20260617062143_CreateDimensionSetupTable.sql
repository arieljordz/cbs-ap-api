BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260617062143_CreateDimensionSetupTable'
)
BEGIN
    CREATE TABLE [CBSAP].[DimensionSetup] (
        [DimensionSetupId] bigint NOT NULL IDENTITY,
        [DimensionSetupName] NVARCHAR(150) NULL,
        [DisplayOrder] smallint NOT NULL,
        [DimensionNameId] bigint NULL,
        [DimensionValueId] bigint NULL,
        [Required] bit NULL,
        [Show] bit NULL,
        [CreatedBy] nvarchar(255) NULL,
        [CreatedDate] datetimeoffset NULL,
        [LastUpdatedBy] nvarchar(255) NULL,
        [LastUpdatedDate] datetimeoffset NULL,
        CONSTRAINT [PK_DimensionSetup] PRIMARY KEY ([DimensionSetupId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260617062143_CreateDimensionSetupTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260617062143_CreateDimensionSetupTable', N'9.0.4');
END;


IF NOT EXISTS (
    SELECT * 
    FROM CBSAP.DimensionSetup 
    WHERE DimensionSetupName = 'Dimension 1'
)
BEGIN
   INSERT INTO CBSAP.DimensionSetup 
   VALUES ('Dimension 1',1,null,null,0,1,null,null,null,null);
END


IF NOT EXISTS (
    SELECT * 
    FROM CBSAP.DimensionSetup 
    WHERE DimensionSetupName = 'Dimension 2'
)
BEGIN
   INSERT INTO CBSAP.DimensionSetup 
   VALUES ('Dimension 2',2,null,null,0,1,null,null,null,null);
END


IF NOT EXISTS (
    SELECT * 
    FROM CBSAP.DimensionSetup 
    WHERE DimensionSetupName = 'Dimension 3'
)
BEGIN
   INSERT INTO CBSAP.DimensionSetup 
   VALUES ('Dimension 3',3,null,null,0,1,null,null,null,null);
END



IF NOT EXISTS (
    SELECT * 
    FROM CBSAP.DimensionSetup 
    WHERE DimensionSetupName = 'Dimension 4'
)
BEGIN
   INSERT INTO CBSAP.DimensionSetup 
   VALUES ('Dimension 4',4,null,null,0,1,null,null,null,null);
END



IF NOT EXISTS (
    SELECT * 
    FROM CBSAP.DimensionSetup 
    WHERE DimensionSetupName = 'Dimension 5'
)
BEGIN
   INSERT INTO CBSAP.DimensionSetup 
   VALUES ('Dimension 5',5,null,null,0,1,null,null,null,null);
END



IF NOT EXISTS (
    SELECT * 
    FROM CBSAP.DimensionSetup 
    WHERE DimensionSetupName = 'Dimension 6'
)
BEGIN
   INSERT INTO CBSAP.DimensionSetup 
   VALUES ('Dimension 6',6,null,null,0,1,null,null,null,null);
END



IF NOT EXISTS (
    SELECT * 
    FROM CBSAP.DimensionSetup 
    WHERE DimensionSetupName = 'Dimension 7'
)
BEGIN
   INSERT INTO CBSAP.DimensionSetup 
   VALUES ('Dimension 7',7,null,null,0,1,null,null,null,null);
END



IF NOT EXISTS (
    SELECT * 
    FROM CBSAP.DimensionSetup 
    WHERE DimensionSetupName = 'Dimension 8'
)
BEGIN
   INSERT INTO CBSAP.DimensionSetup 
   VALUES ('Dimension 8',8,null,null,0,1,null,null,null,null);
END

--Menu


IF NOT EXISTS (
    SELECT * 
    FROM CBSAP.MenuItem
    WHERE [Label] = 'Dimension Setup' AND MenuID = 7
)
BEGIN
   INSERT INTO CBSAP.MenuItem 
   VALUES (7,'Dimension Setup','pi pi-cog','/dimension-setup');
END




COMMIT;
GO

