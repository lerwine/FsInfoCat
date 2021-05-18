using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model.Upstream
{
    public interface IUserProfile : IUpstreamTimeStampedEntity, IValidatableObject
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

        IReadOnlyCollection<IUpstreamSymbolicName> CreatedSymbolicNames { get; }
        IReadOnlyCollection<IUpstreamFileComparison> CreatedComparisons { get; }
        IReadOnlyCollection<IUpstreamSubDirectory> CreatedDirectories { get; }
        IReadOnlyCollection<IUpstreamFile> CreatedFiles { get; }
        IReadOnlyCollection<IUpstreamFileSystem> CreatedFileSystems { get; }
        IReadOnlyCollection<IUpstreamContentHash> CreatedHashCalculations { get; }
        IReadOnlyCollection<IHostDevice> CreatedHostDevices { get; }
        IReadOnlyCollection<IHostPlatform> CreatedHostPlatforms { get; }
        IReadOnlyCollection<IUpstreamRedundancy> CreatedRedundancies { get; }
        IReadOnlyCollection<IUserProfile> CreatedUserProfiles { get; }
        IReadOnlyCollection<IUpstreamVolume> CreatedVolumes { get; }
        IReadOnlyCollection<IUpstreamVolume> ModifiedVolumes { get; }
        IReadOnlyCollection<IUserProfile> ModifiedUserProfiles { get; }
        IReadOnlyCollection<IUpstreamRedundancy> ModifiedRedundancies { get; }
        IReadOnlyCollection<IHostPlatform> ModifiedHostPlatforms { get; }
        IReadOnlyCollection<IHostDevice> ModifiedHostDevices { get; }
        IReadOnlyCollection<IUpstreamContentHash> ModifiedHashCalculations { get; }
        IReadOnlyCollection<IUpstreamSymbolicName> ModifiedSymbolicNames { get; }
        IReadOnlyCollection<IUpstreamFileSystem> ModifiedFileSystems { get; }
        IReadOnlyCollection<IUpstreamFile> ModifiedFiles { get; }
        IReadOnlyCollection<IUpstreamSubDirectory> ModifiedDirectories { get; }
        IReadOnlyCollection<IUpstreamFileComparison> ModifiedComparisons { get; }
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
