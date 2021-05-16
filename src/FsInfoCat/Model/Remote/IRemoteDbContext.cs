using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FsInfoCat.Model.Remote
{
    public interface IRemoteDbContext : IDbContext
    {
        new IQueryable<IRemoteHashCalculation> Checksums { get; }
        new IQueryable<IRemoteFileComparison> Comparisons { get; }
        new IQueryable<IRemoteFile> Files { get; }
        new IQueryable<IRemoteSubDirectory> Subdirectories { get; }
        new IQueryable<IRemoteVolume> Volumes { get; }
        new IQueryable<IRemoteSymbolicName> FsSymbolicNames { get; }
        new IQueryable<IRemoteFileSystem> FileSystems { get; }
        IQueryable<IHostDevice> HostDevices { get; }
        IQueryable<IHostPlatform> HostPlatforms { get; }
        new IQueryable<IRemoteRedundancy> Redundancies { get; }
        IQueryable<IFileRelocateTask> FileRelocateTasks { get; }
        IQueryable<IDirectoryRelocateTask> DirectoryRelocateTasks { get; }
        IQueryable<IUserProfile> UserProfiles { get; }
        IQueryable<IUserGroup> UserGroups { get; }
    }
}
