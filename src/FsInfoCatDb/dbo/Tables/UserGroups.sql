CREATE TABLE [dbo].[UserGroups] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [DisplayName]  NVARCHAR (128)   NOT NULL,
    [IsInactive]   BIT              NOT NULL,
    [CreatedOn]    DATETIME         NOT NULL,
    [CreatedById]  UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn]   DATETIME         NOT NULL,
    [ModifiedById] UNIQUEIDENTIFIER NOT NULL,
    [Roles]        TINYINT          NOT NULL,
    [Notes]        NVARCHAR (MAX)   NOT NULL,
    CONSTRAINT [PK_UserGroups] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CreatedByUserGroup] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[UserAccounts] ([Id]),
    CONSTRAINT [FK_ModifiedByUserGroup] FOREIGN KEY ([ModifiedById]) REFERENCES [dbo].[UserAccounts] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_CreatedByUserGroup]
    ON [dbo].[UserGroups]([CreatedById] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_ModifiedByUserGroup]
    ON [dbo].[UserGroups]([ModifiedById] ASC);

