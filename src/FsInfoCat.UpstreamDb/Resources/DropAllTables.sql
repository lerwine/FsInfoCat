ALTER TABLE [dbo].[Comparisons] DROP CONSTRAINT [FK_SourceFileComparison];
GO
ALTER TABLE [dbo].[Comparisons] DROP CONSTRAINT [FK_TargetFileComparison];
GO
ALTER TABLE [dbo].[Comparisons] DROP CONSTRAINT [FK_CreatedByComparison];
GO
ALTER TABLE [dbo].[Comparisons] DROP CONSTRAINT [FK_ModifiedByComparison];
GO
ALTER TABLE [dbo].[ContentInfos] DROP CONSTRAINT [FK_CreatedByContentInfo];
GO
ALTER TABLE [dbo].[ContentInfos] DROP CONSTRAINT [FK_ModifiedByContentInfo];
GO
ALTER TABLE [dbo].[Directories] DROP CONSTRAINT [FK_DirectoryParent];
GO
ALTER TABLE [dbo].[Directories] DROP CONSTRAINT [FK_CreatedByDirectory];
GO
ALTER TABLE [dbo].[Directories] DROP CONSTRAINT [FK_ModifiedByDirectory];
GO
ALTER TABLE [dbo].[Directories] DROP CONSTRAINT [FK_SourceDirectoryRelocateTask];
GO
ALTER TABLE [dbo].[DirectoryRelocateTasks] DROP CONSTRAINT [FK_UserGroupDirectoryRelocateTask];
GO
ALTER TABLE [dbo].[DirectoryRelocateTasks] DROP CONSTRAINT [FK_UserProfileDirectoryRelocateTask];
GO
ALTER TABLE [dbo].[DirectoryRelocateTasks] DROP CONSTRAINT [FK_CreatedByDirectoryRelocateTask];
GO
ALTER TABLE [dbo].[DirectoryRelocateTasks] DROP CONSTRAINT [FK_ModifiedByDirectoryRelocateTask];
GO
ALTER TABLE [dbo].[DirectoryRelocateTasks] DROP CONSTRAINT [FK_TargetDirectoryRelocateTask];
GO
ALTER TABLE [dbo].[FileRelocateTasks] DROP CONSTRAINT [FK_DirectoryFileRelocateTask];
GO
ALTER TABLE [dbo].[FileRelocateTasks] DROP CONSTRAINT [FK_UserGroupFileRelocateTask];
GO
ALTER TABLE [dbo].[FileRelocateTasks] DROP CONSTRAINT [FK_UserProfileFileRelocateTask];
GO
ALTER TABLE [dbo].[FileRelocateTasks] DROP CONSTRAINT [FK_CreatedByFileRelocateTask];
GO
ALTER TABLE [dbo].[FileRelocateTasks] DROP CONSTRAINT [FK_ModifiedByFileRelocateTask];
GO
ALTER TABLE [dbo].[Files] DROP CONSTRAINT [FK_DirectoryFile];
GO
ALTER TABLE [dbo].[Files] DROP CONSTRAINT [FK_CreatedByFile];
GO
ALTER TABLE [dbo].[Files] DROP CONSTRAINT [FK_ModifiedByFile];
GO
ALTER TABLE [dbo].[Files] DROP CONSTRAINT [FK_FileRelocateTaskFile];
GO
ALTER TABLE [dbo].[Files] DROP CONSTRAINT [FK_ContentInfoFile];
GO
ALTER TABLE [dbo].[FileSystems] DROP CONSTRAINT [FK_CreatedByFileSystem];
GO
ALTER TABLE [dbo].[FileSystems] DROP CONSTRAINT [FK_ModifiedByFileSystem];
GO
ALTER TABLE [dbo].[FileSystems] DROP CONSTRAINT [FK_SymbolicNameFileSystem];
GO
ALTER TABLE [dbo].[HostDevices] DROP CONSTRAINT [FK_HostPlatformHostDevice];
GO
ALTER TABLE [dbo].[HostDevices] DROP CONSTRAINT [FK_CreatedByHostDevice];
GO
ALTER TABLE [dbo].[HostDevices] DROP CONSTRAINT [FK_ModifiedByHostDevice];
GO
ALTER TABLE [dbo].[HostPlatforms] DROP CONSTRAINT [FK_CreatedByHostPlatform];
GO
ALTER TABLE [dbo].[HostPlatforms] DROP CONSTRAINT [FK_ModifiedByHostPlatform];
GO
ALTER TABLE [dbo].[HostPlatforms] DROP CONSTRAINT [FK_DefaultFSTypeHostPlatform];
GO
ALTER TABLE [dbo].[Redundancies] DROP CONSTRAINT [FK_RedundantSetRedundancy];
GO
ALTER TABLE [dbo].[Redundancies] DROP CONSTRAINT [FK_FileRedundancy];
GO
ALTER TABLE [dbo].[Redundancies] DROP CONSTRAINT [FK_CreatedByRedundancy];
GO
ALTER TABLE [dbo].[Redundancies] DROP CONSTRAINT [FK_ModifiedByRedundancy];
GO
ALTER TABLE [dbo].[RedundantSets] DROP CONSTRAINT [FK_CreatedByRedundantSet];
GO
ALTER TABLE [dbo].[RedundantSets] DROP CONSTRAINT [FK_ModifiedByRedundantSet];
GO
ALTER TABLE [dbo].[SymbolicNames] DROP CONSTRAINT [FK_FileSystemSymbolicName];
GO
ALTER TABLE [dbo].[SymbolicNames] DROP CONSTRAINT [FK_CreatedBySymbolicName];
GO
ALTER TABLE [dbo].[SymbolicNames] DROP CONSTRAINT [FK_ModifiedBySymbolicName];
GO
ALTER TABLE [dbo].[UserGroups] DROP CONSTRAINT [FK_CreatedByUserGroup];
GO
ALTER TABLE [dbo].[UserGroups] DROP CONSTRAINT [FK_ModifiedByUserGroup];
GO
ALTER TABLE [dbo].[UserGroupUserProfile] DROP CONSTRAINT [FK_UserGroupUserProfile_UserGroup];
GO
ALTER TABLE [dbo].[UserGroupUserProfile] DROP CONSTRAINT [FK_UserGroupUserProfile_UserProfile];
GO
ALTER TABLE [dbo].[UserProfiles] DROP CONSTRAINT [FK_CreatedByUserProfile];
GO
ALTER TABLE [dbo].[UserProfiles] DROP CONSTRAINT [FK_ModifiedByUserProfile];
GO
ALTER TABLE [dbo].[Volumes] DROP CONSTRAINT [FK_HostDeviceVolume];
GO
ALTER TABLE [dbo].[Volumes] DROP CONSTRAINT [FK_FileSystemVolume];
GO
ALTER TABLE [dbo].[Volumes] DROP CONSTRAINT [FK_CreatedByVolume];
GO
ALTER TABLE [dbo].[Volumes] DROP CONSTRAINT [FK_ModifiedByVolume];
GO
ALTER TABLE [dbo].[Volumes] DROP CONSTRAINT [FK_VolumeDirectory];
GO
DROP TABLE [dbo].[Comparisons];
GO
DROP TABLE [dbo].[ContentInfos];
GO
DROP TABLE [dbo].[Directories];
GO
DROP TABLE [dbo].[DirectoryRelocateTasks];
GO
DROP TABLE [dbo].[FileRelocateTasks];
GO
DROP TABLE [dbo].[Files];
GO
DROP TABLE [dbo].[FileSystems];
GO
DROP TABLE [dbo].[HostDevices];
GO
DROP TABLE [dbo].[HostPlatforms];
GO
DROP TABLE [dbo].[Redundancies];
GO
DROP TABLE [dbo].[RedundantSets];
GO
DROP TABLE [dbo].[SymbolicNames];
GO
DROP TABLE [dbo].[UserGroups];
GO
DROP TABLE [dbo].[UserGroupUserProfile];
GO
DROP TABLE [dbo].[UserProfiles];
GO
DROP TABLE [dbo].[Volumes];
