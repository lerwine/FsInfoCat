CREATE TABLE [dbo].[WindowsGroupIdentities] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [SID]            NVARCHAR (MAX)   NOT NULL,
    [DomainId]       UNIQUEIDENTIFIER NOT NULL,
    [Name]           NVARCHAR (MAX)   NOT NULL,
    [IsInactive]     BIT              NOT NULL,
    [GroupId]        UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]      DATETIME         NOT NULL,
    [CreatedById]    UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn]     DATETIME         NOT NULL,
    [ModifiedById]   UNIQUEIDENTIFIER NOT NULL,
    [Notes]          NVARCHAR (MAX)   NOT NULL,
    [AutoAddMembers] BIT              NOT NULL,
    CONSTRAINT [PK_WindowsGroupIdentities] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CreatedByUserAccountWindowsGroupIdentity] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[UserAccounts] ([Id]),
    CONSTRAINT [FK_ModifiedByWindowsGroupIdentity] FOREIGN KEY ([ModifiedById]) REFERENCES [dbo].[UserAccounts] ([Id]),
    CONSTRAINT [FK_UserGroupWindowsGroupIdentity] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[UserGroups] ([Id]),
    CONSTRAINT [FK_WindowsAuthDomainWindowsGroupIdentity] FOREIGN KEY ([DomainId]) REFERENCES [dbo].[WindowsAuthDomains] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_UserGroupWindowsGroupIdentity]
    ON [dbo].[WindowsGroupIdentities]([GroupId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_WindowsAuthDomainWindowsGroupIdentity]
    ON [dbo].[WindowsGroupIdentities]([DomainId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_CreatedByUserAccountWindowsGroupIdentity]
    ON [dbo].[WindowsGroupIdentities]([CreatedById] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_ModifiedByWindowsGroupIdentity]
    ON [dbo].[WindowsGroupIdentities]([ModifiedById] ASC);

