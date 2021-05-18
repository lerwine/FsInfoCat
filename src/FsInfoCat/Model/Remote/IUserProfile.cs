using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model.Remote
{
    public interface IUserProfile : IRemoteTimeStampedEntity, IValidatableObject
    {
        Guid Id { get; }
        string DisplayName { get; }
        string FirstName { get; }
        string LastName { get; }
        string MI { get; }
        string Suffix { get; }
        string Title { get; }
        int? DbPrincipalId { get; }
        IReadOnlyCollection<byte> SID { get; }
        string LoginName { get; }
        UserRole ExplicitRoles { get; }
        string Notes { get; }
        bool IsInactive { get; }

        IReadOnlyCollection<IRemoteSymbolicName> CreatedSymbolicNames { get; }
        IReadOnlyCollection<IRemoteFileComparison> CreatedComparisons { get; }
        IReadOnlyCollection<IRemoteSubDirectory> CreatedDirectories { get; }
        IReadOnlyCollection<IRemoteFile> CreatedFiles { get; }
        IReadOnlyCollection<IRemoteFileSystem> CreatedFileSystems { get; }
        IReadOnlyCollection<IRemoteHashCalculation> CreatedHashCalculations { get; }
        IReadOnlyCollection<IHostDevice> CreatedHostDevices { get; }
        IReadOnlyCollection<IHostPlatform> CreatedHostPlatforms { get; }
        IReadOnlyCollection<IRemoteRedundancy> CreatedRedundancies { get; }
        IReadOnlyCollection<IUserProfile> CreatedUserProfiles { get; }
        IReadOnlyCollection<IRemoteVolume> CreatedVolumes { get; }
        IReadOnlyCollection<IRemoteVolume> ModifiedVolumes { get; }
        IReadOnlyCollection<IUserProfile> ModifiedUserProfiles { get; }
        IReadOnlyCollection<IRemoteRedundancy> ModifiedRedundancies { get; }
        IReadOnlyCollection<IHostPlatform> ModifiedHostPlatforms { get; }
        IReadOnlyCollection<IHostDevice> ModifiedHostDevices { get; }
        IReadOnlyCollection<IRemoteHashCalculation> ModifiedHashCalculations { get; }
        IReadOnlyCollection<IRemoteSymbolicName> ModifiedSymbolicNames { get; }
        IReadOnlyCollection<IRemoteFileSystem> ModifiedFileSystems { get; }
        IReadOnlyCollection<IRemoteFile> ModifiedFiles { get; }
        IReadOnlyCollection<IRemoteSubDirectory> ModifiedDirectories { get; }
        IReadOnlyCollection<IRemoteFileComparison> ModifiedComparisons { get; }
        IReadOnlyCollection<IUserGroupMembership> AssignmentGroups { get; }
        IReadOnlyCollection<IDirectoryRelocateTask> DirectoryRelocationTasks { get; }
        IReadOnlyCollection<IFileRelocateTask> FileRelocationTasks { get; }
        IReadOnlyCollection<IDirectoryRelocateTask> CreatedDirectoryRelocateTasks { get; }
        IReadOnlyCollection<IFileRelocateTask> CreatedFileRelocateTasks { get; }
        IReadOnlyCollection<IUserGroup> CreatedUserGroups { get; }
        IReadOnlyCollection<IUserGroupMembership> CreatedMemberships { get; }
        IReadOnlyCollection<IDirectoryRelocateTask> ModifiedDirectoryRelocateTasks { get; }
        IReadOnlyCollection<IFileRelocateTask> ModifiedFileRelocateTasks { get; }
        IReadOnlyCollection<IUserGroup> ModifiedUserGroups { get; }
        IReadOnlyCollection<IUserGroupMembership> ModifiedMemberships { get; }
    }
}
