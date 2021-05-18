
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/28/2021 01:08:15
-- Generated from EDMX file: C:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat.Desktop\Model\Remote\UpstreamDbModel.edmx
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

IF OBJECT_ID(N'[dbo].[FK_DefaultFSTypeHostPlatform]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HostPlatforms] DROP CONSTRAINT [FK_DefaultFSTypeHostPlatform];
GO
IF OBJECT_ID(N'[dbo].[FK_HostPlatformHostDevice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HostDevices] DROP CONSTRAINT [FK_HostPlatformHostDevice];
GO
IF OBJECT_ID(N'[dbo].[FK_HostDeviceVolume]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Volumes] DROP CONSTRAINT [FK_HostDeviceVolume];
GO
IF OBJECT_ID(N'[dbo].[FK_FileSystemVolume]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Volumes] DROP CONSTRAINT [FK_FileSystemVolume];
GO
IF OBJECT_ID(N'[dbo].[FK_FileSystemFsSymbolicName]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FsSymbolicNames] DROP CONSTRAINT [FK_FileSystemFsSymbolicName];
GO
IF OBJECT_ID(N'[dbo].[FK_ContentInfoFile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Files] DROP CONSTRAINT [FK_ContentInfoFile];
GO
IF OBJECT_ID(N'[dbo].[FK_VolumeDirectory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Volumes] DROP CONSTRAINT [FK_VolumeDirectory];
GO
IF OBJECT_ID(N'[dbo].[FK_DirectoryParent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Directories] DROP CONSTRAINT [FK_DirectoryParent];
GO
IF OBJECT_ID(N'[dbo].[FK_DirectoryFile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Files] DROP CONSTRAINT [FK_DirectoryFile];
GO
IF OBJECT_ID(N'[dbo].[FK_FileComparison1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comparisons] DROP CONSTRAINT [FK_FileComparison1];
GO
IF OBJECT_ID(N'[dbo].[FK_FileComparison2]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comparisons] DROP CONSTRAINT [FK_FileComparison2];
GO
IF OBJECT_ID(N'[dbo].[FK_CreatedByFsSymbolicName]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FsSymbolicNames] DROP CONSTRAINT [FK_CreatedByFsSymbolicName];
GO
IF OBJECT_ID(N'[dbo].[FK_CreatedByComparison]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comparisons] DROP CONSTRAINT [FK_CreatedByComparison];
GO
IF OBJECT_ID(N'[dbo].[FK_CreatedByDirectory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Directories] DROP CONSTRAINT [FK_CreatedByDirectory];
GO
IF OBJECT_ID(N'[dbo].[FK_CreatedByFile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Files] DROP CONSTRAINT [FK_CreatedByFile];
GO
IF OBJECT_ID(N'[dbo].[FK_CreatedByFileSystem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FileSystems] DROP CONSTRAINT [FK_CreatedByFileSystem];
GO
IF OBJECT_ID(N'[dbo].[FK_CreatedByContentInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ContentInfos] DROP CONSTRAINT [FK_CreatedByContentInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_CreatedByHostDevice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HostDevices] DROP CONSTRAINT [FK_CreatedByHostDevice];
GO
IF OBJECT_ID(N'[dbo].[FK_CreatedByHostPlatform]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HostPlatforms] DROP CONSTRAINT [FK_CreatedByHostPlatform];
GO
IF OBJECT_ID(N'[dbo].[FK_CreatedByRedundancy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Redundancies] DROP CONSTRAINT [FK_CreatedByRedundancy];
GO
IF OBJECT_ID(N'[dbo].[FK_CreatedByUserProfile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserProfiles] DROP CONSTRAINT [FK_CreatedByUserProfile];
GO
IF OBJECT_ID(N'[dbo].[FK_CreatedByVolume]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Volumes] DROP CONSTRAINT [FK_CreatedByVolume];
GO
IF OBJECT_ID(N'[dbo].[FK_ModifiedByVolume]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Volumes] DROP CONSTRAINT [FK_ModifiedByVolume];
GO
IF OBJECT_ID(N'[dbo].[FK_ModifiedByUserProfile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserProfiles] DROP CONSTRAINT [FK_ModifiedByUserProfile];
GO
IF OBJECT_ID(N'[dbo].[FK_ModifiedByRedundancy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Redundancies] DROP CONSTRAINT [FK_ModifiedByRedundancy];
GO
IF OBJECT_ID(N'[dbo].[FK_ModifiedByHostPlatform]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HostPlatforms] DROP CONSTRAINT [FK_ModifiedByHostPlatform];
GO
IF OBJECT_ID(N'[dbo].[FK_ModifiedByHostDevice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HostDevices] DROP CONSTRAINT [FK_ModifiedByHostDevice];
GO
IF OBJECT_ID(N'[dbo].[FK_ModifiedByContentInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ContentInfos] DROP CONSTRAINT [FK_ModifiedByContentInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_ModifiedByFsSymbolicName]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FsSymbolicNames] DROP CONSTRAINT [FK_ModifiedByFsSymbolicName];
GO
IF OBJECT_ID(N'[dbo].[FK_ModifiedByFileSystem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FileSystems] DROP CONSTRAINT [FK_ModifiedByFileSystem];
GO
IF OBJECT_ID(N'[dbo].[FK_ModifiedByFile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Files] DROP CONSTRAINT [FK_ModifiedByFile];
GO
IF OBJECT_ID(N'[dbo].[FK_ModifiedByDirectory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Directories] DROP CONSTRAINT [FK_ModifiedByDirectory];
GO
IF OBJECT_ID(N'[dbo].[FK_ModifiedByComparison]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comparisons] DROP CONSTRAINT [FK_ModifiedByComparison];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UserProfiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserProfiles];
GO
IF OBJECT_ID(N'[dbo].[Volumes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Volumes];
GO
IF OBJECT_ID(N'[dbo].[Directories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Directories];
GO
IF OBJECT_ID(N'[dbo].[Files]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Files];
GO
IF OBJECT_ID(N'[dbo].[FileSystems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FileSystems];
GO
IF OBJECT_ID(N'[dbo].[HostDevices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HostDevices];
GO
IF OBJECT_ID(N'[dbo].[HostPlatforms]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HostPlatforms];
GO
IF OBJECT_ID(N'[dbo].[FsSymbolicNames]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FsSymbolicNames];
GO
IF OBJECT_ID(N'[dbo].[ContentInfos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ContentInfos];
GO
IF OBJECT_ID(N'[dbo].[Redundancies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Redundancies];
GO
IF OBJECT_ID(N'[dbo].[RedundancySets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RedundancySets];
GO
IF OBJECT_ID(N'[dbo].[Comparisons]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Comparisons];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UserProfiles'
CREATE TABLE [dbo].[UserProfiles] (
    [Id] uniqueidentifier  NOT NULL,
    [DisplayName] nvarchar(128)  NOT NULL,
    [FirstName] nvarchar(32)  NULL,
    [LastName] nvarchar(64)  NOT NULL,
    [MI] nchar(1)  NULL,
    [Suffix] nvarchar(32)  NULL,
    [Title] nvarchar(32)  NULL,
    [DbPrincipalId] int  NULL,
    [SID] varbinary(85)  NOT NULL,
    [LoginName] nvarchar(128)  NOT NULL,
    [ExplicitRoles] tinyint  NOT NULL,
    [Notes] nvarchar(max)  NOT NULL,
    [IsInactive] bit  NOT NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating table 'Volumes'
CREATE TABLE [dbo].[Volumes] (
    [Id] uniqueidentifier  NOT NULL,
    [DisplayName] nvarchar(128)  NOT NULL,
    [VolumeName] nvarchar(128)  NOT NULL,
    [Identifier] nvarchar(1024)  NOT NULL,
    [HostDeviceId] uniqueidentifier  NULL,
    [FileSystemId] uniqueidentifier  NOT NULL,
    [Type] tinyint  NOT NULL,
    [CaseSensitiveSearch] bit  NULL,
    [ReadOnly] bit  NULL,
    [MaxNameLength] bigint  NULL,
    [Notes] nvarchar(max)  NOT NULL,
    [IsInactive] bit  NOT NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL,
    [RootDirectory_Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Directories'
CREATE TABLE [dbo].[Directories] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] nvarchar(128)  NOT NULL,
    [Options] tinyint  NOT NULL,
    [Notes] nvarchar(max)  NOT NULL,
    [Deleted] bit  NOT NULL,
    [ParentId] uniqueidentifier  NULL,
    [SourceRelocationTaskId] uniqueidentifier  NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating table 'Files'
CREATE TABLE [dbo].[Files] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] nvarchar(128)  NOT NULL,
    [Options] tinyint  NOT NULL,
    [LastAccessed] datetime  NOT NULL,
    [LastHashCalculation] datetime NULL,
    [Notes] nvarchar(max)  NOT NULL,
    [Deleted] bit  NOT NULL,
    [ContentHashId] uniqueidentifier  NOT NULL,
    [RedundancyId] uniqueidentifier  NULL,
    [FileRelocateTaskId] uniqueidentifier  NULL,
    [ParentId] uniqueidentifier  NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating table 'ContentHashes'
CREATE TABLE [dbo].[ContentHashes] (
    [Id] uniqueidentifier  NOT NULL,
    [Length] bigint  NOT NULL,
    [Data] binary(16)  NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating table 'Redundancies'
CREATE TABLE [dbo].[Redundancies] (
    [Id] uniqueidentifier  NOT NULL,
    [Status] tinyint  NOT NULL,
    [Notes] nvarchar(max)  NOT NULL,
    [FileId] uniqueidentifier  NOT NULL,
    [RedundantSetId] uniqueidentifier  NOT NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating table 'RedundantSets'
CREATE TABLE [dbo].[RedundantSets] (
    [Id] uniqueidentifier  NOT NULL,
    [Notes] nvarchar(max)  NOT NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating table 'FileSystems'
CREATE TABLE [dbo].[FileSystems] (
    [Id] uniqueidentifier  NOT NULL,
    [DisplayName] nvarchar(128)  NOT NULL,
    [CaseSensitiveSearch] bit  NOT NULL,
    [ReadOnly] bit  NOT NULL,
    [MaxNameLength] bigint  NOT NULL,
    [DefaultDriveType] tinyint  NULL,
    [DefaultSymbolicNameId] uniqueidentifier  NOT NULL,
    [Notes] nvarchar(max)  NOT NULL,
    [IsInactive] bit  NOT NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating table 'HostDevices'
CREATE TABLE [dbo].[HostDevices] (
    [Id] uniqueidentifier  NOT NULL,
    [DisplayName] nvarchar(128)  NOT NULL,
    [MachineIdentifer] nvarchar(128)  NOT NULL,
    [MachineName] nvarchar(128)  NOT NULL,
    [PlatformId] uniqueidentifier  NOT NULL,
    [Notes] nvarchar(max)  NOT NULL,
    [IsInactive] bit  NOT NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating table 'HostPlatforms'
CREATE TABLE [dbo].[HostPlatforms] (
    [Id] uniqueidentifier  NOT NULL,
    [DisplayName] nvarchar(128)  NOT NULL,
    [Type] tinyint  NOT NULL,
    [DefaultFsTypeId] uniqueidentifier  NULL,
    [Notes] nvarchar(max)  NOT NULL,
    [IsInactive] bit  NOT NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating table 'FsSymbolicNames'
CREATE TABLE [dbo].[FsSymbolicNames] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] nvarchar(128)  NOT NULL,
    [FileSystemId] uniqueidentifier  NOT NULL,
    [Notes] nvarchar(max)  NOT NULL,
    [IsInactive] bit  NOT NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating table 'Comparisons'
CREATE TABLE [dbo].[Comparisons] (
    [Id] uniqueidentifier  NOT NULL,
    [FileId1] uniqueidentifier  NOT NULL,
    [FileId2] uniqueidentifier  NOT NULL,
    [AreEqual] bit  NOT NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating table 'FileRelocateTasks'
CREATE TABLE [dbo].[FileRelocateTasks] (
    [Id] uniqueidentifier  NOT NULL,
    [Status] tinyint  NOT NULL,
    [Priority] tinyint  NOT NULL,
    [ShortDescription] nvarchar(1024)  NOT NULL,
    [TargetDirectoryId] uniqueidentifier  NOT NULL,
    [AssignmentGroupId] uniqueidentifier  NULL,
    [AssignedToId] uniqueidentifier  NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [Notes] nvarchar(max)  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating table 'DirectoryRelocateTasks'
CREATE TABLE [dbo].[DirectoryRelocateTasks] (
    [Id] uniqueidentifier  NOT NULL,
    [Status] tinyint  NOT NULL,
    [Priority] tinyint  NOT NULL,
    [ShortDescription] nvarchar(1024)  NOT NULL,
    [AssignmentGroupId] uniqueidentifier  NULL,
    [AssignedToId] uniqueidentifier  NULL,
    [Notes] nvarchar(max)  NOT NULL,
    [IsInactive] bit  NOT NULL,
    [TargetDirectoryId] uniqueidentifier  NOT NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating table 'UserGroups'
CREATE TABLE [dbo].[UserGroups] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] nvarchar(128)  NOT NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [Roles] tinyint  NOT NULL,
    [Notes] nvarchar(max)  NOT NULL,
    [IsInactive] bit  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating table 'UserGroupUserProfile'
CREATE TABLE [dbo].[UserGroupUserProfile] (
    [AssignmentGroups_Id] uniqueidentifier  NOT NULL,
    [Members_Id] uniqueidentifier  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'UserProfiles'
ALTER TABLE [dbo].[UserProfiles]
ADD CONSTRAINT [PK_UserProfiles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Volumes'
ALTER TABLE [dbo].[Volumes]
ADD CONSTRAINT [PK_Volumes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Directories'
ALTER TABLE [dbo].[Directories]
ADD CONSTRAINT [PK_Directories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Files'
ALTER TABLE [dbo].[Files]
ADD CONSTRAINT [PK_Files]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FileSystems'
ALTER TABLE [dbo].[FileSystems]
ADD CONSTRAINT [PK_FileSystems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HostDevices'
ALTER TABLE [dbo].[HostDevices]
ADD CONSTRAINT [PK_HostDevices]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HostPlatforms'
ALTER TABLE [dbo].[HostPlatforms]
ADD CONSTRAINT [PK_HostPlatforms]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FsSymbolicNames'
ALTER TABLE [dbo].[FsSymbolicNames]
ADD CONSTRAINT [PK_FsSymbolicNames]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ContentInfo'
ALTER TABLE [dbo].[ContentInfo]
ADD CONSTRAINT [PK_ContentInfo]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Redundancies'
ALTER TABLE [dbo].[Redundancies]
ADD CONSTRAINT [PK_Redundancies]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RedundantSets'
ALTER TABLE [dbo].[RedundantSets]
ADD CONSTRAINT [PK_RedundantSets]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Comparisons'
ALTER TABLE [dbo].[Comparisons]
ADD CONSTRAINT [PK_Comparisons]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FileRelocateTasks'
ALTER TABLE [dbo].[FileRelocateTasks]
ADD CONSTRAINT [PK_FileRelocateTasks]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DirectoryRelocateTasks'
ALTER TABLE [dbo].[DirectoryRelocateTasks]
ADD CONSTRAINT [PK_DirectoryRelocateTasks]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserGroups'
ALTER TABLE [dbo].[UserGroups]
ADD CONSTRAINT [PK_UserGroups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [AssignmentGroups_Id], [Members_Id] in table 'UserGroupUserProfile'
ALTER TABLE [dbo].[UserGroupUserProfile]
ADD CONSTRAINT [PK_UserGroupUserProfile]
    PRIMARY KEY CLUSTERED ([AssignmentGroups_Id], [Members_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [DefaultFsTypeId] in table 'HostPlatforms'
ALTER TABLE [dbo].[HostPlatforms]
ADD CONSTRAINT [FK_DefaultFSTypeHostPlatform]
    FOREIGN KEY ([DefaultFsTypeId])
    REFERENCES [dbo].[FileSystems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DefaultFSTypeHostPlatform'
CREATE INDEX [IX_FK_DefaultFSTypeHostPlatform]
ON [dbo].[HostPlatforms]
    ([DefaultFsTypeId]);
GO

-- Creating foreign key on [PlatformId] in table 'HostDevices'
ALTER TABLE [dbo].[HostDevices]
ADD CONSTRAINT [FK_HostPlatformHostDevice]
    FOREIGN KEY ([PlatformId])
    REFERENCES [dbo].[HostPlatforms]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HostPlatformHostDevice'
CREATE INDEX [IX_FK_HostPlatformHostDevice]
ON [dbo].[HostDevices]
    ([PlatformId]);
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

-- Creating foreign key on [FileSystemId] in table 'Volumes'
ALTER TABLE [dbo].[Volumes]
ADD CONSTRAINT [FK_FileSystemVolume]
    FOREIGN KEY ([FileSystemId])
    REFERENCES [dbo].[FileSystems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FileSystemVolume'
CREATE INDEX [IX_FK_FileSystemVolume]
ON [dbo].[Volumes]
    ([FileSystemId]);
GO

-- Creating foreign key on [FileSystemId] in table 'FsSymbolicNames'
ALTER TABLE [dbo].[FsSymbolicNames]
ADD CONSTRAINT [FK_FileSystemFsSymbolicName]
    FOREIGN KEY ([FileSystemId])
    REFERENCES [dbo].[FileSystems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FileSystemFsSymbolicName'
CREATE INDEX [IX_FK_FileSystemFsSymbolicName]
ON [dbo].[FsSymbolicNames]
    ([FileSystemId]);
GO

-- Creating foreign key on [ContentInfoId] in table 'Files'
ALTER TABLE [dbo].[Files]
ADD CONSTRAINT [FK_ContentInfoFile]
    FOREIGN KEY ([ContentInfoId])
    REFERENCES [dbo].[ContentInfos]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ContentInfoFile'
CREATE INDEX [IX_FK_ContentInfoFile]
ON [dbo].[Files]
    ([ContentInfoId]);
GO

-- Creating foreign key on [RootDirectory_Id] in table 'Volumes'
ALTER TABLE [dbo].[Volumes]
ADD CONSTRAINT [FK_VolumeDirectory]
    FOREIGN KEY ([RootDirectory_Id])
    REFERENCES [dbo].[Directories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VolumeDirectory'
CREATE INDEX [IX_FK_VolumeDirectory]
ON [dbo].[Volumes]
    ([RootDirectory_Id]);
GO

-- Creating foreign key on [ParentId] in table 'Directories'
ALTER TABLE [dbo].[Directories]
ADD CONSTRAINT [FK_DirectoryParent]
    FOREIGN KEY ([ParentId])
    REFERENCES [dbo].[Directories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DirectoryParent'
CREATE INDEX [IX_FK_DirectoryParent]
ON [dbo].[Directories]
    ([ParentId]);
GO

-- Creating foreign key on [ParentId] in table 'Files'
ALTER TABLE [dbo].[Files]
ADD CONSTRAINT [FK_DirectoryFile]
    FOREIGN KEY ([ParentId])
    REFERENCES [dbo].[Directories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DirectoryFile'
CREATE INDEX [IX_FK_DirectoryFile]
ON [dbo].[Files]
    ([ParentId]);
GO

-- Creating foreign key on [FileId1] in table 'Comparisons'
ALTER TABLE [dbo].[Comparisons]
ADD CONSTRAINT [FK_FileComparison1]
    FOREIGN KEY ([FileId1])
    REFERENCES [dbo].[Files]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FileComparison1'
CREATE INDEX [IX_FK_FileComparison1]
ON [dbo].[Comparisons]
    ([FileId1]);
GO

-- Creating foreign key on [FileId2] in table 'Comparisons'
ALTER TABLE [dbo].[Comparisons]
ADD CONSTRAINT [FK_FileComparison2]
    FOREIGN KEY ([FileId2])
    REFERENCES [dbo].[Files]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FileComparison2'
CREATE INDEX [IX_FK_FileComparison2]
ON [dbo].[Comparisons]
    ([FileId2]);
GO

-- Creating foreign key on [CreatedById] in table 'FsSymbolicNames'
ALTER TABLE [dbo].[FsSymbolicNames]
ADD CONSTRAINT [FK_CreatedByFsSymbolicName]
    FOREIGN KEY ([CreatedById])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByFsSymbolicName'
CREATE INDEX [IX_FK_CreatedByFsSymbolicName]
ON [dbo].[FsSymbolicNames]
    ([CreatedById]);
GO

-- Creating foreign key on [CreatedById] in table 'Comparisons'
ALTER TABLE [dbo].[Comparisons]
ADD CONSTRAINT [FK_CreatedByComparison]
    FOREIGN KEY ([CreatedById])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByComparison'
CREATE INDEX [IX_FK_CreatedByComparison]
ON [dbo].[Comparisons]
    ([CreatedById]);
GO

-- Creating foreign key on [CreatedById] in table 'Directories'
ALTER TABLE [dbo].[Directories]
ADD CONSTRAINT [FK_CreatedByDirectory]
    FOREIGN KEY ([CreatedById])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByDirectory'
CREATE INDEX [IX_FK_CreatedByDirectory]
ON [dbo].[Directories]
    ([CreatedById]);
GO

-- Creating foreign key on [CreatedById] in table 'Files'
ALTER TABLE [dbo].[Files]
ADD CONSTRAINT [FK_CreatedByFile]
    FOREIGN KEY ([CreatedById])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByFile'
CREATE INDEX [IX_FK_CreatedByFile]
ON [dbo].[Files]
    ([CreatedById]);
GO

-- Creating foreign key on [CreatedById] in table 'FileSystems'
ALTER TABLE [dbo].[FileSystems]
ADD CONSTRAINT [FK_CreatedByFileSystem]
    FOREIGN KEY ([CreatedById])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByFileSystem'
CREATE INDEX [IX_FK_CreatedByFileSystem]
ON [dbo].[FileSystems]
    ([CreatedById]);
GO

-- Creating foreign key on [CreatedById] in table 'ContentInfos'
ALTER TABLE [dbo].[ContentInfos]
ADD CONSTRAINT [FK_CreatedByContentInfo]
    FOREIGN KEY ([CreatedById])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByContentInfo'
CREATE INDEX [IX_FK_CreatedByContentInfo]
ON [dbo].[ContentInfos]
    ([CreatedById]);
GO

-- Creating foreign key on [CreatedById] in table 'HostDevices'
ALTER TABLE [dbo].[HostDevices]
ADD CONSTRAINT [FK_CreatedByHostDevice]
    FOREIGN KEY ([CreatedById])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByHostDevice'
CREATE INDEX [IX_FK_CreatedByHostDevice]
ON [dbo].[HostDevices]
    ([CreatedById]);
GO

-- Creating foreign key on [CreatedById] in table 'HostPlatforms'
ALTER TABLE [dbo].[HostPlatforms]
ADD CONSTRAINT [FK_CreatedByHostPlatform]
    FOREIGN KEY ([CreatedById])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByHostPlatform'
CREATE INDEX [IX_FK_CreatedByHostPlatform]
ON [dbo].[HostPlatforms]
    ([CreatedById]);
GO

-- Creating foreign key on [CreatedById] in table 'Redundancies'
ALTER TABLE [dbo].[Redundancies]
ADD CONSTRAINT [FK_CreatedByRedundancy]
    FOREIGN KEY ([CreatedById])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByRedundancy'
CREATE INDEX [IX_FK_CreatedByRedundancy]
ON [dbo].[Redundancies]
    ([CreatedById]);
GO

-- Creating foreign key on [CreatedById] in table 'UserProfiles'
ALTER TABLE [dbo].[UserProfiles]
ADD CONSTRAINT [FK_CreatedByUserProfile]
    FOREIGN KEY ([CreatedById])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByUserProfile'
CREATE INDEX [IX_FK_CreatedByUserProfile]
ON [dbo].[UserProfiles]
    ([CreatedById]);
GO

-- Creating foreign key on [CreatedById] in table 'Volumes'
ALTER TABLE [dbo].[Volumes]
ADD CONSTRAINT [FK_CreatedByVolume]
    FOREIGN KEY ([CreatedById])
    REFERENCES [dbo].[UserProfiles]
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
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByVolume'
CREATE INDEX [IX_FK_ModifiedByVolume]
ON [dbo].[Volumes]
    ([ModifiedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'UserProfiles'
ALTER TABLE [dbo].[UserProfiles]
ADD CONSTRAINT [FK_ModifiedByUserProfile]
    FOREIGN KEY ([ModifiedById])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByUserProfile'
CREATE INDEX [IX_FK_ModifiedByUserProfile]
ON [dbo].[UserProfiles]
    ([ModifiedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'Redundancies'
ALTER TABLE [dbo].[Redundancies]
ADD CONSTRAINT [FK_ModifiedByRedundancy]
    FOREIGN KEY ([ModifiedById])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByRedundancy'
CREATE INDEX [IX_FK_ModifiedByRedundancy]
ON [dbo].[Redundancies]
    ([ModifiedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'HostPlatforms'
ALTER TABLE [dbo].[HostPlatforms]
ADD CONSTRAINT [FK_ModifiedByHostPlatform]
    FOREIGN KEY ([ModifiedById])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByHostPlatform'
CREATE INDEX [IX_FK_ModifiedByHostPlatform]
ON [dbo].[HostPlatforms]
    ([ModifiedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'HostDevices'
ALTER TABLE [dbo].[HostDevices]
ADD CONSTRAINT [FK_ModifiedByHostDevice]
    FOREIGN KEY ([ModifiedById])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByHostDevice'
CREATE INDEX [IX_FK_ModifiedByHostDevice]
ON [dbo].[HostDevices]
    ([ModifiedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'ContentInfos'
ALTER TABLE [dbo].[ContentInfos]
ADD CONSTRAINT [FK_ModifiedByContentInfo]
    FOREIGN KEY ([ModifiedById])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByContentInfo'
CREATE INDEX [IX_FK_ModifiedByContentInfo]
ON [dbo].[ContentInfos]
    ([ModifiedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'FsSymbolicNames'
ALTER TABLE [dbo].[FsSymbolicNames]
ADD CONSTRAINT [FK_ModifiedByFsSymbolicName]
    FOREIGN KEY ([ModifiedById])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByFsSymbolicName'
CREATE INDEX [IX_FK_ModifiedByFsSymbolicName]
ON [dbo].[FsSymbolicNames]
    ([ModifiedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'FileSystems'
ALTER TABLE [dbo].[FileSystems]
ADD CONSTRAINT [FK_ModifiedByFileSystem]
    FOREIGN KEY ([ModifiedById])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByFileSystem'
CREATE INDEX [IX_FK_ModifiedByFileSystem]
ON [dbo].[FileSystems]
    ([ModifiedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'Files'
ALTER TABLE [dbo].[Files]
ADD CONSTRAINT [FK_ModifiedByFile]
    FOREIGN KEY ([ModifiedById])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByFile'
CREATE INDEX [IX_FK_ModifiedByFile]
ON [dbo].[Files]
    ([ModifiedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'Directories'
ALTER TABLE [dbo].[Directories]
ADD CONSTRAINT [FK_ModifiedByDirectory]
    FOREIGN KEY ([ModifiedById])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByDirectory'
CREATE INDEX [IX_FK_ModifiedByDirectory]
ON [dbo].[Directories]
    ([ModifiedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'Comparisons'
ALTER TABLE [dbo].[Comparisons]
ADD CONSTRAINT [FK_ModifiedByComparison]
    FOREIGN KEY ([ModifiedById])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByComparison'
CREATE INDEX [IX_FK_ModifiedByComparison]
ON [dbo].[Comparisons]
    ([ModifiedById]);
GO

-- Creating foreign key on [SourceRelocationTaskId] in table 'Directories'
ALTER TABLE [dbo].[Directories]
ADD CONSTRAINT [FK_SourceDirectoryRelocateTask]
    FOREIGN KEY ([SourceRelocationTaskId])
    REFERENCES [dbo].[DirectoryRelocateTasks]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SourceDirectoryRelocateTask'
CREATE INDEX [IX_FK_SourceDirectoryRelocateTask]
ON [dbo].[Directories]
    ([SourceRelocationTaskId]);
GO

-- Creating foreign key on [AssignmentGroups_Id] in table 'UserGroupUserProfile'
ALTER TABLE [dbo].[UserGroupUserProfile]
ADD CONSTRAINT [FK_UserGroupUserProfile_UserGroup]
    FOREIGN KEY ([AssignmentGroups_Id])
    REFERENCES [dbo].[UserGroups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Members_Id] in table 'UserGroupUserProfile'
ALTER TABLE [dbo].[UserGroupUserProfile]
ADD CONSTRAINT [FK_UserGroupUserProfile_UserProfile]
    FOREIGN KEY ([Members_Id])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserGroupUserProfile_UserProfile'
CREATE INDEX [IX_FK_UserGroupUserProfile_UserProfile]
ON [dbo].[UserGroupUserProfile]
    ([Members_Id]);
GO

-- Creating foreign key on [AssignmentGroupId] in table 'DirectoryRelocateTasks'
ALTER TABLE [dbo].[DirectoryRelocateTasks]
ADD CONSTRAINT [FK_UserGroupDirectoryRelocateTask]
    FOREIGN KEY ([AssignmentGroupId])
    REFERENCES [dbo].[UserGroups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserGroupDirectoryRelocateTask'
CREATE INDEX [IX_FK_UserGroupDirectoryRelocateTask]
ON [dbo].[DirectoryRelocateTasks]
    ([AssignmentGroupId]);
GO

-- Creating foreign key on [AssignedToId] in table 'DirectoryRelocateTasks'
ALTER TABLE [dbo].[DirectoryRelocateTasks]
ADD CONSTRAINT [FK_UserProfileDirectoryRelocateTask]
    FOREIGN KEY ([AssignedToId])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserProfileDirectoryRelocateTask'
CREATE INDEX [IX_FK_UserProfileDirectoryRelocateTask]
ON [dbo].[DirectoryRelocateTasks]
    ([AssignedToId]);
GO

-- Creating foreign key on [FileRelocateTaskId] in table 'Files'
ALTER TABLE [dbo].[Files]
ADD CONSTRAINT [FK_FileRelocateTaskFile]
    FOREIGN KEY ([FileRelocateTaskId])
    REFERENCES [dbo].[FileRelocateTasks]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FileRelocateTaskFile'
CREATE INDEX [IX_FK_FileRelocateTaskFile]
ON [dbo].[Files]
    ([FileRelocateTaskId]);
GO

-- Creating foreign key on [RedundancyId] in table 'Files'
ALTER TABLE [dbo].[Files]
ADD CONSTRAINT [FK_FileRedundancy]
    FOREIGN KEY ([RedundancyId])
    REFERENCES [dbo].[Redundancy]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FileRedundancy'
CREATE INDEX [IX_FK_FileRedundancy]
ON [dbo].[Files]
    ([Id]);
GO

-- Creating foreign key on [TargetDirectoryId] in table 'FileRelocateTasks'
ALTER TABLE [dbo].[FileRelocateTasks]
ADD CONSTRAINT [FK_DirectoryFileRelocateTask]
    FOREIGN KEY ([TargetDirectoryId])
    REFERENCES [dbo].[Directories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DirectoryFileRelocateTask'
CREATE INDEX [IX_FK_DirectoryFileRelocateTask]
ON [dbo].[FileRelocateTasks]
    ([TargetDirectoryId]);
GO

-- Creating foreign key on [TargetDirectoryId] in table 'DirectoryRelocateTasks'
ALTER TABLE [dbo].[DirectoryRelocateTasks]
ADD CONSTRAINT [FK_TargetDirectoryRelocateTask]
    FOREIGN KEY ([TargetDirectoryId])
    REFERENCES [dbo].[Directories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TargetDirectoryRelocateTask'
CREATE INDEX [IX_FK_TargetDirectoryRelocateTask]
ON [dbo].[DirectoryRelocateTasks]
    ([TargetDirectoryId]);
GO

-- Creating foreign key on [AssignmentGroupId] in table 'FileRelocateTasks'
ALTER TABLE [dbo].[FileRelocateTasks]
ADD CONSTRAINT [FK_UserGroupFileRelocateTask]
    FOREIGN KEY ([AssignmentGroupId])
    REFERENCES [dbo].[UserGroups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserGroupFileRelocateTask'
CREATE INDEX [IX_FK_UserGroupFileRelocateTask]
ON [dbo].[FileRelocateTasks]
    ([AssignmentGroupId]);
GO

-- Creating foreign key on [AssignedToId] in table 'FileRelocateTasks'
ALTER TABLE [dbo].[FileRelocateTasks]
ADD CONSTRAINT [FK_UserProfileFileRelocateTask]
    FOREIGN KEY ([AssignedToId])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserProfileFileRelocateTask'
CREATE INDEX [IX_FK_UserProfileFileRelocateTask]
ON [dbo].[FileRelocateTasks]
    ([AssignedToId]);
GO

-- Creating foreign key on [CreatedById] in table 'DirectoryRelocateTasks'
ALTER TABLE [dbo].[DirectoryRelocateTasks]
ADD CONSTRAINT [FK_CreatedByDirectoryRelocateTask]
    FOREIGN KEY ([CreatedById])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByDirectoryRelocateTask'
CREATE INDEX [IX_FK_CreatedByDirectoryRelocateTask]
ON [dbo].[DirectoryRelocateTasks]
    ([CreatedById]);
GO

-- Creating foreign key on [CreatedById] in table 'FileRelocateTasks'
ALTER TABLE [dbo].[FileRelocateTasks]
ADD CONSTRAINT [FK_CreatedByFileRelocateTask]
    FOREIGN KEY ([CreatedById])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByFileRelocateTask'
CREATE INDEX [IX_FK_CreatedByFileRelocateTask]
ON [dbo].[FileRelocateTasks]
    ([CreatedById]);
GO

-- Creating foreign key on [CreatedById] in table 'UserGroups'
ALTER TABLE [dbo].[UserGroups]
ADD CONSTRAINT [FK_CreatedByUserGroup]
    FOREIGN KEY ([CreatedById])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByUserGroup'
CREATE INDEX [IX_FK_CreatedByUserGroup]
ON [dbo].[UserGroups]
    ([CreatedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'DirectoryRelocateTasks'
ALTER TABLE [dbo].[DirectoryRelocateTasks]
ADD CONSTRAINT [FK_ModifiedByDirectoryRelocateTask]
    FOREIGN KEY ([ModifiedById])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByDirectoryRelocateTask'
CREATE INDEX [IX_FK_ModifiedByDirectoryRelocateTask]
ON [dbo].[DirectoryRelocateTasks]
    ([ModifiedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'FileRelocateTasks'
ALTER TABLE [dbo].[FileRelocateTasks]
ADD CONSTRAINT [FK_ModifiedByFileRelocateTask]
    FOREIGN KEY ([ModifiedById])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByFileRelocateTask'
CREATE INDEX [IX_FK_ModifiedByFileRelocateTask]
ON [dbo].[FileRelocateTasks]
    ([ModifiedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'UserGroups'
ALTER TABLE [dbo].[UserGroups]
ADD CONSTRAINT [FK_ModifiedByUserGroup]
    FOREIGN KEY ([ModifiedById])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByUserGroup'
CREATE INDEX [IX_FK_ModifiedByUserGroup]
ON [dbo].[UserGroups]
    ([ModifiedById]);
GO

-- Creating foreign key on [DefaultSymbolicNameId] in table 'FileSystems'
ALTER TABLE [dbo].[FileSystems]
ADD CONSTRAINT [FK_FsSymbolicNameFileSystem]
    FOREIGN KEY ([DefaultSymbolicNameId])
    REFERENCES [dbo].[FsSymbolicNames]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FsSymbolicNameFileSystem'
CREATE INDEX [IX_FK_FsSymbolicNameFileSystem]
ON [dbo].[FileSystems]
    ([DefaultSymbolicNameId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
