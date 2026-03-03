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

BEGIN TRANSACTION;
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[UserLogInfo]') AND [c].[name] = N'PasswordrecoveryToken');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[UserLogInfo] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [CBSAP].[UserLogInfo] ALTER COLUMN [PasswordrecoveryToken] NVARCHAR(1000) NULL;
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[UserLogInfo]') AND [c].[name] = N'PasswordSalt');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[UserLogInfo] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [CBSAP].[UserLogInfo] ALTER COLUMN [PasswordSalt] NVARCHAR(60) NOT NULL;
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[UserLogInfo]') AND [c].[name] = N'PasswordHash');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[UserLogInfo] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [CBSAP].[UserLogInfo] ALTER COLUMN [PasswordHash] NVARCHAR(60) NOT NULL;
GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[UserLogInfo]') AND [c].[name] = N'LastUpdatedDate');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[UserLogInfo] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [CBSAP].[UserLogInfo] ADD DEFAULT '2023-12-04T08:58:12.6647778+08:00' FOR [LastUpdatedDate];
GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[UserLogInfo]') AND [c].[name] = N'CreatedDate');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[UserLogInfo] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [CBSAP].[UserLogInfo] ADD DEFAULT '2023-12-04T08:58:12.6647209+08:00' FOR [CreatedDate];
GO

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[UserLogInfo]') AND [c].[name] = N'ConfirmationToken');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[UserLogInfo] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [CBSAP].[UserLogInfo] ALTER COLUMN [ConfirmationToken] NVARCHAR(1000) NULL;
GO

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[UserAccount]') AND [c].[name] = N'LastUpdatedDate');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[UserAccount] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [CBSAP].[UserAccount] ADD DEFAULT '2023-12-04T08:58:12.6627193+08:00' FOR [LastUpdatedDate];
GO

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[UserAccount]') AND [c].[name] = N'LastName');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[UserAccount] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [CBSAP].[UserAccount] ALTER COLUMN [LastName] NVARCHAR(200) NOT NULL;
GO

DECLARE @var12 sysname;
SELECT @var12 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[UserAccount]') AND [c].[name] = N'FirstName');
IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[UserAccount] DROP CONSTRAINT [' + @var12 + '];');
ALTER TABLE [CBSAP].[UserAccount] ALTER COLUMN [FirstName] NVARCHAR(200) NOT NULL;
GO

DECLARE @var13 sysname;
SELECT @var13 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[UserAccount]') AND [c].[name] = N'EmailAddress');
IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[UserAccount] DROP CONSTRAINT [' + @var13 + '];');
ALTER TABLE [CBSAP].[UserAccount] ALTER COLUMN [EmailAddress] NVARCHAR(200) NOT NULL;
GO

DECLARE @var14 sysname;
SELECT @var14 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[UserAccount]') AND [c].[name] = N'CreatedDate');
IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[UserAccount] DROP CONSTRAINT [' + @var14 + '];');
ALTER TABLE [CBSAP].[UserAccount] ADD DEFAULT '2023-12-04T08:58:12.6626540+08:00' FOR [CreatedDate];
GO

DECLARE @var15 sysname;
SELECT @var15 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[UserAccount]') AND [c].[name] = N'BirthDate');
IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[UserAccount] DROP CONSTRAINT [' + @var15 + '];');
ALTER TABLE [CBSAP].[UserAccount] ALTER COLUMN [BirthDate] Date NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231204005812_UpdateBirthdate', N'7.0.12');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var16 sysname;
SELECT @var16 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[UserLogInfo]') AND [c].[name] = N'LastUpdatedDate');
IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[UserLogInfo] DROP CONSTRAINT [' + @var16 + '];');
ALTER TABLE [CBSAP].[UserLogInfo] ADD DEFAULT '2023-12-04T09:40:09.7907991+08:00' FOR [LastUpdatedDate];
GO

DECLARE @var17 sysname;
SELECT @var17 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[UserLogInfo]') AND [c].[name] = N'CreatedDate');
IF @var17 IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[UserLogInfo] DROP CONSTRAINT [' + @var17 + '];');
ALTER TABLE [CBSAP].[UserLogInfo] ADD DEFAULT '2023-12-04T09:40:09.7907333+08:00' FOR [CreatedDate];
GO

DECLARE @var18 sysname;
SELECT @var18 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[UserAccount]') AND [c].[name] = N'LastUpdatedDate');
IF @var18 IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[UserAccount] DROP CONSTRAINT [' + @var18 + '];');
ALTER TABLE [CBSAP].[UserAccount] ADD DEFAULT '2023-12-04T09:40:09.7889174+08:00' FOR [LastUpdatedDate];
GO

DECLARE @var19 sysname;
SELECT @var19 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CBSAP].[UserAccount]') AND [c].[name] = N'CreatedDate');
IF @var19 IS NOT NULL EXEC(N'ALTER TABLE [CBSAP].[UserAccount] DROP CONSTRAINT [' + @var19 + '];');
ALTER TABLE [CBSAP].[UserAccount] ADD DEFAULT '2023-12-04T09:40:09.7888422+08:00' FOR [CreatedDate];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231204014009_UpdateBdateColumn', N'7.0.12');
GO

COMMIT;
GO

