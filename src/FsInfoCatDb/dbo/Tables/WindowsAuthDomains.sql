CREATE TABLE [dbo].[WindowsAuthDomains] (
    [Id]                    UNIQUEIDENTIFIER NOT NULL,
    [SID]                   NVARCHAR (MAX)   NOT NULL,
    [Name]                  NVARCHAR (MAX)   NOT NULL,
    [IsInactive]            BIT              NOT NULL,
    [CreatedOn]             DATETIME         NOT NULL,
    [CreatedById]           UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn]            DATETIME         NOT NULL,
    [ModifiedById]          UNIQUEIDENTIFIER NOT NULL,
    [Notes]                 NVARCHAR (MAX)   NOT NULL,
    [AutoAddUsers]          BIT              NOT NULL,
    [DefaultNewUserGroupId] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_WindowsAuthDomains] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CreatedByWindowsAuthDomain] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[UserAccounts] ([Id]),
    CONSTRAINT [FK_DefaultNewUserGroupWindowsAuthDomain] FOREIGN KEY ([DefaultNewUserGroupId]) REFERENCES [dbo].[UserGroups] ([Id]),
    CONSTRAINT [FK_ModifiedByWindowsAuthDomain] FOREIGN KEY ([ModifiedById]) REFERENCES [dbo].[UserAccounts] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_CreatedByWindowsAuthDomain]
    ON [dbo].[WindowsAuthDomains]([CreatedById] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_ModifiedByWindowsAuthDomain]
    ON [dbo].[WindowsAuthDomains]([ModifiedById] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_DefaultNewUserGroupWindowsAuthDomain]
    ON [dbo].[WindowsAuthDomains]([DefaultNewUserGroupId] ASC);

