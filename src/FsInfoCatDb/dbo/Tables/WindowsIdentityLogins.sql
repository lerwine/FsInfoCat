CREATE TABLE [dbo].[WindowsIdentityLogins] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [SID]          NVARCHAR (MAX)   NOT NULL,
    [DomainId]     UNIQUEIDENTIFIER NOT NULL,
    [AccountName]  NVARCHAR (MAX)   NOT NULL,
    [IsInactive]   BIT              NOT NULL,
    [UserId]       UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]    DATETIME         NOT NULL,
    [CreatedById]  UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn]   DATETIME         NOT NULL,
    [ModifiedById] UNIQUEIDENTIFIER NOT NULL,
    [Notes]        NVARCHAR (MAX)   NOT NULL,
    CONSTRAINT [PK_WindowsIdentityLogins] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CreatedByWindowsIdentityLogin] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[UserAccounts] ([Id]),
    CONSTRAINT [FK_ModifiedByWindowsIdentityLogin] FOREIGN KEY ([ModifiedById]) REFERENCES [dbo].[UserAccounts] ([Id]),
    CONSTRAINT [FK_UserAccountWindowsIdentityLogin] FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserAccounts] ([Id]),
    CONSTRAINT [FK_WindowsAuthDomainWindowsIdentityLogin] FOREIGN KEY ([DomainId]) REFERENCES [dbo].[WindowsAuthDomains] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_WindowsAuthDomainWindowsIdentityLogin]
    ON [dbo].[WindowsIdentityLogins]([DomainId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_UserAccountWindowsIdentityLogin]
    ON [dbo].[WindowsIdentityLogins]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_CreatedByWindowsIdentityLogin]
    ON [dbo].[WindowsIdentityLogins]([CreatedById] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_ModifiedByWindowsIdentityLogin]
    ON [dbo].[WindowsIdentityLogins]([ModifiedById] ASC);

