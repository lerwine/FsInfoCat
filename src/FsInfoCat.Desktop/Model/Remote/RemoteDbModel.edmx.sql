
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/25/2021 17:21:32
-- Generated from EDMX file: C:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat.Desktop\Model\Remote\RemoteDbModel.edmx
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
    [RootPathName] nvarchar(1024)  NOT NULL,
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
    [ParentId] uniqueidentifier  NULL,
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
    [HashCalculationId] uniqueidentifier  NOT NULL,
    [ParentId] uniqueidentifier  NOT NULL,
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

-- Creating table 'HashCalculations'
CREATE TABLE [dbo].[HashCalculations] (
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

-- Creating table 'RedundancyFile'
CREATE TABLE [dbo].[RedundancyFile] (
    [Redundancies_Id] uniqueidentifier  NOT NULL,
    [Files_Id] uniqueidentifier  NOT NULL
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

-- Creating primary key on [Id] in table 'HashCalculations'
ALTER TABLE [dbo].[HashCalculations]
ADD CONSTRAINT [PK_HashCalculations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Redundancies'
ALTER TABLE [dbo].[Redundancies]
ADD CONSTRAINT [PK_Redundancies]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Comparisons'
ALTER TABLE [dbo].[Comparisons]
ADD CONSTRAINT [PK_Comparisons]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Redundancies_Id], [Files_Id] in table 'RedundancyFile'
ALTER TABLE [dbo].[RedundancyFile]
ADD CONSTRAINT [PK_RedundancyFile]
    PRIMARY KEY CLUSTERED ([Redundancies_Id], [Files_Id] ASC);
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

-- Creating foreign key on [HashCalculationId] in table 'Files'
ALTER TABLE [dbo].[Files]
ADD CONSTRAINT [FK_HashCalculationFile]
    FOREIGN KEY ([HashCalculationId])
    REFERENCES [dbo].[HashCalculations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HashCalculationFile'
CREATE INDEX [IX_FK_HashCalculationFile]
ON [dbo].[Files]
    ([HashCalculationId]);
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

-- Creating foreign key on [Redundancies_Id] in table 'RedundancyFile'
ALTER TABLE [dbo].[RedundancyFile]
ADD CONSTRAINT [FK_RedundancyFile_Redundancy]
    FOREIGN KEY ([Redundancies_Id])
    REFERENCES [dbo].[Redundancies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Files_Id] in table 'RedundancyFile'
ALTER TABLE [dbo].[RedundancyFile]
ADD CONSTRAINT [FK_RedundancyFile_File]
    FOREIGN KEY ([Files_Id])
    REFERENCES [dbo].[Files]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RedundancyFile_File'
CREATE INDEX [IX_FK_RedundancyFile_File]
ON [dbo].[RedundancyFile]
    ([Files_Id]);
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

-- Creating foreign key on [CreatedById] in table 'HashCalculations'
ALTER TABLE [dbo].[HashCalculations]
ADD CONSTRAINT [FK_CreatedByHashCalculation]
    FOREIGN KEY ([CreatedById])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByHashCalculation'
CREATE INDEX [IX_FK_CreatedByHashCalculation]
ON [dbo].[HashCalculations]
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

-- Creating foreign key on [ModifiedById] in table 'HashCalculations'
ALTER TABLE [dbo].[HashCalculations]
ADD CONSTRAINT [FK_ModifiedByHashCalculation]
    FOREIGN KEY ([ModifiedById])
    REFERENCES [dbo].[UserProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByHashCalculation'
CREATE INDEX [IX_FK_ModifiedByHashCalculation]
ON [dbo].[HashCalculations]
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

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------