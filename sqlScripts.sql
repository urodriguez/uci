IF COL_LENGTH('dbo.[Product]', 'Code') IS NULL
	alter table dbo.[Product] add Code varchar(8) not null
GO

IF COL_LENGTH('dbo.[User]', 'IsUsingCustomPassword') IS NULL
	ALTER TABLE dbo.[User] ADD IsUsingCustomPassword bit NOT NULL DEFAULT 1
GO