
IF OBJECT_ID('dbo.MediaVolume', 'U') IS NOT NULL
DROP TABLE dbo.MediaVolume
GO
IF OBJECT_ID('dbo.MediaHost', 'U') IS NOT NULL
DROP TABLE dbo.MediaHost
GO
IF OBJECT_ID('dbo.RegisteredUser', 'U') IS NOT NULL
DROP TABLE dbo.RegisteredUser
GO
-- Create MediaHost table in the specified schema
CREATE TABLE dbo.RegisteredUser
(
    UserID UNIQUEIDENTIFIER NOT NULL, -- primary key column
    CreatedOn DATETIME NOT NULL,
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    ModifiedOn DATETIME NOT NULL,
    ModifiedBy UNIQUEIDENTIFIER NOT NULL,
    DisplayName NVARCHAR(256) NOT NULL DEFAULT '',
    LoginName NVARCHAR(32) NOT NULL,
    PwHash NCHAR(96) NOT NULL,
    [Role] TINYINT NOT NULL,
    Notes NTEXT NOT NULL DEFAULT '',
    CONSTRAINT PK_RegisteredUser PRIMARY KEY CLUSTERED
    (
        UserID ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
DECLARE @CreatedOn DATETIME;
SET @CreatedOn = GETDATE();
INSERT INTO dbo.RegisteredUser (UserID, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy,
        DisplayName, LoginName, PwHash, [Role], Notes)
    Values ('00000000-0000-0000-0000-000000000000', @CreatedOn, '00000000-0000-0000-0000-000000000000', @CreatedOn, '00000000-0000-0000-0000-000000000000',
        'FS InfoCat Administrator', 'admin', 'XrVxbJmeSwCzNtQp6PpepBE8WUh1nfhPGlQCBoIOaS6ho6+7UH/lGnJgH0w2hF2oFzxDrQ6liVHWDldtFawAetBTWV+jCsFG', 4, '');
ALTER TABLE dbo.RegisteredUser WITH CHECK ADD CONSTRAINT FK_RegisteredUser_CreatedBy FOREIGN KEY(CreatedBy)
    REFERENCES dbo.RegisteredUser (UserID)
    ON DELETE NO ACTION;
ALTER TABLE dbo.RegisteredUser CHECK CONSTRAINT FK_RegisteredUser_CreatedBy;
ALTER TABLE dbo.RegisteredUser WITH CHECK ADD CONSTRAINT FK_RegisteredUser_ModifiedBy FOREIGN KEY(ModifiedBy)
    REFERENCES dbo.RegisteredUser (UserID)
    ON DELETE NO ACTION;
ALTER TABLE dbo.RegisteredUser CHECK CONSTRAINT FK_RegisteredUser_ModifiedBy;

-- Create MediaHost table in the specified schema
CREATE TABLE dbo.MediaHost
(
    HostID UNIQUEIDENTIFIER NOT NULL, -- primary key column
    CreatedOn DATETIME NOT NULL,
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    ModifiedOn DATETIME NOT NULL,
    ModifiedBy UNIQUEIDENTIFIER NOT NULL,
    DisplayName NVARCHAR(256) NOT NULL DEFAULT '',
    MachineName NVARCHAR(256) NOT NULL,
    IsWindows BIT NOT NULL DEFAULT 0,
    IsInactive BIT NOT NULL DEFAULT 0,
    Notes NTEXT NOT NULL DEFAULT '',
    CONSTRAINT PK_MediaHost PRIMARY KEY CLUSTERED
    (
        HostID ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
ALTER TABLE dbo.MediaHost WITH CHECK ADD CONSTRAINT FK_MediaHost_CreatedBy FOREIGN KEY(CreatedBy)
    REFERENCES dbo.RegisteredUser (UserID)
    ON DELETE NO ACTION;
ALTER TABLE dbo.MediaHost CHECK CONSTRAINT FK_MediaHost_CreatedBy;
ALTER TABLE dbo.MediaHost WITH CHECK ADD CONSTRAINT FK_MediaHost_ModifiedBy FOREIGN KEY(ModifiedBy)
    REFERENCES dbo.RegisteredUser (UserID)
    ON DELETE NO ACTION;
ALTER TABLE dbo.MediaHost CHECK CONSTRAINT FK_MediaHost_ModifiedBy;

-- Create MediaVolume table in the specified schema
CREATE TABLE dbo.MediaVolume
(
    VolumeID UNIQUEIDENTIFIER NOT NULL, -- primary key column
    HostID UNIQUEIDENTIFIER NULL,
    CreatedOn DATETIME NOT NULL,
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    ModifiedOn DATETIME NOT NULL,
    ModifiedBy UNIQUEIDENTIFIER NOT NULL,
    DisplayName NVARCHAR(256) NOT NULL DEFAULT '',
    RootPathName NVARCHAR(1024) NOT NULL,
    FileSystemName NVARCHAR(256) NOT NULL,
    VolumeName NVARCHAR(128) NOT NULL,
    SerialNumber BIGINT NOT NULL,
    MaxNameLength BIGINT NOT NULL,
    Flags INT NOT NULL,
    IsInactive BIT NOT NULL DEFAULT 0,
    Notes NTEXT NOT NULL DEFAULT '',
    CONSTRAINT PK_MediaVolume PRIMARY KEY CLUSTERED
    (
        VolumeID ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
);
ALTER TABLE dbo.MediaVolume WITH CHECK ADD CONSTRAINT FK_MediaVolume_Host FOREIGN KEY(HostID)
    REFERENCES dbo.MediaHost (HostID)
    ON DELETE CASCADE;
ALTER TABLE dbo.MediaVolume CHECK CONSTRAINT FK_MediaVolume_Host;
ALTER TABLE dbo.MediaVolume WITH CHECK ADD CONSTRAINT FK_MediaVolume_CreatedBy FOREIGN KEY(CreatedBy)
    REFERENCES dbo.RegisteredUser (UserID)
    ON DELETE NO ACTION;
ALTER TABLE dbo.MediaVolume CHECK CONSTRAINT FK_MediaVolume_CreatedBy;
ALTER TABLE dbo.MediaVolume WITH CHECK ADD CONSTRAINT FK_MediaVolume_ModifiedBy FOREIGN KEY(ModifiedBy)
    REFERENCES dbo.RegisteredUser (UserID)
    ON DELETE NO ACTION;
ALTER TABLE dbo.MediaVolume CHECK CONSTRAINT FK_MediaVolume_ModifiedBy;
