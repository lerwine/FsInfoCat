using System.Linq;

namespace FsInfoCat.Model.Upstream
{
    public interface IUpstreamDbContext : IDbContext
    {
        new IQueryable<IUpstreamContentHash> HashCalculations { get; }
        new IQueryable<IUpstreamFileComparison> Comparisons { get; }
        new IQueryable<IUpstreamFile> Files { get; }
        new IQueryable<IUpstreamSubDirectory> Subdirectories { get; }
        new IQueryable<IUpstreamVolume> Volumes { get; }
        new IQueryable<IUpstreamSymbolicName> SymbolicNames { get; }
        new IQueryable<IUpstreamFileSystem> FileSystems { get; }
        IQueryable<IHostDevice> HostDevices { get; }
        IQueryable<IHostPlatform> HostPlatforms { get; }
        new IQueryable<IUpstreamRedundancy> Redundancies { get; }
        IQueryable<IFileRelocateTask> FileRelocateTasks { get; }
        IQueryable<IDirectoryRelocateTask> DirectoryRelocateTasks { get; }
        IQueryable<IUserProfile> UserProfiles { get; }
        IQueryable<IUserGroup> UserGroups { get; }
        IQueryable<IUserGroupMembership> UserGroupMemberships { get; }

        void AddHashCalculation(IUpstreamContentHash hashCalculation);
        void UpdateHashCalculation(IUpstreamContentHash hashCalculation);
        void RemoveHashCalculation(IUpstreamContentHash hashCalculation);
        void AddComparison(IUpstreamFileComparison fileComparison);
        void UpdateComparison(IUpstreamFileComparison fileComparison);
        void RemoveComparison(IUpstreamFileComparison fileComparison);
        void AddFile(IUpstreamFile file);
        void UpdateFile(IUpstreamFile file);
        void RemoveFile(IUpstreamFile file);
        void AddSubDirectory(IUpstreamSubDirectory subDirectory);
        void UpdateSubDirectory(IUpstreamSubDirectory subDirectory);
        void RemoveSubDirectory(IUpstreamSubDirectory subDirectory);
        void AddVolume(IUpstreamVolume volume);
        void UpdateVolume(IUpstreamVolume volume);
        void RemoveVolume(IUpstreamVolume volume);
        void AddSymbolicName(IUpstreamSymbolicName symbolicName);
        void UpdateSymbolicName(IUpstreamSymbolicName symbolicName);
        void RemoveSymbolicName(IUpstreamSymbolicName symbolicName);
        void AddFileSystem(IUpstreamFileSystem fileSystem);
        void UpdateFileSystem(IUpstreamFileSystem fileSystem);
        void RemoveFileSystem(IUpstreamFileSystem fileSystem);
        void AddHostDevice(IHostDevice hostDevice);
        void UpdateHostDevice(IHostDevice hostDevice);
        void RemoveHostDevice(IHostDevice hostDevice);
        void AddHostPlatform(IHostPlatform hostPlatform);
        void UpdateHostPlatform(IHostPlatform hostPlatform);
        void RemoveHostPlatform(IHostPlatform hostPlatform);
        void AddRedundancy(IUpstreamRedundancy redundancy);
        void UpdateRedundancy(IUpstreamRedundancy redundancy);
        void RemoveRedundancy(IUpstreamRedundancy redundancy);
        void AddFileRelocateTask(IFileRelocateTask fileRelocateTask);
        void UpdateFileRelocateTask(IFileRelocateTask fileRelocateTask);
        void RemoveFileRelocateTask(IFileRelocateTask fileRelocateTask);
        void AddDirectoryRelocateTask(IDirectoryRelocateTask directoryRelocateTask);
        void UpdateDirectoryRelocateTask(IDirectoryRelocateTask directoryRelocateTask);
        void RemoveDirectoryRelocateTask(IDirectoryRelocateTask directoryRelocateTask);
        void AddUserProfile(IUserProfile userProfile);
        void UpdateUserProfile(IUserProfile userProfile);
        void RemoveUserProfile(IUserProfile userProfile);
        void AddUserGroup(IUserGroup userGroup);
        void UpdateUserGroup(IUserGroup userGroup);
        void RemoveUserGroup(IUserGroup userGroup);
    }
}
