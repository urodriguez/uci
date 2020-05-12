DECLARE @isDevEnv bit = 1

IF @isDevEnv = 1
	USE [UciRod.Infrastructure.Logging]
ELSE
	USE [UciRod.Infrastructure.Logging-Test]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS(SELECT 1 FROM sys.tables WHERE name = 'User')
	CREATE TABLE [dbo].[User](
		[Id] uniqueidentifier NOT NULL,
		[Name] nvarchar(20) NOT NULL,
		[Password] nvarchar(max) NULL,
		[FirstName] nvarchar(20) NOT NULL,
		[MiddleName] nvarchar(50) NULL,
		[LastName] nvarchar(20) NOT NULL,
		[Email] nvarchar(70) NOT NULL,
		[RoleId] int NOT NULL,
		[DateCreated] datetime NOT NULL,
		[LastLoginTime] datetime NULL,
		[Activate] bit NOT NULL,
		[EmailConfirmed] bit NOT NULL,
		[AccessFailedCount] int NOT NULL,
		[IsUsingCustomPassword] bit NOT NULL
	)

IF COL_LENGTH('dbo.[Product]', 'Code') IS NULL
	alter table dbo.[Product] add Code varchar(8) not null

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