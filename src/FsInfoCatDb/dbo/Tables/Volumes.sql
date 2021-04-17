CREATE TABLE [dbo].[Volumes] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [HostDeviceId]  UNIQUEIDENTIFIER NULL,
    [DisplayName]   NVARCHAR (MAX)   NOT NULL,
    [RootPathName]  NVARCHAR (MAX)   NOT NULL,
    [DriveFormat]   NVARCHAR (MAX)   NOT NULL,
    [VolumeName]    NVARCHAR (MAX)   NOT NULL,
    [Identifier]    NVARCHAR (MAX)   NOT NULL,
    [MaxNameLength] BIGINT           NOT NULL,
    [CaseSensitive] BIT              NOT NULL,
    [IsInactive]    NVARCHAR (MAX)   NOT NULL,
    [CreatedOn]     DATETIME         NOT NULL,
    [CreatedById]   UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn]    DATETIME         NOT NULL,
    [ModifiedById]  UNIQUEIDENTIFIER NOT NULL,
    [Notes]         NVARCHAR (MAX)   NOT NULL,
    CONSTRAINT [PK_Volumes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CreatedByVolume] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[UserAccounts] ([Id]),
    CONSTRAINT [FK_HostDeviceVolume] FOREIGN KEY ([HostDeviceId]) REFERENCES [dbo].[HostDevices] ([Id]),
    CONSTRAINT [FK_ModifiedByVolume] FOREIGN KEY ([ModifiedById]) REFERENCES [dbo].[UserAccounts] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_CreatedByVolume]
    ON [dbo].[Volumes]([CreatedById] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_ModifiedByVolume]
    ON [dbo].[Volumes]([ModifiedById] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_HostDeviceVolume]
    ON [dbo].[Volumes]([HostDeviceId] ASC);

