CREATE TABLE [dbo].[UserAccounts] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [DisplayName]   NVARCHAR (128)   NOT NULL,
    [FirstName]     NVARCHAR (32)    NOT NULL,
    [LastName]      NVARCHAR (64)    NOT NULL,
    [MI]            NCHAR (1)        NULL,
    [Suffix]        NVARCHAR (32)    NULL,
    [Title]         NVARCHAR (32)    NULL,
    [IsInactive]    BIT              NOT NULL,
    [CreatedOn]     DATETIME         NOT NULL,
    [CreatedById]   UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn]    DATETIME         NOT NULL,
    [ModifiedById]  UNIQUEIDENTIFIER NOT NULL,
    [ExplicitRoles] TINYINT          NOT NULL,
    [Notes]         NVARCHAR (MAX)   NOT NULL,
    [DbPrincipalId] INT              NULL,
    [SID]           VARBINARY (85)   NOT NULL,
    [LoginName]     NVARCHAR (128)   NOT NULL,
    CONSTRAINT [PK_UserAccounts] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CreatedByUserAccount] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[UserAccounts] ([Id]),
    CONSTRAINT [FK_ModifiedByUserAccount] FOREIGN KEY ([ModifiedById]) REFERENCES [dbo].[UserAccounts] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [IX_FK_CreatedByUserAccount]
    ON [dbo].[UserAccounts]([CreatedById] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_ModifiedByUserAccount]
    ON [dbo].[UserAccounts]([ModifiedById] ASC);

