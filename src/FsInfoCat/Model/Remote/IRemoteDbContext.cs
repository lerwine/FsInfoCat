using System.Linq;

namespace FsInfoCat.Model.Remote
{
    public interface IRemoteDbContext : IDbContext
    {
        new IQueryable<IRemoteHashCalculation> HashCalculations { get; }
        new IQueryable<IRemoteFileComparison> Comparisons { get; }
        new IQueryable<IRemoteFile> Files { get; }
        new IQueryable<IRemoteSubDirectory> Subdirectories { get; }
        new IQueryable<IRemoteVolume> Volumes { get; }
        new IQueryable<IRemoteSymbolicName> SymbolicNames { get; }
        new IQueryable<IRemoteFileSystem> FileSystems { get; }
        IQueryable<IHostDevice> HostDevices { get; }
        IQueryable<IHostPlatform> HostPlatforms { get; }
        new IQueryable<IRemoteRedundancy> Redundancies { get; }
        IQueryable<IFileRelocateTask> FileRelocateTasks { get; }
        IQueryable<IDirectoryRelocateTask> DirectoryRelocateTasks { get; }
        IQueryable<IUserProfile> UserProfiles { get; }
        IQueryable<IUserGroup> UserGroups { get; }

        void AddHashCalculation(IRemoteHashCalculation hashCalculation);
        void UpdateHashCalculation(IRemoteHashCalculation hashCalculation);
        void RemoveHashCalculation(IRemoteHashCalculation hashCalculation);
        void AddComparison(IRemoteFileComparison fileComparison);
        void UpdateComparison(IRemoteFileComparison fileComparison);
        void RemoveComparison(IRemoteFileComparison fileComparison);
        void AddFile(IRemoteFile file);
        void UpdateFile(IRemoteFile file);
        void RemoveFile(IRemoteFile file);
        void AddSubDirectory(IRemoteSubDirectory subDirectory);
        void UpdateSubDirectory(IRemoteSubDirectory subDirectory);
        void RemoveSubDirectory(IRemoteSubDirectory subDirectory);
        void AddVolume(IRemoteVolume volume);
        void UpdateVolume(IRemoteVolume volume);
        void RemoveVolume(IRemoteVolume volume);
        void AddSymbolicName(IRemoteSymbolicName symbolicName);
        void UpdateSymbolicName(IRemoteSymbolicName symbolicName);
        void RemoveSymbolicName(IRemoteSymbolicName symbolicName);
        void AddFileSystem(IRemoteFileSystem fileSystem);
        void UpdateFileSystem(IRemoteFileSystem fileSystem);
        void RemoveFileSystem(IRemoteFileSystem fileSystem);
        void AddHostDevice(IHostDevice hostDevice);
        void UpdateHostDevice(IHostDevice hostDevice);
        void RemoveHostDevice(IHostDevice hostDevice);
        void AddHostPlatform(IHostPlatform hostPlatform);
        void UpdateHostPlatform(IHostPlatform hostPlatform);
        void RemoveHostPlatform(IHostPlatform hostPlatform);
        void AddRedundancy(IRemoteRedundancy redundancy);
        void UpdateRedundancy(IRemoteRedundancy redundancy);
        void RemoveRedundancy(IRemoteRedundancy redundancy);
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
