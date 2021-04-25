
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/25/2021 17:48:12
-- Generated from EDMX file: C:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat.Desktop\Model\Local\LocalDbModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [FsInfoCatDb];
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

-- Creating table 'Volumes'
CREATE TABLE [dbo].[Volumes] (
    [Id] uniqueidentifier  NOT NULL,
    [DisplayName] nvarchar(128)  NOT NULL,
    [RootPathName] nvarchar(1024)  NOT NULL,
    [VolumeName] nvarchar(128)  NOT NULL,
    [Identifier] nvarchar(1024)  NOT NULL,
    [FileSystemId] uniqueidentifier  NOT NULL,
    [Type] tinyint  NOT NULL,
    [CaseSensitiveSearch] bit  NULL,
    [ReadOnly] bit  NULL,
    [MaxNameLength] bigint  NULL,
    [Notes] nvarchar(max)  NOT NULL,
    [IsInactive] bit  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedOn] datetime  NOT NULL,
    [RootDirectory_Id] uniqueidentifier  NOT NULL
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
    [CreatedOn] datetime  NOT NULL,
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
    [CreatedOn] datetime  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating table 'Directories'
CREATE TABLE [dbo].[Directories] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] nvarchar(128)  NOT NULL,
    [ParentId] uniqueidentifier  NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating table 'Files'
CREATE TABLE [dbo].[Files] (
    [Id] uniqueidentifier  NOT NULL,
    [ParentId] uniqueidentifier  NOT NULL,
    [HashCalculationId] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating table 'HashCalculations'
CREATE TABLE [dbo].[HashCalculations] (
    [Id] uniqueidentifier  NOT NULL,
    [Length] bigint  NOT NULL,
    [Data] binary(16)  NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating table 'Redundancies'
CREATE TABLE [dbo].[Redundancies] (
    [Id] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating table 'Comparisons'
CREATE TABLE [dbo].[Comparisons] (
    [Id] uniqueidentifier  NOT NULL,
    [FileId1] uniqueidentifier  NOT NULL,
    [FileId2] uniqueidentifier  NOT NULL,
    [AreEqual] bit  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
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

-- Creating primary key on [Id] in table 'Volumes'
ALTER TABLE [dbo].[Volumes]
ADD CONSTRAINT [PK_Volumes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FileSystems'
ALTER TABLE [dbo].[FileSystems]
ADD CONSTRAINT [PK_FileSystems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FsSymbolicNames'
ALTER TABLE [dbo].[FsSymbolicNames]
ADD CONSTRAINT [PK_FsSymbolicNames]
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

-- Creating foreign key on [ParentId] in table 'Directories'
ALTER TABLE [dbo].[Directories]
ADD CONSTRAINT [FK_ParentDirectory]
    FOREIGN KEY ([ParentId])
    REFERENCES [dbo].[Directories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ParentDirectory'
CREATE INDEX [IX_FK_ParentDirectory]
ON [dbo].[Directories]
    ([ParentId]);
GO

-- Creating foreign key on [RootDirectory_Id] in table 'Volumes'
ALTER TABLE [dbo].[Volumes]
ADD CONSTRAINT [FK_DirectoryVolume]
    FOREIGN KEY ([RootDirectory_Id])
    REFERENCES [dbo].[Directories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DirectoryVolume'
CREATE INDEX [IX_FK_DirectoryVolume]
ON [dbo].[Volumes]
    ([RootDirectory_Id]);
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

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------