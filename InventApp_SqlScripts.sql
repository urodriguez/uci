BEGIN TRANSACTION

create table dbo.FilterPreferences(
	Id uniqueidentifier not null,
	Days int not null,
	OrderBy varchar(32) not null,
	PageSize int not null,
	Direction varchar(4),
	CONSTRAINT PK_FilterPreferences PRIMARY KEY (Id)
)

alter table dbo.[User] 
add FilterPrefencesId uniqueidentifier

alter table dbo.[User] 
add CONSTRAINT FK_User_FilterPreferences FOREIGN KEY (FilterPrefencesId) REFERENCES [FilterPreferences] (Id)

SELECT Id = NEWID(),  
       UserId = u.Id,
       FilterPreferencesId = NEWID()
INTO #TempUserFilterPreferences
FROM dbo.[User] u
WHERE u.FilterPrefencesId IS NULL

INSERT INTO FilterPreferences
SELECT #TempUserFilterPreferences.FilterPreferencesId, 10, 'date', 50, 'desc'
FROM #TempUserFilterPreferences

UPDATE dbo.[User]
SET FilterPrefencesId = (
	SELECT FilterPreferencesId 
	FROM #TempUserFilterPreferences 
	WHERE #TempUserFilterPreferences.UserId = dbo.[User].Id
)

DROP TABLE #TempUserFilterPreferences

alter table dbo.[User] 
alter column FilterPrefencesId uniqueidentifier not null

COMMIT TRANSACTION


alter table product add Code varchar(8) not null

alter table audit add Entity varchar(max)
alter table audit add EntityId varchar(64)

alter table audit add Environment varchar(16)

IF COL_LENGTH('dbo.[User]', 'IsUsingCustomPassword') IS NULL
	ALTER TABLE dbo.[User] ADD IsUsingCustomPassword bit NOT NULL DEFAULT 1
GO