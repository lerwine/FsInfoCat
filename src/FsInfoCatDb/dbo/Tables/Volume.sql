CREATE TABLE [dbo].[Volume] (
    [VolumeID]      UNIQUEIDENTIFIER NOT NULL,
    [HostDeviceID]  UNIQUEIDENTIFIER NULL,
    [CreatedOn]     DATETIME         NOT NULL,
    [CreatedBy]     UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn]    DATETIME         NOT NULL,
    [ModifiedBy]    UNIQUEIDENTIFIER NOT NULL,
    [DisplayName]   NVARCHAR (128)   DEFAULT ('') NOT NULL,
    [RootPathName]  NVARCHAR (1024)  NOT NULL,
    [DriveFormat]   NVARCHAR (256)   NOT NULL,
    [VolumeName]    NVARCHAR (128)   NOT NULL,
    [Identifier]    NVARCHAR (1024)  NOT NULL,
    [MaxNameLength] BIGINT           NOT NULL,
    [CaseSensitive] BIT              NOT NULL,
    [IsInactive]    BIT              DEFAULT ((0)) NOT NULL,
    [Notes]         NTEXT            DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Volume] PRIMARY KEY CLUSTERED ([VolumeID] ASC),
    CONSTRAINT [FK_Volume_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Account] ([AccountID]),
    CONSTRAINT [FK_Volume_Host] FOREIGN KEY ([HostDeviceID]) REFERENCES [dbo].[HostDevice] ([HostDeviceID]) ON DELETE CASCADE,
    CONSTRAINT [FK_Volume_ModifiedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [dbo].[Account] ([AccountID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IDX_Volume_Identifier]
    ON [dbo].[Volume]([Identifier] ASC);

