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

IF NOT EXISTS(SELECT 1 FROM sys.tables WHERE name = 'InventionType')
	CREATE TABLE [dbo].[InventionType](
		[Id] uniqueidentifier NOT NULL,
		[Code] varchar(8) NOT NULL,
		[Name] varchar(32) NOT NULL,
		[Description] varchar(128) NOT NULL
	)

IF NOT EXISTS(SELECT 1 FROM sys.tables WHERE name = 'QueueItem')
	CREATE TABLE [dbo].[QueueItem]
	(
		[Id] uniqueidentifier NOT NULL,
		[Type] int NOT NULL,
		[Data] varchar(max) NOT NULL,
		[QueueDate] datetime NOT NULL
	)

COMMIT TRANSACTION