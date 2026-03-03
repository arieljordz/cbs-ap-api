IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF SCHEMA_ID(N'CBSAP') IS NULL EXEC(N'CREATE SCHEMA [CBSAP];');
GO

CREATE TABLE [CBSAP].[UserAccount] (
    [UserId] nvarchar(450) NOT NULL,
    [FirstName] nvarchar(200) NOT NULL,
    [LastName] nvarchar(200) NOT NULL,
    [EmailAddress] nvarchar(200) NOT NULL,
    [BirthDate] datetime2 NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedBy] nvarchar(255) NULL,
    [CreatedDate] datetimeoffset NOT NULL DEFAULT '2023-11-28T13:12:08.1169542+08:00',
    [LastUpdatedBy] nvarchar(255) NULL,
    [LastUpdatedDate] datetimeoffset NOT NULL DEFAULT '2023-11-28T13:12:08.1170559+08:00',
    CONSTRAINT [PK_UserAccount] PRIMARY KEY ([UserId])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231128051208_IntialCreateDB', N'7.0.12');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[UserAccount]') AND [c].[name] = N'LastUpdatedDate');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[UserAccount] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [CBSAP].[UserAccount] ADD DEFAULT '2023-11-28T13:30:36.5340326+08:00' FOR [LastUpdatedDate];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[UserAccount]') AND [c].[name] = N'CreatedDate');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[UserAccount] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [CBSAP].[UserAccount] ADD DEFAULT '2023-11-28T13:30:36.5339525+08:00' FOR [CreatedDate];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231128053036_IntialCreateDBinLocalhost', N'7.0.12');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[UserAccount]') AND [c].[name] = N'LastUpdatedDate');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[UserAccount] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [CBSAP].[UserAccount] ADD DEFAULT '2023-11-29T07:29:26.2728045+08:00' FOR [LastUpdatedDate];
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[UserAccount]') AND [c].[name] = N'CreatedDate');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[UserAccount] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [CBSAP].[UserAccount] ADD DEFAULT '2023-11-29T07:29:26.2727370+08:00' FOR [CreatedDate];
GO

CREATE TABLE [CBSAP].[UserLogInfo] (
    [UserLogInfoID] bigint NOT NULL IDENTITY,
    [PasswordHash] nvarchar(60) NOT NULL,
    [PasswordSalt] nvarchar(60) NOT NULL,
    [ConfirmationToken] nvarchar(1000) NULL,
    [TokenGenerationTime] datetimeoffset NULL,
    [PasswordrecoveryToken] nvarchar(1000) NULL,
    [RecoveryTokenTime] datetimeoffset NULL,
    [MaximumLogInAttemp] int NULL,
    [MinimumPasswordAge] int NULL,
    [MaximuPasswordAge] int NULL,
    [UserId] nvarchar(450) NOT NULL,
    [CreatedBy] nvarchar(255) NULL,
    [CreatedDate] datetimeoffset NOT NULL DEFAULT '2023-11-29T07:29:26.2746987+08:00',
    [LastUpdatedBy] nvarchar(255) NULL,
    [LastUpdatedDate] datetimeoffset NOT NULL DEFAULT '2023-11-29T07:29:26.2747650+08:00',
    CONSTRAINT [PK_UserLogInfo] PRIMARY KEY ([UserLogInfoID]),
    CONSTRAINT [FK_UserLogInfo_UserAccount_UserId] FOREIGN KEY ([UserId]) REFERENCES [CBSAP].[UserAccount] ([UserId]) ON DELETE CASCADE
);
GO

CREATE UNIQUE INDEX [IX_UserLogInfo_UserId] ON [CBSAP].[UserLogInfo] ([UserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231128232926_InitialCreateUserInfoTable', N'7.0.12');
GO

COMMIT;
GO

