CREATE TABLE [dbo].[HostDevices] (
    [Id]               UNIQUEIDENTIFIER NOT NULL,
    [DisplayName]      NVARCHAR (128)   NOT NULL,
    [MachineIdentifer] NVARCHAR (128)   NOT NULL,
    [MachineName]      NVARCHAR (128)   NOT NULL,
    [CreatedOn]        DATETIME         NOT NULL,
    [CreatedById]      UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn]       DATETIME         NOT NULL,
    [ModifiedById]     UNIQUEIDENTIFIER NOT NULL,
    [Notes]            NVARCHAR (MAX)   NOT NULL,
    [Platform]         TINYINT          NOT NULL,
    CONSTRAINT [PK_HostDevices] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CreatedByHostDevice] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[UserAccounts] ([Id]),
    CONSTRAINT [FK_ModifiedByHostDevice] FOREIGN KEY ([ModifiedById]) REFERENCES [dbo].[UserAccounts] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_CreatedByHostDevice]
    ON [dbo].[HostDevices]([CreatedById] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_ModifiedByHostDevice]
    ON [dbo].[HostDevices]([ModifiedById] ASC);

