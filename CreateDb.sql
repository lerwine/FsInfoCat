
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
    CreatedBy NVARCHAR(256) NOT NULL,
    ModifiedOn DATETIME NOT NULL,
    ModifiedBy NVARCHAR(256) NOT NULL,
    LoginName NVARCHAR(32) NOT NULL,
    PwHash NVARCHAR(80) NOT NULL,
    [Role] NVARCHAR(32) NOT NULL,
    IsInactive BIT NOT NULL DEFAULT 0,
    Notes NTEXT NOT NULL DEFAULT '',
    CONSTRAINT PK_RegisteredUser PRIMARY KEY CLUSTERED
    (
        UserID ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

-- Create MediaHost table in the specified schema
CREATE TABLE dbo.MediaHost
(
    HostID UNIQUEIDENTIFIER NOT NULL, -- primary key column
    CreatedOn DATETIME NOT NULL,
    CreatedBy NVARCHAR(256) NOT NULL,
    ModifiedOn DATETIME NOT NULL,
    ModifiedBy NVARCHAR(256) NOT NULL,
    MachineName NVARCHAR(256) NOT NULL,
    IsWindows BIT NOT NULL DEFAULT 0,
    IsInactive BIT NOT NULL DEFAULT 0,
    Notes NTEXT NOT NULL DEFAULT '',
    CONSTRAINT PK_MediaHost PRIMARY KEY CLUSTERED
    (
        HostID ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

-- Create MediaVolume table in the specified schema
CREATE TABLE dbo.MediaVolume
(
    VolumeID UNIQUEIDENTIFIER NOT NULL, -- primary key column
    HostID UNIQUEIDENTIFIER NULL,
    CreatedOn DATETIME NOT NULL,
    CreatedBy NVARCHAR(256) NOT NULL,
    ModifiedOn DATETIME NOT NULL,
    ModifiedBy NVARCHAR(256) NOT NULL,
    RootPathName NVARCHAR(1024) NOT NULL,
    FileSystemName NVARCHAR(128) NOT NULL,
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
DECLARE @CreatedOn DATETIME;
SET @CreatedOn = GETDATE();
INSERT INTO dbo.RegisteredUser (UserID, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, LoginName, PwHash, [Role], IsInactive, Notes)
Values ('c30ac62b-640f-4cda-a8d8-1a700eedd1f4', @CreatedOn, 'admin', @CreatedOn, 'admin', 'admin', 'cbcc0a61e192218d9f395e86648c21c4fb88000a84fb4b752ba823884e35f43ad9d1d55d9644498c', 'Admin', 0, '');
