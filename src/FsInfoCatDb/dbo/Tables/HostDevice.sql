CREATE TABLE [dbo].[HostDevice] (
    [HostDeviceID]     UNIQUEIDENTIFIER NOT NULL,
    [DisplayName]      NVARCHAR (128)   DEFAULT ('') NOT NULL,
    [MachineIdentifer] NVARCHAR (128)   NOT NULL,
    [MachineName]      NVARCHAR (128)   NOT NULL,
    [Platform]         TINYINT          DEFAULT ((0)) NOT NULL,
    [AllowCrawl]       BIT              DEFAULT ((0)) NOT NULL,
    [IsInactive]       BIT              DEFAULT ((0)) NOT NULL,
    [Notes]            NTEXT            DEFAULT ('') NOT NULL,
    [CreatedOn]        DATETIME         NOT NULL,
    [CreatedBy]        UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn]       DATETIME         NOT NULL,
    [ModifiedBy]       UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_HostDevice] PRIMARY KEY CLUSTERED ([HostDeviceID] ASC),
    CONSTRAINT [FK_HostDevice_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Account] ([AccountID]),
    CONSTRAINT [FK_HostDevice_ModifiedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [dbo].[Account] ([AccountID]),
    CONSTRAINT [AK_HostDevice_MachineIdentifer] UNIQUE NONCLUSTERED ([MachineIdentifer] ASC),
    CONSTRAINT [AK_HostDevice_MachineIdentifier] UNIQUE NONCLUSTERED ([DisplayName] ASC),
    CONSTRAINT [AK_HostDevice_MachineName] UNIQUE NONCLUSTERED ([MachineName] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IDX_HostDevice_MachineIdentifer]
    ON [dbo].[HostDevice]([MachineIdentifer] ASC);

