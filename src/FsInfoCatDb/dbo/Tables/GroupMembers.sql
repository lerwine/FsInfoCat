CREATE TABLE [dbo].[GroupMembers] (
    [GroupId]   UNIQUEIDENTIFIER NOT NULL,
    [UserId]    UNIQUEIDENTIFIER NOT NULL,
    [AddedOn]   DATETIME         NOT NULL,
    [AddedById] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_GroupMembers] PRIMARY KEY CLUSTERED ([GroupId] ASC, [UserId] ASC),
    CONSTRAINT [FK_AddedByGroupMember] FOREIGN KEY ([AddedById]) REFERENCES [dbo].[UserAccounts] ([Id]),
    CONSTRAINT [FK_UserAccountGroupMember] FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserAccounts] ([Id]),
    CONSTRAINT [FK_UserGroupGroupMember] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[UserGroups] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_UserAccountGroupMember]
    ON [dbo].[GroupMembers]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_AddedByGroupMember]
    ON [dbo].[GroupMembers]([AddedById] ASC);

