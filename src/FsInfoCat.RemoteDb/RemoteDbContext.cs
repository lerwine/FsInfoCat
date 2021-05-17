using FsInfoCat.Model;
using FsInfoCat.Model.Remote;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace FsInfoCat.RemoteDb
{
    public class RemoteDbContext : IRemoteDbContext
    {
        public DbSet<HashCalculation> Checksums { get; set; }

        IQueryable<IHashCalculation> IDbContext.Checksums => Checksums;

        IQueryable<IRemoteHashCalculation> IRemoteDbContext.Checksums => Checksums;

        public DbSet<FileComparison> Comparisons { get; set; }

        public DbSet<FsFile> Files { get; set; }

        public DbSet<FsDirectory> Subdirectories { get; set; }

        public DbSet<Volume> Volumes { get; set; }

        public DbSet<SymbolicName> FsSymbolicNames { get; set; }

        public DbSet<FileSystem> FileSystems { get; set; }

        public DbSet<HostDevice> HostDevices { get; set; }

        public DbSet<HostPlatform> HostPlatforms { get; set; }

        public DbSet<Redundancy> Redundancies { get; set; }

        public DbSet<FileRelocateTask> FileRelocateTasks { get; set; }

        public DbSet<DirectoryRelocateTask> DirectoryRelocateTasks { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<UserGroup> UserGroups { get; set; }

        IQueryable<IFileComparison> IDbContext.Comparisons => Comparisons;

        IQueryable<IRemoteFileComparison> IRemoteDbContext.Comparisons => Comparisons;

        IQueryable<IFile> IDbContext.Files => Files;

        IQueryable<IRemoteFile> IRemoteDbContext.Files => Files;

        IQueryable<ISubDirectory> IDbContext.Subdirectories => Subdirectories;

        IQueryable<IRemoteSubDirectory> IRemoteDbContext.Subdirectories => Subdirectories;

        IQueryable<IVolume> IDbContext.Volumes => Volumes;

        IQueryable<IRemoteVolume> IRemoteDbContext.Volumes => Volumes;

        IQueryable<IFsSymbolicName> IDbContext.FsSymbolicNames => FsSymbolicNames;

        IQueryable<IRemoteSymbolicName> IRemoteDbContext.FsSymbolicNames => FsSymbolicNames;

        IQueryable<IFileSystem> IDbContext.FileSystems => FileSystems;

        IQueryable<IRemoteFileSystem> IRemoteDbContext.FileSystems => FileSystems;

        IQueryable<IRedundancy> IDbContext.Redundancies => Redundancies;

        IQueryable<IRemoteRedundancy> IRemoteDbContext.Redundancies => Redundancies;

        IQueryable<IHostDevice> IRemoteDbContext.HostDevices => HostDevices;

        IQueryable<IHostPlatform> IRemoteDbContext.HostPlatforms => HostPlatforms;

        IQueryable<IFileRelocateTask> IRemoteDbContext.FileRelocateTasks => FileRelocateTasks;

        IQueryable<IDirectoryRelocateTask> IRemoteDbContext.DirectoryRelocateTasks => DirectoryRelocateTasks;

        IQueryable<IUserProfile> IRemoteDbContext.UserProfiles => UserProfiles;

        IQueryable<IUserGroup> IRemoteDbContext.UserGroups => UserGroups;
    }
}
