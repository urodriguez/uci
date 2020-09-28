SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF DB_ID('UciRod.Inventapp') IS NULL 
	CREATE DATABASE [UciRod.Inventapp]
GO

IF DB_ID('UciRod.Inventapp.Hangfire') IS NULL 
	CREATE DATABASE [UciRod.Inventapp.Hangfire]
GO

USE [UciRod.Inventapp]

BEGIN TRANSACTION

IF NOT EXISTS(SELECT 1 FROM sys.server_principals WHERE name = 'ucirod-inventapp')
begin
	CREATE LOGIN [ucirod-inventapp] WITH PASSWORD=N'Uc1R0d-1nv3nt4pp', DEFAULT_DATABASE=[master], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
end

IF NOT EXISTS(SELECT 1 FROM sys.database_principals WHERE name = 'ucirod-inventapp')
begin
	CREATE USER [ucirod-inventapp] FOR LOGIN [ucirod-inventapp]
	ALTER USER [ucirod-inventapp] WITH DEFAULT_SCHEMA=[dbo]
	ALTER ROLE [db_owner] ADD MEMBER [ucirod-inventapp]
end

IF NOT EXISTS(SELECT 1 FROM sys.tables WHERE name = 'User')
begin
	CREATE TABLE [dbo].[User](
		[Id] uniqueidentifier NOT NULL,
		[Email] varchar(64) NOT NULL,
		[Password] varchar(8) NULL,
		[FirstName] varchar(32) NOT NULL,
		[MiddleName] varchar(32) NULL,
		[LastName] varchar(32) NOT NULL,
		[Role] int NOT NULL,
		[DateCreated] datetime NOT NULL,
		[LastLoginTime] datetime NULL,
		[Active] bit NOT NULL,
		[EmailConfirmed] bit NOT NULL,
		[AccessFailedCount] int NOT NULL,
		[IsUsingCustomPassword] bit NOT NULL
	)

	INSERT INTO [dbo].[User]
	VALUES ('6fe0ddd8-81b3-42fe-bf0d-455422e0b7a3', 'inventApp@gmail.com', 'Pa$$w0rd', 'Invent', '', 'App', 1, GETDATE(), null, 1, 1, 0, 1)
end

IF NOT EXISTS(SELECT 1 FROM sys.tables WHERE name = 'Invention')
	CREATE TABLE [dbo].[Invention](
		[Id] uniqueidentifier NOT NULL,
		[Code] varchar(8) NOT NULL,
		[Name] varchar(32) NOT NULL,
		[Category] char(1) NOT NULL,
		[Price] decimal(8, 2) NOT NULL
	)	

IF NOT EXISTS(SELECT 1 FROM sys.tables WHERE name = 'InventionCategory')
	CREATE TABLE [dbo].[InventionCategory](
		[Id] uniqueidentifier NOT NULL,
		[Code] varchar(8) NOT NULL,
		[Name] varchar(32) NOT NULL,
		[Description] varchar(128)
	)

IF NOT EXISTS(SELECT 1 FROM sys.tables WHERE name = 'QueueItem')
	CREATE TABLE [dbo].[QueueItem]
	(
		[Id] uniqueidentifier NOT NULL,
		[Type] int NOT NULL,
		[Data] varchar(max) NOT NULL,
		[QueueDate] datetime NOT NULL
	)

IF NOT EXISTS(SELECT 1 FROM sys.objects where parent_object_id = OBJECT_ID('User') and name = 'PK_User')
	ALTER TABLE [dbo].[User] ADD CONSTRAINT PK_User PRIMARY KEY (Id)

IF NOT EXISTS(SELECT 1 FROM sys.objects where parent_object_id = OBJECT_ID('Invention') and name = 'PK_Invention')
	ALTER TABLE [dbo].[Invention] ADD CONSTRAINT PK_Invention PRIMARY KEY (Id)

IF NOT EXISTS(SELECT 1 FROM sys.objects where parent_object_id = OBJECT_ID('InventionCategory') and name = 'PK_InventionCategory')
	ALTER TABLE [dbo].[InventionCategory] ADD CONSTRAINT PK_InventionCategory PRIMARY KEY (Id)

IF NOT EXISTS(SELECT 1 FROM sys.objects where parent_object_id = OBJECT_ID('QueueItem') and name = 'PK_QueueItem')
	ALTER TABLE [dbo].[QueueItem] ADD CONSTRAINT PK_QueueItem PRIMARY KEY (Id)

IF COL_LENGTH('dbo.Invention', 'UserId') IS NULL
	ALTER TABLE [dbo].[Invention] ADD [UserId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_Invention_User FOREIGN KEY([UserId]) REFERENCES [dbo].[User] (Id) ON DELETE CASCADE

IF COL_LENGTH('dbo.Invention', 'Category') IS NOT NULL
	ALTER TABLE [dbo].[Invention] DROP COLUMN [Category]

IF COL_LENGTH('dbo.Invention', 'CategoryId') IS NULL
	ALTER TABLE [dbo].[Invention] ADD [CategoryId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_Invention_Category FOREIGN KEY([CategoryId]) REFERENCES [dbo].[InventionCategory] (Id) ON DELETE CASCADE

IF COL_LENGTH('dbo.Invention', 'Description') IS NULL
	ALTER TABLE [dbo].[Invention] ADD [Description] varchar(128)

IF COL_LENGTH('dbo.Invention', 'Enable') IS NULL
	ALTER TABLE [dbo].[Invention] ADD [Enable] bit not null default 1

COMMIT TRANSACTION