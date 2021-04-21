
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/20/2021 22:40:05
-- Generated from EDMX file: C:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat.Desktop\Model\DbModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [FsInfoCat];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_AddedByGroupMember]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GroupMembers] DROP CONSTRAINT [FK_AddedByGroupMember];
GO
IF OBJECT_ID(N'[dbo].[FK_CreatedByHostDevice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HostDevices] DROP CONSTRAINT [FK_CreatedByHostDevice];
GO
IF OBJECT_ID(N'[dbo].[FK_CreatedByUserAccount]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserAccounts] DROP CONSTRAINT [FK_CreatedByUserAccount];
GO
IF OBJECT_ID(N'[dbo].[FK_CreatedByUserGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserGroups] DROP CONSTRAINT [FK_CreatedByUserGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_CreatedByVolume]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Volumes] DROP CONSTRAINT [FK_CreatedByVolume];
GO
IF OBJECT_ID(N'[dbo].[FK_HostDeviceVolume]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Volumes] DROP CONSTRAINT [FK_HostDeviceVolume];
GO
IF OBJECT_ID(N'[dbo].[FK_ModifiedByHostDevice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HostDevices] DROP CONSTRAINT [FK_ModifiedByHostDevice];
GO
IF OBJECT_ID(N'[dbo].[FK_ModifiedByUserAccount]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserAccounts] DROP CONSTRAINT [FK_ModifiedByUserAccount];
GO
IF OBJECT_ID(N'[dbo].[FK_ModifiedByUserGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserGroups] DROP CONSTRAINT [FK_ModifiedByUserGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_ModifiedByVolume]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Volumes] DROP CONSTRAINT [FK_ModifiedByVolume];
GO
IF OBJECT_ID(N'[dbo].[FK_UserAccountGroupMember]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GroupMembers] DROP CONSTRAINT [FK_UserAccountGroupMember];
GO
IF OBJECT_ID(N'[dbo].[FK_UserGroupGroupMember]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GroupMembers] DROP CONSTRAINT [FK_UserGroupGroupMember];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[GroupMembers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GroupMembers];
GO
IF OBJECT_ID(N'[dbo].[HostDevices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HostDevices];
GO
IF OBJECT_ID(N'[dbo].[UserAccounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserAccounts];
GO
IF OBJECT_ID(N'[dbo].[UserGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserGroups];
GO
IF OBJECT_ID(N'[dbo].[Volumes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Volumes];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UserAccounts'
CREATE TABLE [dbo].[UserAccounts] (
    [Id] uniqueidentifier  NOT NULL,
    [DisplayName] nvarchar(128)  NOT NULL,
    [FirstName] nvarchar(32)  NOT NULL,
    [LastName] nvarchar(64)  NOT NULL,
    [MI] nchar(1)  NULL,
    [Suffix] nvarchar(32)  NULL,
    [Title] nvarchar(32)  NULL,
    [IsInactive] bit  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ExplicitRoles] tinyint  NOT NULL,
    [Notes] nvarchar(max)  NOT NULL,
    [DbPrincipalId] int  NULL,
    [SID] varbinary(85)  NOT NULL,
    [LoginName] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'HostDevices'
CREATE TABLE [dbo].[HostDevices] (
    [Id] uniqueidentifier  NOT NULL,
    [DisplayName] nvarchar(128)  NOT NULL,
    [MachineIdentifer] nvarchar(128)  NOT NULL,
    [MachineName] nvarchar(128)  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [Notes] nvarchar(max)  NOT NULL,
    [Platform] tinyint  NOT NULL
);
GO

-- Creating table 'Volumes'
CREATE TABLE [dbo].[Volumes] (
    [Id] uniqueidentifier  NOT NULL,
    [HostDeviceId] uniqueidentifier  NULL,
    [DisplayName] nvarchar(max)  NOT NULL,
    [RootPathName] nvarchar(max)  NOT NULL,
    [DriveFormat] nvarchar(max)  NOT NULL,
    [VolumeName] nvarchar(max)  NOT NULL,
    [Identifier] nvarchar(max)  NOT NULL,
    [MaxNameLength] bigint  NOT NULL,
    [CaseSensitive] bit  NOT NULL,
    [IsInactive] nvarchar(max)  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [Notes] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'UserGroups'
CREATE TABLE [dbo].[UserGroups] (
    [Id] uniqueidentifier  NOT NULL,
    [DisplayName] nvarchar(128)  NOT NULL,
    [IsInactive] bit  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [Roles] tinyint  NOT NULL,
    [Notes] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'GroupMembers'
CREATE TABLE [dbo].[GroupMembers] (
    [GroupId] uniqueidentifier  NOT NULL,
    [UserId] uniqueidentifier  NOT NULL,
    [AddedOn] datetime  NOT NULL,
    [AddedById] uniqueidentifier  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'UserAccounts'
ALTER TABLE [dbo].[UserAccounts]
ADD CONSTRAINT [PK_UserAccounts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HostDevices'
ALTER TABLE [dbo].[HostDevices]
ADD CONSTRAINT [PK_HostDevices]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Volumes'
ALTER TABLE [dbo].[Volumes]
ADD CONSTRAINT [PK_Volumes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserGroups'
ALTER TABLE [dbo].[UserGroups]
ADD CONSTRAINT [PK_UserGroups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [GroupId], [UserId] in table 'GroupMembers'
ALTER TABLE [dbo].[GroupMembers]
ADD CONSTRAINT [PK_GroupMembers]
    PRIMARY KEY CLUSTERED ([GroupId], [UserId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CreatedById] in table 'HostDevices'
ALTER TABLE [dbo].[HostDevices]
ADD CONSTRAINT [FK_CreatedByHostDevice]
    FOREIGN KEY ([CreatedById])
    REFERENCES [dbo].[UserAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByHostDevice'
CREATE INDEX [IX_FK_CreatedByHostDevice]
ON [dbo].[HostDevices]
    ([CreatedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'HostDevices'
ALTER TABLE [dbo].[HostDevices]
ADD CONSTRAINT [FK_ModifiedByHostDevice]
    FOREIGN KEY ([ModifiedById])
    REFERENCES [dbo].[UserAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByHostDevice'
CREATE INDEX [IX_FK_ModifiedByHostDevice]
ON [dbo].[HostDevices]
    ([ModifiedById]);
GO

-- Creating foreign key on [CreatedById] in table 'UserAccounts'
ALTER TABLE [dbo].[UserAccounts]
ADD CONSTRAINT [FK_CreatedByUserAccount]
    FOREIGN KEY ([CreatedById])
    REFERENCES [dbo].[UserAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByUserAccount'
CREATE INDEX [IX_FK_CreatedByUserAccount]
ON [dbo].[UserAccounts]
    ([CreatedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'UserAccounts'
ALTER TABLE [dbo].[UserAccounts]
ADD CONSTRAINT [FK_ModifiedByUserAccount]
    FOREIGN KEY ([ModifiedById])
    REFERENCES [dbo].[UserAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByUserAccount'
CREATE INDEX [IX_FK_ModifiedByUserAccount]
ON [dbo].[UserAccounts]
    ([ModifiedById]);
GO

-- Creating foreign key on [CreatedById] in table 'Volumes'
ALTER TABLE [dbo].[Volumes]
ADD CONSTRAINT [FK_CreatedByVolume]
    FOREIGN KEY ([CreatedById])
    REFERENCES [dbo].[UserAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByVolume'
CREATE INDEX [IX_FK_CreatedByVolume]
ON [dbo].[Volumes]
    ([CreatedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'Volumes'
ALTER TABLE [dbo].[Volumes]
ADD CONSTRAINT [FK_ModifiedByVolume]
    FOREIGN KEY ([ModifiedById])
    REFERENCES [dbo].[UserAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByVolume'
CREATE INDEX [IX_FK_ModifiedByVolume]
ON [dbo].[Volumes]
    ([ModifiedById]);
GO

-- Creating foreign key on [HostDeviceId] in table 'Volumes'
ALTER TABLE [dbo].[Volumes]
ADD CONSTRAINT [FK_HostDeviceVolume]
    FOREIGN KEY ([HostDeviceId])
    REFERENCES [dbo].[HostDevices]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HostDeviceVolume'
CREATE INDEX [IX_FK_HostDeviceVolume]
ON [dbo].[Volumes]
    ([HostDeviceId]);
GO

-- Creating foreign key on [GroupId] in table 'GroupMembers'
ALTER TABLE [dbo].[GroupMembers]
ADD CONSTRAINT [FK_UserGroupGroupMember]
    FOREIGN KEY ([GroupId])
    REFERENCES [dbo].[UserGroups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [UserId] in table 'GroupMembers'
ALTER TABLE [dbo].[GroupMembers]
ADD CONSTRAINT [FK_UserAccountGroupMember]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[UserAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserAccountGroupMember'
CREATE INDEX [IX_FK_UserAccountGroupMember]
ON [dbo].[GroupMembers]
    ([UserId]);
GO

-- Creating foreign key on [AddedById] in table 'GroupMembers'
ALTER TABLE [dbo].[GroupMembers]
ADD CONSTRAINT [FK_AddedByGroupMember]
    FOREIGN KEY ([AddedById])
    REFERENCES [dbo].[UserAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AddedByGroupMember'
CREATE INDEX [IX_FK_AddedByGroupMember]
ON [dbo].[GroupMembers]
    ([AddedById]);
GO

-- Creating foreign key on [CreatedById] in table 'UserGroups'
ALTER TABLE [dbo].[UserGroups]
ADD CONSTRAINT [FK_CreatedByUserGroup]
    FOREIGN KEY ([CreatedById])
    REFERENCES [dbo].[UserAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByUserGroup'
CREATE INDEX [IX_FK_CreatedByUserGroup]
ON [dbo].[UserGroups]
    ([CreatedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'UserGroups'
ALTER TABLE [dbo].[UserGroups]
ADD CONSTRAINT [FK_ModifiedByUserGroup]
    FOREIGN KEY ([ModifiedById])
    REFERENCES [dbo].[UserAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByUserGroup'
CREATE INDEX [IX_FK_ModifiedByUserGroup]
ON [dbo].[UserGroups]
    ([ModifiedById]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------