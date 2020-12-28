
IF OBJECT_ID('dbo.Volume', 'U') IS NOT NULL
DROP TABLE dbo.Volume
GO
IF OBJECT_ID('dbo.HostDevice', 'U') IS NOT NULL
DROP TABLE dbo.HostDevice
GO
IF OBJECT_ID('dbo.Account', 'U') IS NOT NULL
DROP TABLE dbo.Account
GO
-- Create Account table in the specified schema
CREATE TABLE dbo.Account
(
    AccountID UNIQUEIDENTIFIER NOT NULL, -- primary key column
    CreatedOn DATETIME NOT NULL,
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    ModifiedOn DATETIME NOT NULL,
    ModifiedBy UNIQUEIDENTIFIER NOT NULL,
    DisplayName NVARCHAR(256) NOT NULL DEFAULT '',
    LoginName NVARCHAR(32) NOT NULL,
    PwHash NCHAR(96) NOT NULL,
    [Role] TINYINT NOT NULL,
    Notes NTEXT NOT NULL DEFAULT '',
    CONSTRAINT PK_Account PRIMARY KEY CLUSTERED
    (
        AccountID ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
DECLARE @CreatedOn DATETIME;
SET @CreatedOn = GETDATE();
INSERT INTO dbo.Account (AccountID, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy,
        DisplayName, LoginName, PwHash, [Role], Notes)
    Values ('00000000-0000-0000-0000-000000000000', @CreatedOn, '00000000-0000-0000-0000-000000000000', @CreatedOn, '00000000-0000-0000-0000-000000000000',
        'FS InfoCat Administrator', 'admin', 'XrVxbJmeSwCzNtQp6PpepBE8WUh1nfhPGlQCBoIOaS6ho6+7UH/lGnJgH0w2hF2oFzxDrQ6liVHWDldtFawAetBTWV+jCsFG', 4, '');
ALTER TABLE dbo.Account WITH CHECK ADD CONSTRAINT FK_Account_CreatedBy FOREIGN KEY(CreatedBy)
    REFERENCES dbo.Account (AccountID)
    ON DELETE NO ACTION;
ALTER TABLE dbo.Account CHECK CONSTRAINT FK_Account_CreatedBy;
ALTER TABLE dbo.Account WITH CHECK ADD CONSTRAINT FK_Account_ModifiedBy FOREIGN KEY(ModifiedBy)
    REFERENCES dbo.Account (AccountID)
    ON DELETE NO ACTION;
ALTER TABLE dbo.Account CHECK CONSTRAINT FK_Account_ModifiedBy;
CREATE UNIQUE CLUSTERED INDEX IDX_Account_LoginName ON dbo.Account (LoginName);

-- Create HostDevice table in the specified schema
CREATE TABLE dbo.HostDevice
(
    HostID UNIQUEIDENTIFIER NOT NULL, -- primary key column
    CreatedOn DATETIME NOT NULL,
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    ModifiedOn DATETIME NOT NULL,
    ModifiedBy UNIQUEIDENTIFIER NOT NULL,
    DisplayName NVARCHAR(256) NOT NULL DEFAULT '',
    MachineIdentifer NVARCHAR(256) NOT NULL,
    MachineName NVARCHAR(256) NOT NULL,
    IsWindows BIT NOT NULL DEFAULT 0,
    IsInactive BIT NOT NULL DEFAULT 0,
    Notes NTEXT NOT NULL DEFAULT '',
    CONSTRAINT PK_HostDevice PRIMARY KEY CLUSTERED
    (
        HostID ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
    CONSTRAINT AK_HostDevice_MachineIdentifier UNIQUE(DisplayName),
    CONSTRAINT AK_HostDevice_MachineIdentifer UNIQUE(MachineIdentifer),
    CONSTRAINT AK_HostDevice_MachineName UNIQUE(MachineName)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
ALTER TABLE dbo.HostDevice WITH CHECK ADD CONSTRAINT FK_HostDevice_CreatedBy FOREIGN KEY(CreatedBy)
    REFERENCES dbo.Account (AccountID)
    ON DELETE NO ACTION;
ALTER TABLE dbo.HostDevice CHECK CONSTRAINT FK_HostDevice_CreatedBy;
ALTER TABLE dbo.HostDevice WITH CHECK ADD CONSTRAINT FK_HostDevice_ModifiedBy FOREIGN KEY(ModifiedBy)
    REFERENCES dbo.Account (AccountID)
    ON DELETE NO ACTION;
ALTER TABLE dbo.HostDevice CHECK CONSTRAINT FK_HostDevice_ModifiedBy;
CREATE UNIQUE CLUSTERED INDEX IDX_HostDevice_MachineIdentifer ON dbo.HostDevice (MachineIdentifer);

-- Create Volume table in the specified schema
CREATE TABLE dbo.Volume
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
    CONSTRAINT PK_Volume PRIMARY KEY CLUSTERED
    (
        VolumeID ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
);
ALTER TABLE dbo.Volume WITH CHECK ADD CONSTRAINT FK_Volume_Host FOREIGN KEY(HostID)
    REFERENCES dbo.HostDevice (HostID)
    ON DELETE CASCADE;
ALTER TABLE dbo.Volume CHECK CONSTRAINT FK_Volume_Host;
ALTER TABLE dbo.Volume WITH CHECK ADD CONSTRAINT FK_Volume_CreatedBy FOREIGN KEY(CreatedBy)
    REFERENCES dbo.Account (AccountID)
    ON DELETE NO ACTION;
ALTER TABLE dbo.Volume CHECK CONSTRAINT FK_Volume_CreatedBy;
ALTER TABLE dbo.Volume WITH CHECK ADD CONSTRAINT FK_Volume_ModifiedBy FOREIGN KEY(ModifiedBy)
    REFERENCES dbo.Account (AccountID)
    ON DELETE NO ACTION;
ALTER TABLE dbo.Volume CHECK CONSTRAINT FK_Volume_ModifiedBy;
CREATE UNIQUE CLUSTERED INDEX IDX_Volume_SerialNumber ON dbo.Volume (SerialNumber);
