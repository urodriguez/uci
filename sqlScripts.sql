DECLARE @isDevEnv bit = 1

IF @isDevEnv = 1
	USE [UciRod.Inventapp]
ELSE
	USE [UciRod.Inventapp-Test]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

BEGIN TRANSACTION

IF NOT EXISTS(SELECT 1 FROM sys.tables WHERE name = 'User')
begin
	CREATE TABLE [dbo].[User](
		[Id] uniqueidentifier NOT NULL,
		[Name] nvarchar(32) NOT NULL,
		[Password] nvarchar(8) NULL,
		[FirstName] nvarchar(32) NOT NULL,
		[MiddleName] nvarchar(32) NULL,
		[LastName] nvarchar(32) NOT NULL,
		[Email] nvarchar(64) NOT NULL,
		[Role] int NOT NULL,
		[DateCreated] datetime NOT NULL,
		[LastLoginTime] datetime NULL,
		[Activate] bit NOT NULL,
		[EmailConfirmed] bit NOT NULL,
		[AccessFailedCount] int NOT NULL,
		[IsUsingCustomPassword] bit NOT NULL
	)

	INSERT INTO [dbo].[User]
	VALUES ('6fe0ddd8-81b3-42fe-bf0d-455422e0b7a3', 'inventApp-admin', 'Pa$$w0rd', 'Invent', '', 'App', 'inventApp@gmail.com', 1, GETDATE(), null, 1, 1, 0, 1)
end

IF NOT EXISTS(SELECT 1 FROM sys.tables WHERE name = 'Invention')
	CREATE TABLE [dbo].[Invention](
		[Id] [uniqueidentifier] NOT NULL,
		[Code] [nvarchar](8) NOT NULL,
		[Name] [nvarchar](32) NOT NULL,
		[Category] [char](1) NOT NULL,
		[Price] [decimal](8, 2) NOT NULL
	)	

IF NOT EXISTS(SELECT 1 FROM sys.tables WHERE name = 'InventionType')
	CREATE TABLE [dbo].[InventionType](
		[Id] [uniqueidentifier] NOT NULL,
		[Code] [varchar](8) NOT NULL,
		[Name] [varchar](64) NOT NULL,
		[Description] [varchar](128) NOT NULL
	)

IF @isDevEnv = 1
begin
	IF DB_ID('UciRod.Inventapp.Hangfire') IS NULL 
		CREATE DATABASE [UciRod.Inventapp.Hangfire]
end
ELSE 
begin
	IF DB_ID('UciRod.Inventapp.Hangfire-Test') IS NULL 
		CREATE DATABASE [UciRod.Inventapp.Hangfire-Test]
end

IF NOT EXISTS(SELECT 1 FROM sys.tables WHERE name = 'QueueItem')
BEGIN
	CREATE TABLE dbo.QueueItem
	(
		Id uniqueidentifier NOT NULL,
		Type int NOT NULL,
		Data nvarchar(max) NOT NULL,
		QueueDate datetime NOT NULL
	)
END

COMMIT TRANSACTION