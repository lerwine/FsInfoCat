-- Creating table 'FileSystems'
CREATE TABLE [FileSystems] (
    [Id] uniqueidentifier  NOT NULL,
    [DisplayName] nvarchar(128)  NOT NULL,
    [CaseSensitiveSearch] bit  NOT NULL,
    [ReadOnly] bit  NOT NULL,
    [MaxNameLength] bigint  NOT NULL,
    [DefaultDriveType] tinyint  NULL,
    [DefaultSymbolicNameId] uniqueidentifier  NOT NULL,
    [Notes] ntext  NOT NULL,
    [IsInactive] bit  NOT NULL,
    [UpstreamId] uniqueidentifier NULL,
    [LastSynchronized] datetime  NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'FileSystems'
ALTER TABLE [FileSystems] ADD CONSTRAINT [PK_FileSystems] PRIMARY KEY ([Id]);
GO

-- Creating table 'SymbolicNames'
CREATE TABLE [SymbolicNames] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] nvarchar(128)  NOT NULL,
    [FileSystemId] uniqueidentifier  NOT NULL,
    [Notes] ntext  NOT NULL,
    [IsInactive] bit  NOT NULL,
    [UpstreamId] uniqueidentifier NULL,
    [LastSynchronized] datetime  NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'SymbolicNames'
ALTER TABLE [SymbolicNames] ADD CONSTRAINT [PK_SymbolicNames] PRIMARY KEY ([Id]);
GO

-- Creating foreign key on [FileSystemId] in table 'SymbolicNames'
ALTER TABLE [SymbolicNames] ADD CONSTRAINT [FK_FileSystemSymbolicName] FOREIGN KEY ([FileSystemId]) REFERENCES [FileSystems] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FileSystemSymbolicName'
CREATE INDEX [IX_FK_FileSystemSymbolicName] ON [SymbolicNames] ([FileSystemId]);
GO

-- Creating foreign key on [DefaultSymbolicNameId] in table 'FileSystems'
ALTER TABLE [FileSystems] ADD CONSTRAINT [FK_SymbolicNameFileSystem] FOREIGN KEY ([DefaultSymbolicNameId]) REFERENCES [SymbolicNames] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SymbolicNameFileSystem'
CREATE INDEX [IX_FK_SymbolicNameFileSystem] ON [FileSystems] ([DefaultSymbolicNameId]);
GO

-- Creating table 'Volumes'
CREATE TABLE [Volumes] (
    [Id] uniqueidentifier  NOT NULL,
    [DisplayName] nvarchar(128)  NOT NULL,
    [VolumeName] nvarchar(128)  NOT NULL,
    [Identifier] nvarchar(1024)  NOT NULL,
    [FileSystemId] uniqueidentifier  NOT NULL,
    [RootDirectoryId] uniqueidentifier  NOT NULL,
    [Type] tinyint  NOT NULL,
    [CaseSensitiveSearch] bit  NULL,
    [ReadOnly] bit  NULL,
    [MaxNameLength] bigint  NULL,
    [Notes] ntext  NOT NULL,
    [IsInactive] bit  NOT NULL,
    [UpstreamId] uniqueidentifier NULL,
    [LastSynchronized] datetime  NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'Volumes'
ALTER TABLE [Volumes] ADD CONSTRAINT [PK_Volumes] PRIMARY KEY ([Id]);
GO

-- Creating foreign key on [FileSystemId] in table 'Volumes'
ALTER TABLE [Volumes] ADD CONSTRAINT [FK_FileSystemVolume] FOREIGN KEY ([FileSystemId]) REFERENCES [FileSystems] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FileSystemVolume'
CREATE INDEX [IX_FK_FileSystemVolume] ON [Volumes] ([FileSystemId]);
GO

-- Creating table 'Directories'
CREATE TABLE [Directories] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] nvarchar(128)  NOT NULL,
    [Options] tinyint  NOT NULL,
    [Notes] ntext  NOT NULL,
    [Deleted] bit  NOT NULL,
    [ParentId] uniqueidentifier  NULL,
    [UpstreamId] uniqueidentifier NULL,
    [LastSynchronized] datetime  NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'Directories'
ALTER TABLE [Directories] ADD CONSTRAINT [PK_Directories] PRIMARY KEY ([Id]);
GO

-- Creating foreign key on [RootDirectoryId] in table 'Volumes'
ALTER TABLE [Volumes] ADD CONSTRAINT [FK_VolumeDirectory] FOREIGN KEY ([RootDirectoryId]) REFERENCES [Directories] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VolumeDirectory'
CREATE INDEX [IX_FK_VolumeDirectory] ON [Volumes] ([RootDirectoryId]);
GO

-- Creating foreign key on [ParentId] in table 'Directories'
ALTER TABLE [Directories] ADD CONSTRAINT [FK_DirectoryParent] FOREIGN KEY ([ParentId]) REFERENCES [Directories] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DirectoryParent'
CREATE INDEX [IX_FK_DirectoryParent] ON [Directories] ([ParentId]);
GO

