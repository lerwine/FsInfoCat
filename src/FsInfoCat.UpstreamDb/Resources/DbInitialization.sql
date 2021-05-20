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
    [Notes] ntext  NOT NULL,
    [IsInactive] bit  NOT NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'UserProfiles'
ALTER TABLE [dbo].[UserProfiles] ADD CONSTRAINT [PK_UserProfiles] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating foreign key on [CreatedById] in table 'UserProfiles'
ALTER TABLE [dbo].[UserProfiles] ADD CONSTRAINT [FK_CreatedByUserProfile] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByUserProfile'
CREATE INDEX [FK_CreatedByUserProfile] ON [dbo].[UserProfiles] ([CreatedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'UserProfiles'
ALTER TABLE [dbo].[UserProfiles] ADD CONSTRAINT [FK_ModifiedByUserProfile] FOREIGN KEY ([ModifiedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByUserProfile'
CREATE INDEX [FK_ModifiedByUserProfile] ON [dbo].[UserProfiles] ([ModifiedById]);
GO

-- Creating table 'UserGroups'
CREATE TABLE [dbo].[UserGroups] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] nvarchar(128)  NOT NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [Roles] tinyint  NOT NULL,
    [Notes] ntext  NOT NULL,
    [IsInactive] bit  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'UserGroups'
ALTER TABLE [dbo].[UserGroups] ADD CONSTRAINT [PK_UserGroups] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating foreign key on [CreatedById] in table 'UserGroups'
ALTER TABLE [dbo].[UserGroups] ADD CONSTRAINT [FK_CreatedByUserGroup] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByUserGroup'
CREATE INDEX [FK_CreatedByUserGroup] ON [dbo].[UserGroups] ([CreatedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'UserGroups'
ALTER TABLE [dbo].[UserGroups] ADD CONSTRAINT [FK_ModifiedByUserGroup] FOREIGN KEY ([ModifiedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByUserGroup'
CREATE INDEX [FK_ModifiedByUserGroup] ON [dbo].[UserGroups] ([ModifiedById]);
GO

-- Creating table 'UserGroupUserProfile'
CREATE TABLE [dbo].[UserGroupUserProfile] (
    [AssignmentGroups_Id] uniqueidentifier  NOT NULL,
    [Members_Id] uniqueidentifier  NOT NULL
);
GO

-- Creating primary key on [AssignmentGroups_Id], [Members_Id] in table 'UserGroupUserProfile'
ALTER TABLE [dbo].[UserGroupUserProfile] ADD CONSTRAINT [PK_UserGroupUserProfile] PRIMARY KEY CLUSTERED ([AssignmentGroups_Id], [Members_Id] ASC);
GO

-- Creating foreign key on [AssignmentGroups_Id] in table 'UserGroupUserProfile'
ALTER TABLE [dbo].[UserGroupUserProfile] ADD CONSTRAINT [FK_UserGroupUserProfile_UserGroup] FOREIGN KEY ([AssignmentGroups_Id]) REFERENCES [dbo].[UserGroups] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Members_Id] in table 'UserGroupUserProfile'
ALTER TABLE [dbo].[UserGroupUserProfile] ADD CONSTRAINT [FK_UserGroupUserProfile_UserProfile] FOREIGN KEY ([Members_Id]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserGroupUserProfile_UserProfile'
CREATE INDEX [IX_FK_UserGroupUserProfile_UserProfile] ON [dbo].[UserGroupUserProfile] ([Members_Id]);
GO

-- Creating table 'HostPlatforms'
CREATE TABLE [dbo].[HostPlatforms] (
    [Id] uniqueidentifier  NOT NULL,
    [DisplayName] nvarchar(128)  NOT NULL,
    [Type] tinyint  NOT NULL,
    [DefaultFsTypeId] uniqueidentifier  NULL,
    [Notes] ntext  NOT NULL,
    [IsInactive] bit  NOT NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'HostPlatforms'
ALTER TABLE [dbo].[HostPlatforms] ADD CONSTRAINT [PK_HostPlatforms] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating foreign key on [CreatedById] in table 'HostPlatforms'
ALTER TABLE [dbo].[HostPlatforms] ADD CONSTRAINT [FK_CreatedByHostPlatform] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByHostPlatform'
CREATE INDEX [FK_CreatedByHostPlatform] ON [dbo].[HostPlatforms] ([CreatedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'HostPlatforms'
ALTER TABLE [dbo].[HostPlatforms] ADD CONSTRAINT [FK_ModifiedByHostPlatform] FOREIGN KEY ([ModifiedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByHostPlatform'
CREATE INDEX [FK_ModifiedByHostPlatform] ON [dbo].[HostPlatforms] ([ModifiedById]);
GO

-- Creating table 'HostDevices'
CREATE TABLE [dbo].[HostDevices] (
    [Id] uniqueidentifier  NOT NULL,
    [DisplayName] nvarchar(128)  NOT NULL,
    [MachineIdentifer] nvarchar(128)  NOT NULL,
    [MachineName] nvarchar(128)  NOT NULL,
    [PlatformId] uniqueidentifier  NOT NULL,
    [Notes] ntext  NOT NULL,
    [IsInactive] bit  NOT NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'HostDevices'
ALTER TABLE [dbo].[HostDevices] ADD CONSTRAINT [PK_HostDevices] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating foreign key on [PlatformId] in table 'HostDevices'
ALTER TABLE [dbo].[HostDevices] ADD CONSTRAINT [FK_HostPlatformHostDevice] FOREIGN KEY ([PlatformId]) REFERENCES [dbo].[HostPlatforms] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HostPlatformHostDevice'
CREATE INDEX [IX_FK_HostPlatformHostDevice] ON [dbo].[HostDevices] ([PlatformId]);
GO

-- Creating foreign key on [CreatedById] in table 'HostDevices'
ALTER TABLE [dbo].[HostDevices] ADD CONSTRAINT [FK_CreatedByHostDevice] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByHostDevice'
CREATE INDEX [FK_CreatedByHostDevice] ON [dbo].[HostDevices] ([CreatedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'HostDevices'
ALTER TABLE [dbo].[HostDevices] ADD CONSTRAINT [FK_ModifiedByHostDevice] FOREIGN KEY ([ModifiedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByHostDevice'
CREATE INDEX [FK_ModifiedByHostDevice] ON [dbo].[HostDevices] ([ModifiedById]);
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
    [Notes] ntext  NOT NULL,
    [IsInactive] bit  NOT NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'FileSystems'
ALTER TABLE [dbo].[FileSystems] ADD CONSTRAINT [PK_FileSystems] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating foreign key on [DefaultFsTypeId] in table 'HostPlatforms'
ALTER TABLE [dbo].[HostPlatforms] ADD CONSTRAINT [FK_DefaultFSTypeHostPlatform] FOREIGN KEY ([DefaultFsTypeId]) REFERENCES [dbo].[FileSystems] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DefaultFSTypeHostPlatform'
CREATE INDEX [IX_FK_DefaultFSTypeHostPlatform] ON [dbo].[HostPlatforms] ([DefaultFsTypeId]);
GO

-- Creating foreign key on [CreatedById] in table 'FileSystems'
ALTER TABLE [dbo].[FileSystems] ADD CONSTRAINT [FK_CreatedByFileSystem] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByFileSystem'
CREATE INDEX [FK_CreatedByFileSystem] ON [dbo].[FileSystems] ([CreatedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'FileSystems'
ALTER TABLE [dbo].[FileSystems] ADD CONSTRAINT [FK_ModifiedByFileSystem] FOREIGN KEY ([ModifiedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByFileSystem'
CREATE INDEX [FK_ModifiedByFileSystem] ON [dbo].[FileSystems] ([ModifiedById]);
GO

-- Creating table 'SymbolicNames'
CREATE TABLE [dbo].[SymbolicNames] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] nvarchar(128)  NOT NULL,
    [FileSystemId] uniqueidentifier  NOT NULL,
    [Notes] ntext  NOT NULL,
    [IsInactive] bit  NOT NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'SymbolicNames'
ALTER TABLE [dbo].[SymbolicNames] ADD CONSTRAINT [PK_SymbolicNames] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating foreign key on [FileSystemId] in table 'SymbolicNames'
ALTER TABLE [dbo].[SymbolicNames] ADD CONSTRAINT [FK_FileSystemSymbolicName] FOREIGN KEY ([FileSystemId]) REFERENCES [dbo].[FileSystems] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FileSystemSymbolicName'
CREATE INDEX [IX_FK_FileSystemSymbolicName] ON [dbo].[SymbolicNames] ([FileSystemId]);
GO

-- Creating foreign key on [DefaultSymbolicNameId] in table 'FileSystems'
ALTER TABLE [dbo].[FileSystems] ADD CONSTRAINT [FK_SymbolicNameFileSystem] FOREIGN KEY ([DefaultSymbolicNameId]) REFERENCES [dbo].[SymbolicNames] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SymbolicNameFileSystem'
CREATE INDEX [IX_FK_SymbolicNameFileSystem] ON [dbo].[FileSystems] ([DefaultSymbolicNameId]);
GO

-- Creating foreign key on [CreatedById] in table 'SymbolicNames'
ALTER TABLE [dbo].[SymbolicNames] ADD CONSTRAINT [FK_CreatedBySymbolicName] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedBySymbolicName'
CREATE INDEX [FK_CreatedBySymbolicName] ON [dbo].[SymbolicNames] ([CreatedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'SymbolicNames'
ALTER TABLE [dbo].[SymbolicNames] ADD CONSTRAINT [FK_ModifiedBySymbolicName] FOREIGN KEY ([ModifiedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedBySymbolicName'
CREATE INDEX [FK_ModifiedBySymbolicName] ON [dbo].[SymbolicNames] ([ModifiedById]);
GO

-- Creating table 'Volumes'
CREATE TABLE [dbo].[Volumes] (
    [Id] uniqueidentifier  NOT NULL,
    [DisplayName] nvarchar(128)  NOT NULL,
    [VolumeName] nvarchar(128)  NOT NULL,
    [Identifier] nvarchar(1024)  NOT NULL,
    [FileSystemId] uniqueidentifier  NOT NULL,
    [HostDeviceId] uniqueidentifier  NULL,
    [RootDirectoryId] uniqueidentifier  NOT NULL,
    [Type] tinyint  NOT NULL,
    [CaseSensitiveSearch] bit  NULL,
    [ReadOnly] bit  NULL,
    [MaxNameLength] bigint  NULL,
    [Notes] ntext  NOT NULL,
    [IsInactive] bit  NOT NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'Volumes'
ALTER TABLE [dbo].[Volumes] ADD CONSTRAINT [PK_Volumes] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating foreign key on [HostDeviceId] in table 'Volumes'
ALTER TABLE [dbo].[Volumes] ADD CONSTRAINT [FK_HostDeviceVolume] FOREIGN KEY ([HostDeviceId]) REFERENCES [dbo].[HostDevices] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HostDeviceVolume'
CREATE INDEX [IX_FK_HostDeviceVolume] ON [dbo].[Volumes] ([HostDeviceId]);
GO

-- Creating foreign key on [FileSystemId] in table 'Volumes'
ALTER TABLE [dbo].[Volumes] ADD CONSTRAINT [FK_FileSystemVolume] FOREIGN KEY ([FileSystemId]) REFERENCES [dbo].[FileSystems] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FileSystemVolume'
CREATE INDEX [IX_FK_FileSystemVolume] ON [dbo].[Volumes] ([FileSystemId]);
GO

-- Creating foreign key on [CreatedById] in table 'Volumes'
ALTER TABLE [dbo].[Volumes] ADD CONSTRAINT [FK_CreatedByVolume] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByVolume'
CREATE INDEX [FK_CreatedByVolume] ON [dbo].[Volumes] ([CreatedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'Volumes'
ALTER TABLE [dbo].[Volumes] ADD CONSTRAINT [FK_ModifiedByVolume] FOREIGN KEY ([ModifiedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByVolume'
CREATE INDEX [FK_ModifiedByVolume] ON [dbo].[Volumes] ([ModifiedById]);
GO

-- Creating table 'Directories'
CREATE TABLE [dbo].[Directories] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] nvarchar(128)  NOT NULL,
    [Options] tinyint  NOT NULL,
    [Notes] ntext  NOT NULL,
    [Deleted] bit  NOT NULL,
    [ParentId] uniqueidentifier  NULL,
    [SourceRelocationTaskId] uniqueidentifier  NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'Directories'
ALTER TABLE [dbo].[Directories] ADD CONSTRAINT [PK_Directories] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating foreign key on [RootDirectoryId] in table 'Volumes'
ALTER TABLE [dbo].[Volumes] ADD CONSTRAINT [FK_VolumeDirectory] FOREIGN KEY ([RootDirectoryId]) REFERENCES [dbo].[Directories] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VolumeDirectory'
CREATE INDEX [IX_FK_VolumeDirectory] ON [dbo].[Volumes] ([RootDirectoryId]);
GO

-- Creating foreign key on [ParentId] in table 'Directories'
ALTER TABLE [dbo].[Directories] ADD CONSTRAINT [FK_DirectoryParent] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[Directories] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DirectoryParent'
CREATE INDEX [IX_FK_DirectoryParent] ON [dbo].[Directories] ([ParentId]);
GO

-- Creating foreign key on [CreatedById] in table 'Directories'
ALTER TABLE [dbo].[Directories] ADD CONSTRAINT [FK_CreatedByDirectory] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByDirectory'
CREATE INDEX [FK_CreatedByDirectory] ON [dbo].[Directories] ([CreatedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'Directories'
ALTER TABLE [dbo].[Directories] ADD CONSTRAINT [FK_ModifiedByDirectory] FOREIGN KEY ([ModifiedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByDirectory'
CREATE INDEX [FK_ModifiedByDirectory] ON [dbo].[Directories] ([ModifiedById]);
GO

-- Creating table 'DirectoryRelocateTasks'
CREATE TABLE [dbo].[DirectoryRelocateTasks] (
    [Id] uniqueidentifier  NOT NULL,
    [Status] tinyint  NOT NULL,
    [Priority] tinyint  NOT NULL,
    [ShortDescription] nvarchar(1024)  NOT NULL,
    [AssignmentGroupId] uniqueidentifier  NULL,
    [AssignedToId] uniqueidentifier  NULL,
    [Notes] ntext  NOT NULL,
    [IsInactive] bit  NOT NULL,
    [TargetDirectoryId] uniqueidentifier  NOT NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'DirectoryRelocateTasks'
ALTER TABLE [dbo].[DirectoryRelocateTasks] ADD CONSTRAINT [PK_DirectoryRelocateTasks] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating foreign key on [SourceRelocationTaskId] in table 'Directories'
ALTER TABLE [dbo].[Directories] ADD CONSTRAINT [FK_SourceDirectoryRelocateTask] FOREIGN KEY ([SourceRelocationTaskId]) REFERENCES [dbo].[DirectoryRelocateTasks] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SourceDirectoryRelocateTask'
CREATE INDEX [IX_FK_SourceDirectoryRelocateTask] ON [dbo].[Directories] ([SourceRelocationTaskId]);
GO

-- Creating foreign key on [AssignmentGroupId] in table 'DirectoryRelocateTasks'
ALTER TABLE [dbo].[DirectoryRelocateTasks] ADD CONSTRAINT [FK_UserGroupDirectoryRelocateTask] FOREIGN KEY ([AssignmentGroupId]) REFERENCES [dbo].[UserGroups] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserGroupDirectoryRelocateTask'
CREATE INDEX [IX_FK_UserGroupDirectoryRelocateTask] ON [dbo].[DirectoryRelocateTasks] ([AssignmentGroupId]);
GO

-- Creating foreign key on [AssignedToId] in table 'DirectoryRelocateTasks'
ALTER TABLE [dbo].[DirectoryRelocateTasks] ADD CONSTRAINT [FK_UserProfileDirectoryRelocateTask] FOREIGN KEY ([AssignedToId]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserProfileDirectoryRelocateTask'
CREATE INDEX [IX_FK_UserProfileDirectoryRelocateTask] ON [dbo].[DirectoryRelocateTasks] ([AssignedToId]);
GO

-- Creating foreign key on [CreatedById] in table 'DirectoryRelocateTasks'
ALTER TABLE [dbo].[DirectoryRelocateTasks] ADD CONSTRAINT [FK_CreatedByDirectoryRelocateTask] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByDirectoryRelocateTask'
CREATE INDEX [FK_CreatedByDirectoryRelocateTask] ON [dbo].[DirectoryRelocateTasks] ([CreatedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'DirectoryRelocateTasks'
ALTER TABLE [dbo].[DirectoryRelocateTasks] ADD CONSTRAINT [FK_ModifiedByDirectoryRelocateTask] FOREIGN KEY ([ModifiedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByDirectoryRelocateTask'
CREATE INDEX [FK_ModifiedByDirectoryRelocateTask] ON [dbo].[DirectoryRelocateTasks] ([ModifiedById]);
GO

-- Creating table 'Files'
CREATE TABLE [dbo].[Files] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] nvarchar(128)  NOT NULL,
    [Options] tinyint  NOT NULL,
    [LastAccessed] datetime  NOT NULL,
    [LastHashCalculation] datetime NULL,
    [Notes] ntext  NOT NULL,
    [Deleted] bit  NOT NULL,
    [ContentInfoId] uniqueidentifier  NOT NULL,
    [ParentId] uniqueidentifier  NULL,
    [FileRelocateTaskId] uniqueidentifier  NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'Files'
ALTER TABLE [dbo].[Files] ADD CONSTRAINT [PK_Files] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating foreign key on [ParentId] in table 'Files'
ALTER TABLE [dbo].[Files] ADD CONSTRAINT [FK_DirectoryFile] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[Directories] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DirectoryFile'
CREATE INDEX [IX_FK_DirectoryFile] ON [dbo].[Files] ([ParentId]);
GO

-- Creating foreign key on [CreatedById] in table 'Files'
ALTER TABLE [dbo].[Files] ADD CONSTRAINT [FK_CreatedByFile] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByFile'
CREATE INDEX [FK_CreatedByFile] ON [dbo].[Files] ([CreatedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'Files'
ALTER TABLE [dbo].[Files] ADD CONSTRAINT [FK_ModifiedByFile] FOREIGN KEY ([ModifiedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByFile'
CREATE INDEX [FK_ModifiedByFile] ON [dbo].[Files] ([ModifiedById]);
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
    [Notes] ntext  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'FileRelocateTasks'
ALTER TABLE [dbo].[FileRelocateTasks] ADD CONSTRAINT [PK_FileRelocateTasks] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating foreign key on [FileRelocateTaskId] in table 'Files'
ALTER TABLE [dbo].[Files] ADD CONSTRAINT [FK_FileRelocateTaskFile] FOREIGN KEY ([FileRelocateTaskId]) REFERENCES [dbo].[FileRelocateTasks] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FileRelocateTaskFile'
CREATE INDEX [IX_FK_FileRelocateTaskFile] ON [dbo].[Files] ([FileRelocateTaskId]);
GO

-- Creating foreign key on [TargetDirectoryId] in table 'FileRelocateTasks'
ALTER TABLE [dbo].[FileRelocateTasks] ADD CONSTRAINT [FK_DirectoryFileRelocateTask] FOREIGN KEY ([TargetDirectoryId]) REFERENCES [dbo].[Directories] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DirectoryFileRelocateTask'
CREATE INDEX [IX_FK_DirectoryFileRelocateTask] ON [dbo].[FileRelocateTasks] ([TargetDirectoryId]);
GO

-- Creating foreign key on [TargetDirectoryId] in table 'DirectoryRelocateTasks'
ALTER TABLE [dbo].[DirectoryRelocateTasks] ADD CONSTRAINT [FK_TargetDirectoryRelocateTask] FOREIGN KEY ([TargetDirectoryId]) REFERENCES [dbo].[Directories] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TargetDirectoryRelocateTask'
CREATE INDEX [IX_FK_TargetDirectoryRelocateTask] ON [dbo].[DirectoryRelocateTasks] ([TargetDirectoryId]);
GO

-- Creating foreign key on [AssignmentGroupId] in table 'FileRelocateTasks'
ALTER TABLE [dbo].[FileRelocateTasks] ADD CONSTRAINT [FK_UserGroupFileRelocateTask] FOREIGN KEY ([AssignmentGroupId]) REFERENCES [dbo].[UserGroups] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserGroupFileRelocateTask'
CREATE INDEX [IX_FK_UserGroupFileRelocateTask] ON [dbo].[FileRelocateTasks] ([AssignmentGroupId]);
GO

-- Creating foreign key on [AssignedToId] in table 'FileRelocateTasks'
ALTER TABLE [dbo].[FileRelocateTasks] ADD CONSTRAINT [FK_UserProfileFileRelocateTask] FOREIGN KEY ([AssignedToId]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserProfileFileRelocateTask'
CREATE INDEX [IX_FK_UserProfileFileRelocateTask] ON [dbo].[FileRelocateTasks] ([AssignedToId]);
GO

-- Creating foreign key on [CreatedById] in table 'FileRelocateTasks'
ALTER TABLE [dbo].[FileRelocateTasks] ADD CONSTRAINT [FK_CreatedByFileRelocateTask] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByFileRelocateTask'
CREATE INDEX [FK_CreatedByFileRelocateTask] ON [dbo].[FileRelocateTasks] ([CreatedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'FileRelocateTasks'
ALTER TABLE [dbo].[FileRelocateTasks] ADD CONSTRAINT [FK_ModifiedByFileRelocateTask] FOREIGN KEY ([ModifiedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByFileRelocateTask'
CREATE INDEX [FK_ModifiedByFileRelocateTask] ON [dbo].[FileRelocateTasks] ([ModifiedById]);
GO

-- Creating table 'ContentInfos'
CREATE TABLE [dbo].[ContentInfos] (
    [Id] uniqueidentifier  NOT NULL,
    [Length] bigint  NOT NULL,
    [Data] binary(16)  NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'ContentInfo'
ALTER TABLE [dbo].[ContentInfos] ADD CONSTRAINT [PK_ContentInfo] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating foreign key on [ContentInfoId] in table 'Files'
ALTER TABLE [dbo].[Files] ADD CONSTRAINT [FK_ContentInfoFile] FOREIGN KEY ([ContentInfoId]) REFERENCES [dbo].[ContentInfos] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ContentInfoFile'
CREATE INDEX [IX_FK_ContentInfoFile] ON [dbo].[Files] ([ContentInfoId]);
GO

-- Creating foreign key on [CreatedById] in table 'ContentInfos'
ALTER TABLE [dbo].[ContentInfos] ADD CONSTRAINT [FK_CreatedByContentInfo] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByContentInfo'
CREATE INDEX [FK_CreatedByContentInfo] ON [dbo].[ContentInfos] ([CreatedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'ContentInfos'
ALTER TABLE [dbo].[ContentInfos] ADD CONSTRAINT [FK_ModifiedByContentInfo] FOREIGN KEY ([ModifiedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByContentInfo'
CREATE INDEX [FK_ModifiedByContentInfo] ON [dbo].[ContentInfos] ([ModifiedById]);
GO

-- Creating table 'RedundantSets'
CREATE TABLE [dbo].[RedundantSets] (
    [Id] uniqueidentifier  NOT NULL,
    [Notes] ntext  NOT NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'RedundantSets'
ALTER TABLE [dbo].[RedundantSets] ADD CONSTRAINT [PK_RedundantSets] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating foreign key on [CreatedById] in table 'RedundantSets'
ALTER TABLE [dbo].[RedundantSets] ADD CONSTRAINT [FK_CreatedByRedundantSet] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByRedundantSet'
CREATE INDEX [FK_CreatedByRedundantSet] ON [dbo].[RedundantSets] ([CreatedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'RedundantSets'
ALTER TABLE [dbo].[RedundantSets] ADD CONSTRAINT [FK_ModifiedByRedundantSet] FOREIGN KEY ([ModifiedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByRedundantSet'
CREATE INDEX [FK_ModifiedByRedundantSet] ON [dbo].[RedundantSets] ([ModifiedById]);
GO

-- Creating table 'Redundancies'
CREATE TABLE [dbo].[Redundancies] (
    [FileId] uniqueidentifier  NOT NULL,
    [RedundantSetId] uniqueidentifier  NOT NULL,
    [Status] tinyint  NOT NULL,
    [Notes] ntext  NOT NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'Redundancies'
ALTER TABLE [dbo].[Redundancies] ADD CONSTRAINT [PK_Redundancies] PRIMARY KEY CLUSTERED ([FileId], [RedundantSetId] ASC);
GO

-- Creating foreign key on [RedundantSetId] in table 'Redundancies'
ALTER TABLE [dbo].[Redundancies] ADD CONSTRAINT [FK_RedundantSetRedundancy] FOREIGN KEY ([RedundantSetId]) REFERENCES [dbo].[RedundantSets] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RedundantSetRedundancy'
CREATE INDEX [IX_FK_RedundantSetRedundancy] ON [dbo].[Redundancies] ([RedundantSetId]);
GO

-- Creating foreign key on [FileId] in table 'Redundancies'
ALTER TABLE [dbo].[Redundancies] ADD CONSTRAINT [FK_FileRedundancy] FOREIGN KEY ([FileId]) REFERENCES [dbo].[Files] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FileRedundancy'
CREATE INDEX [IX_FK_FileRedundancy] ON [dbo].[Redundancies] ([FileId]);
GO

-- Creating foreign key on [CreatedById] in table 'Redundancies'
ALTER TABLE [dbo].[Redundancies] ADD CONSTRAINT [FK_CreatedByRedundancy] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByRedundancy'
CREATE INDEX [FK_CreatedByRedundancy] ON [dbo].[Redundancies] ([CreatedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'Redundancies'
ALTER TABLE [dbo].[Redundancies] ADD CONSTRAINT [FK_ModifiedByRedundancy] FOREIGN KEY ([ModifiedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByRedundancy'
CREATE INDEX [FK_ModifiedByRedundancy] ON [dbo].[Redundancies] ([ModifiedById]);
GO

-- Creating table 'Comparisons'
CREATE TABLE [dbo].[Comparisons] (
    [Id] uniqueidentifier  NOT NULL,
    [SourceFileId] uniqueidentifier  NOT NULL,
    [TargetFileId] uniqueidentifier  NOT NULL,
    [AreEqual] bit  NOT NULL,
    [CreatedById] uniqueidentifier  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [ModifiedById] uniqueidentifier  NOT NULL,
    [ModifiedOn] datetime  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'Comparisons'
ALTER TABLE [dbo].[Comparisons] ADD CONSTRAINT [PK_Comparisons] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating foreign key on [SourceFileId] in table 'Comparisons'
ALTER TABLE [dbo].[Comparisons] ADD CONSTRAINT [FK_SourceFileComparison] FOREIGN KEY ([SourceFileId]) REFERENCES [dbo].[Files] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SourceFileComparison'
CREATE INDEX [IX_FK_SourceFileComparison] ON [dbo].[Comparisons] ([SourceFileId]);
GO

-- Creating foreign key on [TargetFileId] in table 'Comparisons'
ALTER TABLE [dbo].[Comparisons] ADD CONSTRAINT [FK_TargetFileComparison] FOREIGN KEY ([TargetFileId]) REFERENCES [dbo].[Files] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TargetFileComparison'
CREATE INDEX [IX_FK_TargetFileComparison] ON [dbo].[Comparisons] ([TargetFileId]);
GO

-- Creating foreign key on [CreatedById] in table 'Comparisons'
ALTER TABLE [dbo].[Comparisons] ADD CONSTRAINT [FK_CreatedByComparison] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedByComparison'
CREATE INDEX [FK_CreatedByComparison] ON [dbo].[Comparisons] ([CreatedById]);
GO

-- Creating foreign key on [ModifiedById] in table 'Comparisons'
ALTER TABLE [dbo].[Comparisons] ADD CONSTRAINT [FK_ModifiedByComparison] FOREIGN KEY ([ModifiedById]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModifiedByComparison'
CREATE INDEX [FK_ModifiedByComparison] ON [dbo].[Comparisons] ([ModifiedById]);
GO
