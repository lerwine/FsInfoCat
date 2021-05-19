ALTER TABLE [Files] DROP CONSTRAINT [FK_DirectoryFile];
GO
ALTER TABLE [Directories] DROP CONSTRAINT [FK_DirectoryParent];
GO
ALTER TABLE [Files] DROP CONSTRAINT [FK_ContentInfoFile];
GO
ALTER TABLE [Comparisons] DROP CONSTRAINT [FK_SourceFileComparison];
GO
ALTER TABLE [Comparisons] DROP CONSTRAINT [FK_TargetFileComparison];
GO
ALTER TABLE [SymbolicNames] DROP CONSTRAINT [FK_FileSystemSymbolicName];
GO
ALTER TABLE [Redundancies] DROP CONSTRAINT [FK_FileRedundancy];
GO
DROP TABLE [Volumes];
GO
DROP TABLE [Directories];
GO
DROP TABLE [Files];
GO
DROP TABLE [ContentInfos];
GO
DROP TABLE [Redundancies];
GO
DROP TABLE [RedundantSets];
GO
DROP TABLE [FileSystems];
GO
DROP TABLE [Comparisons];
GO
DROP TABLE [SymbolicNames];
GO
