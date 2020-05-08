DECLARE @isDevEnv bit = 1

IF COL_LENGTH('dbo.[Product]', 'Code') IS NULL
	alter table dbo.[Product] add Code varchar(8) not null

IF COL_LENGTH('dbo.[User]', 'IsUsingCustomPassword') IS NULL
	ALTER TABLE dbo.[User] ADD IsUsingCustomPassword bit NOT NULL DEFAULT 1

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
GO