-- Creating table 'Files'
CREATE TABLE [Files] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] nvarchar(128)  NOT NULL,
    [Options] tinyint  NOT NULL,
    [LastAccessed] datetime  NOT NULL,
    [LastHashCalculation] datetime NULL,
    [Notes] ntext  NOT NULL,
    [Deleted] bit  NOT NULL,
    [ContentInfoId] uniqueidentifier  NOT NULL,
    [ParentId] uniqueidentifier  NULL,
    [UpstreamId] uniqueidentifier NULL,
    [LastSynchronized] datetime  NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'Files'
ALTER TABLE [Files] ADD CONSTRAINT [PK_Files] PRIMARY KEY ([Id]);
GO

-- Creating foreign key on [ParentId] in table 'Files'
ALTER TABLE [Files] ADD CONSTRAINT [FK_DirectoryFile] FOREIGN KEY ([ParentId]) REFERENCES [Directories] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DirectoryFile'
CREATE INDEX [IX_FK_DirectoryFile] ON [Files] ([ParentId]);
GO

-- Creating table 'ContentInfos'
CREATE TABLE [ContentInfos] (
    [Id] uniqueidentifier  NOT NULL,
    [Length] bigint  NOT NULL,
    [Data] binary(16)  NULL,
    [UpstreamId] uniqueidentifier NULL,
    [LastSynchronized] datetime  NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'ContentInfo'
ALTER TABLE [ContentInfos] ADD CONSTRAINT [PK_ContentInfo] PRIMARY KEY ([Id]);
GO

-- Creating foreign key on [ContentInfoId] in table 'Files'
ALTER TABLE [Files] ADD CONSTRAINT [FK_ContentInfoFile] FOREIGN KEY ([ContentInfoId]) REFERENCES [ContentInfos] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ContentInfoFile'
CREATE INDEX [IX_FK_ContentInfoFile] ON [Files] ([ContentInfoId]);
GO

-- Creating table 'RedundantSets'
CREATE TABLE [RedundantSets] (
    [Id] uniqueidentifier  NOT NULL,
    [Notes] ntext  NOT NULL,
    [UpstreamId] uniqueidentifier NULL,
    [LastSynchronized] datetime  NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'RedundantSets'
ALTER TABLE [RedundantSets] ADD CONSTRAINT [PK_RedundantSets] PRIMARY KEY ([Id]);
GO

-- Creating table 'Redundancies'
CREATE TABLE [Redundancies] (
    [FileId] uniqueidentifier  NOT NULL,
    [RedundantSetId] uniqueidentifier  NOT NULL,
    [Status] tinyint  NOT NULL,
    [Notes] ntext  NOT NULL,
    [UpstreamId] uniqueidentifier NULL,
    [LastSynchronized] datetime  NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'Redundancies'
ALTER TABLE [Redundancies] ADD CONSTRAINT [PK_Redundancies] PRIMARY KEY ([FileId], [RedundantSetId]);
GO

-- Creating foreign key on [RedundantSetId] in table 'Redundancies'
ALTER TABLE [Redundancies] ADD CONSTRAINT [FK_RedundantSetRedundancy] FOREIGN KEY ([RedundantSetId]) REFERENCES [RedundantSets] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RedundantSetRedundancy'
CREATE INDEX [IX_FK_RedundantSetRedundancy] ON [Redundancies] ([RedundantSetId]);
GO

-- Creating foreign key on [FileId] in table 'Redundancies'
ALTER TABLE [Redundancies] ADD CONSTRAINT [FK_FileRedundancy] FOREIGN KEY ([FileId]) REFERENCES [Files] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FileRedundancy'
CREATE INDEX [IX_FK_FileRedundancy] ON [Redundancies] ([FileId]);
GO

-- Creating table 'Comparisons'
CREATE TABLE [Comparisons] (
    [Id] uniqueidentifier  NOT NULL,
    [SourceFileId] uniqueidentifier  NOT NULL,
    [TargetFileId] uniqueidentifier  NOT NULL,
    [AreEqual] bit  NOT NULL,
    [UpstreamId] uniqueidentifier NULL,
    [LastSynchronized] datetime  NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'Comparisons'
ALTER TABLE [Comparisons] ADD CONSTRAINT [PK_Comparisons] PRIMARY KEY ([Id]);
GO

-- Creating foreign key on [SourceFileId] in table 'Comparisons'
ALTER TABLE [Comparisons] ADD CONSTRAINT [FK_SourceFileComparison] FOREIGN KEY ([SourceFileId]) REFERENCES [Files] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SourceFileComparison'
CREATE INDEX [IX_FK_SourceFileComparison] ON [Comparisons] ([SourceFileId]);
GO

-- Creating foreign key on [TargetFileId] in table 'Comparisons'
ALTER TABLE [Comparisons] ADD CONSTRAINT [FK_TargetFileComparison] FOREIGN KEY ([TargetFileId]) REFERENCES [Files] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TargetFileComparison'
CREATE INDEX [IX_FK_TargetFileComparison] ON [Comparisons] ([TargetFileId]);
GO
