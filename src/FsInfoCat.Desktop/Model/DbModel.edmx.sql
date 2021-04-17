
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/17/2021 16:53:44
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


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


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
    [Notes] nvarchar(max)  NOT NULL
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

-- Creating table 'BasicLogins'
CREATE TABLE [dbo].[BasicLogins] (
    [Id] uniqueidentifier  NOT NULL,
    [LoginName] nvarchar(max)  NOT NULL,
    [PwHash] nvarchar(max)  NOT NULL,
    [FailCount] tinyint  NOT NULL,
    [LockedOut] bit  NOT NULL,
    [IsInactive] bit  NOT NULL,
    [UserId] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'WindowsIdentityLogins'
CREATE TABLE [dbo].[WindowsIdentityLogins] (
    [Id] uniqueidentifier  NOT NULL,
    [SID] nvarchar(max)  NOT NULL,
    [DomainId] uniqueidentifier  NOT NULL,
    [AccountName] nvarchar(max)  NOT NULL,
    [IsInactive] bit  NOT NULL,
    [UserId] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [Notes] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'WindowsAuthDomains'
CREATE TABLE [dbo].[WindowsAuthDomains] (
    [Id] uniqueidentifier  NOT NULL,
    [SID] nvarchar(max)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [IsInactive] bit  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [Notes] nvarchar(max)  NOT NULL,
    [AutoAddUsers] bit  NOT NULL,
    [DefaultNewUserGroupId] uniqueidentifier  NULL
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

-- Creating table 'WindowsGroupIdentities'
CREATE TABLE [dbo].[WindowsGroupIdentities] (
    [Id] uniqueidentifier  NOT NULL,
    [SID] nvarchar(max)  NOT NULL,
    [DomainId] uniqueidentifier  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [IsInactive] bit  NOT NULL,
    [GroupId] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [Notes] nvarchar(max)  NOT NULL,
    [AutoAddMembers] bit  NOT NULL
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

-- Creating primary key on [Id] in table 'BasicLogins'
ALTER TABLE [dbo].[BasicLogins]
ADD CONSTRAINT [PK_BasicLogins]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'WindowsIdentityLogins'
ALTER TABLE [dbo].[WindowsIdentityLogins]
ADD CONSTRAINT [PK_WindowsIdentityLogins]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'WindowsAuthDomains'
ALTER TABLE [dbo].[WindowsAuthDomains]
ADD CONSTRAINT [PK_WindowsAuthDomains]
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

-- Creating primary key on [Id] in table 'WindowsGroupIdentities'
ALTER TABLE [dbo].[WindowsGroupIdentities]
ADD CONSTRAINT [PK_WindowsGroupIdentities]
    PRIMARY KEY CLUSTERED ([Id] ASC);
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

-- Creating foreign key on [CreatedById] in table 'BasicLogins'
ALTER TABLE [dbo].[BasicLogins]
ADD CONSTRAINT [FK_UserAccountBasicLogin]
    FOREIGN KEY ([CreatedById])
    REFERENCES [dbo].[UserAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserAccountBasicLogin'
CREATE INDEX [IX_FK_UserAccountBasicLogin]
ON [dbo].[BasicLogins]
    ([CreatedById]);
GO

-- Creating foreign key on [UserId] in table 'BasicLogins'
ALTER TABLE [dbo].[BasicLogins]
ADD CONSTRAINT [FK_CreatedByBasicLogin]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[UserAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByBasicLogin'
CREATE INDEX [IX_FK_CreatedByBasicLogin]
ON [dbo].[BasicLogins]
    ([UserId]);
GO

-- Creating foreign key on [ModifiedById] in table 'BasicLogins'
ALTER TABLE [dbo].[BasicLogins]
ADD CONSTRAINT [FK_ModifiedByBasicLogin]
    FOREIGN KEY ([ModifiedById])
    REFERENCES [dbo].[UserAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByBasicLogin'
CREATE INDEX [IX_FK_ModifiedByBasicLogin]
ON [dbo].[BasicLogins]
    ([ModifiedById]);
GO

-- Creating foreign key on [DomainId] in table 'WindowsIdentityLogins'
ALTER TABLE [dbo].[WindowsIdentityLogins]
ADD CONSTRAINT [FK_WindowsAuthDomainWindowsIdentityLogin]
    FOREIGN KEY ([DomainId])
    REFERENCES [dbo].[WindowsAuthDomains]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WindowsAuthDomainWindowsIdentityLogin'
CREATE INDEX [IX_FK_WindowsAuthDomainWindowsIdentityLogin]
ON [dbo].[WindowsIdentityLogins]
    ([DomainId]);
GO

-- Creating foreign key on [UserId] in table 'WindowsIdentityLogins'
ALTER TABLE [dbo].[WindowsIdentityLogins]
ADD CONSTRAINT [FK_UserAccountWindowsIdentityLogin]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[UserAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserAccountWindowsIdentityLogin'
CREATE INDEX [IX_FK_UserAccountWindowsIdentityLogin]
ON [dbo].[WindowsIdentityLogins]
    ([UserId]);
GO

-- Creating foreign key on [CreatedById] in table 'WindowsIdentityLogins'
ALTER TABLE [dbo].[WindowsIdentityLogins]
ADD CONSTRAINT [FK_CreatedByWindowsIdentityLogin]
    FOREIGN KEY ([CreatedById])
    REFERENCES [dbo].[UserAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByWindowsIdentityLogin'
CREATE INDEX [IX_FK_CreatedByWindowsIdentityLogin]
ON [dbo].[WindowsIdentityLogins]
    ([CreatedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'WindowsIdentityLogins'
ALTER TABLE [dbo].[WindowsIdentityLogins]
ADD CONSTRAINT [FK_ModifiedByWindowsIdentityLogin]
    FOREIGN KEY ([ModifiedById])
    REFERENCES [dbo].[UserAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByWindowsIdentityLogin'
CREATE INDEX [IX_FK_ModifiedByWindowsIdentityLogin]
ON [dbo].[WindowsIdentityLogins]
    ([ModifiedById]);
GO

-- Creating foreign key on [CreatedById] in table 'WindowsAuthDomains'
ALTER TABLE [dbo].[WindowsAuthDomains]
ADD CONSTRAINT [FK_CreatedByWindowsAuthDomain]
    FOREIGN KEY ([CreatedById])
    REFERENCES [dbo].[UserAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByWindowsAuthDomain'
CREATE INDEX [IX_FK_CreatedByWindowsAuthDomain]
ON [dbo].[WindowsAuthDomains]
    ([CreatedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'WindowsAuthDomains'
ALTER TABLE [dbo].[WindowsAuthDomains]
ADD CONSTRAINT [FK_ModifiedByWindowsAuthDomain]
    FOREIGN KEY ([ModifiedById])
    REFERENCES [dbo].[UserAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByWindowsAuthDomain'
CREATE INDEX [IX_FK_ModifiedByWindowsAuthDomain]
ON [dbo].[WindowsAuthDomains]
    ([ModifiedById]);
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

-- Creating foreign key on [GroupId] in table 'WindowsGroupIdentities'
ALTER TABLE [dbo].[WindowsGroupIdentities]
ADD CONSTRAINT [FK_UserGroupWindowsGroupIdentity]
    FOREIGN KEY ([GroupId])
    REFERENCES [dbo].[UserGroups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserGroupWindowsGroupIdentity'
CREATE INDEX [IX_FK_UserGroupWindowsGroupIdentity]
ON [dbo].[WindowsGroupIdentities]
    ([GroupId]);
GO

-- Creating foreign key on [DomainId] in table 'WindowsGroupIdentities'
ALTER TABLE [dbo].[WindowsGroupIdentities]
ADD CONSTRAINT [FK_WindowsAuthDomainWindowsGroupIdentity]
    FOREIGN KEY ([DomainId])
    REFERENCES [dbo].[WindowsAuthDomains]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WindowsAuthDomainWindowsGroupIdentity'
CREATE INDEX [IX_FK_WindowsAuthDomainWindowsGroupIdentity]
ON [dbo].[WindowsGroupIdentities]
    ([DomainId]);
GO

-- Creating foreign key on [CreatedById] in table 'WindowsGroupIdentities'
ALTER TABLE [dbo].[WindowsGroupIdentities]
ADD CONSTRAINT [FK_CreatedByUserAccountWindowsGroupIdentity]
    FOREIGN KEY ([CreatedById])
    REFERENCES [dbo].[UserAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByUserAccountWindowsGroupIdentity'
CREATE INDEX [IX_FK_CreatedByUserAccountWindowsGroupIdentity]
ON [dbo].[WindowsGroupIdentities]
    ([CreatedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'WindowsGroupIdentities'
ALTER TABLE [dbo].[WindowsGroupIdentities]
ADD CONSTRAINT [FK_ModifiedByWindowsGroupIdentity]
    FOREIGN KEY ([ModifiedById])
    REFERENCES [dbo].[UserAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByWindowsGroupIdentity'
CREATE INDEX [IX_FK_ModifiedByWindowsGroupIdentity]
ON [dbo].[WindowsGroupIdentities]
    ([ModifiedById]);
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

-- Creating foreign key on [DefaultNewUserGroupId] in table 'WindowsAuthDomains'
ALTER TABLE [dbo].[WindowsAuthDomains]
ADD CONSTRAINT [FK_DefaultNewUserGroupWindowsAuthDomain]
    FOREIGN KEY ([DefaultNewUserGroupId])
    REFERENCES [dbo].[UserGroups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DefaultNewUserGroupWindowsAuthDomain'
CREATE INDEX [IX_FK_DefaultNewUserGroupWindowsAuthDomain]
ON [dbo].[WindowsAuthDomains]
    ([DefaultNewUserGroupId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------