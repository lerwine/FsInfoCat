CREATE TABLE [dbo].[Account] (
    [AccountID]   UNIQUEIDENTIFIER NOT NULL,
    [DisplayName] NVARCHAR (128)   DEFAULT ('') NOT NULL,
    [LoginName]   NVARCHAR (32)    NOT NULL,
    [Role]        TINYINT          NOT NULL,
    [Notes]       NTEXT            DEFAULT ('') NOT NULL,
    [CreatedOn]   DATETIME         NOT NULL,
    [CreatedBy]   UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn]  DATETIME         NOT NULL,
    [ModifiedBy]  UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED ([AccountID] ASC),
    CONSTRAINT [FK_Account_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Account] ([AccountID]),
    CONSTRAINT [FK_Account_ModifiedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [dbo].[Account] ([AccountID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IDX_Account_LoginName]
    ON [dbo].[Account]([LoginName] ASC);

