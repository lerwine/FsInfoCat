CREATE TABLE [dbo].[BasicLogins] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [LoginName]    NVARCHAR (MAX)   NOT NULL,
    [PwHash]       NVARCHAR (MAX)   NOT NULL,
    [FailCount]    TINYINT          NOT NULL,
    [LockedOut]    BIT              NOT NULL,
    [IsInactive]   BIT              NOT NULL,
    [UserId]       UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]    DATETIME         NOT NULL,
    [CreatedById]  UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn]   DATETIME         NOT NULL,
    [ModifiedById] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_BasicLogins] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CreatedByBasicLogin] FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserAccounts] ([Id]),
    CONSTRAINT [FK_ModifiedByBasicLogin] FOREIGN KEY ([ModifiedById]) REFERENCES [dbo].[UserAccounts] ([Id]),
    CONSTRAINT [FK_UserAccountBasicLogin] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[UserAccounts] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_UserAccountBasicLogin]
    ON [dbo].[BasicLogins]([CreatedById] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_CreatedByBasicLogin]
    ON [dbo].[BasicLogins]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_ModifiedByBasicLogin]
    ON [dbo].[BasicLogins]([ModifiedById] ASC);

