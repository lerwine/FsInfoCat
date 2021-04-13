CREATE TABLE [dbo].[UserCredential] (
    [AccountID]  UNIQUEIDENTIFIER NOT NULL,
    [PwHash]     NCHAR (96)       NOT NULL,
    [CreatedOn]  DATETIME         NOT NULL,
    [CreatedBy]  UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn] DATETIME         NOT NULL,
    [ModifiedBy] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_UserCredential] PRIMARY KEY CLUSTERED ([AccountID] ASC),
    CONSTRAINT [FK_UserCredential_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Account] ([AccountID]),
    CONSTRAINT [FK_UserCredential_ModifiedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [dbo].[Account] ([AccountID])
);

