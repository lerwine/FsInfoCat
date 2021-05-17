using FsInfoCat.Model.Remote;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.RemoteDb
{
    public class UserProfile : IUserProfile
    {
        public Guid Id => throw new NotImplementedException();

        public string DisplayName => throw new NotImplementedException();

        public string FirstName => throw new NotImplementedException();

        public string LastName => throw new NotImplementedException();

        public string MI => throw new NotImplementedException();

        public string Suffix => throw new NotImplementedException();

        public string Title => throw new NotImplementedException();

        public int? DbPrincipalId => throw new NotImplementedException();

        public byte[] SID => throw new NotImplementedException();

        public string LoginName => throw new NotImplementedException();

        public UserRole ExplicitRoles => throw new NotImplementedException();

        public string Notes => throw new NotImplementedException();

        public bool IsInactive => throw new NotImplementedException();

        public HashSet<SymbolicName> CreatedSymbolicNames => throw new NotImplementedException();

        public HashSet<FileComparison> CreatedComparisons => throw new NotImplementedException();

        public HashSet<FsDirectory> CreatedDirectories => throw new NotImplementedException();

        public HashSet<FsFile> CreatedFiles => throw new NotImplementedException();

        public HashSet<FileSystem> CreatedFileSystems => throw new NotImplementedException();

        public HashSet<HashCalculation> CreatedHashCalculations => throw new NotImplementedException();

        public HashSet<HostDevice> CreatedHostDevices => throw new NotImplementedException();

        public HashSet<HostPlatform> CreatedHostPlatforms => throw new NotImplementedException();

        public HashSet<Redundancy> CreatedRedundancies => throw new NotImplementedException();

        public HashSet<UserProfile> CreatedUserProfiles => throw new NotImplementedException();

        public HashSet<Volume> CreatedVolumes => throw new NotImplementedException();

        public HashSet<Volume> ModifiedVolumes => throw new NotImplementedException();

        public HashSet<UserProfile> ModifiedUserProfiles => throw new NotImplementedException();

        public HashSet<Redundancy> ModifiedRedundancies => throw new NotImplementedException();

        public HashSet<HostPlatform> ModifiedHostPlatforms => throw new NotImplementedException();

        public HashSet<HostDevice> ModifiedHostDevices => throw new NotImplementedException();

        public HashSet<HashCalculation> ModifiedHashCalculations => throw new NotImplementedException();

        public HashSet<SymbolicName> ModifiedSymbolicNames => throw new NotImplementedException();

        public HashSet<FileSystem> ModifiedFileSystems => throw new NotImplementedException();

        public HashSet<FsFile> ModifiedFiles => throw new NotImplementedException();

        public HashSet<FsDirectory> ModifiedDirectories => throw new NotImplementedException();

        public HashSet<FileComparison> ModifiedComparisons => throw new NotImplementedException();

        public HashSet<UserGroup> AssignmentGroups => throw new NotImplementedException();

        public HashSet<DirectoryRelocateTask> DirectoryRelocationTasks => throw new NotImplementedException();

        public HashSet<FileRelocateTask> FileRelocationTasks => throw new NotImplementedException();

        public HashSet<DirectoryRelocateTask> CreatedDirectoryRelocateTasks => throw new NotImplementedException();

        public HashSet<FileRelocateTask> CreatedFileRelocateTasks => throw new NotImplementedException();

        public HashSet<UserGroup> CreatedUserGroups => throw new NotImplementedException();

        public HashSet<DirectoryRelocateTask> ModifiedDirectoryRelocateTasks => throw new NotImplementedException();

        public HashSet<FileRelocateTask> ModifiedFileRelocateTasks => throw new NotImplementedException();

        public HashSet<UserGroup> ModifiedUserGroups => throw new NotImplementedException();

        public Guid CreatedById => throw new NotImplementedException();

        public Guid ModifiedById => throw new NotImplementedException();

        public UserProfile CreatedBy => throw new NotImplementedException();

        public UserProfile ModifiedBy => throw new NotImplementedException();

        public DateTime CreatedOn => throw new NotImplementedException();

        public DateTime ModifiedOn => throw new NotImplementedException();

        IReadOnlyCollection<byte> IUserProfile.SID => throw new NotImplementedException();

        IReadOnlyCollection<IRemoteSymbolicName> IUserProfile.CreatedSymbolicNames => throw new NotImplementedException();

        IReadOnlyCollection<IRemoteFileComparison> IUserProfile.CreatedComparisons => throw new NotImplementedException();

        IReadOnlyCollection<IRemoteSubDirectory> IUserProfile.CreatedDirectories => throw new NotImplementedException();

        IReadOnlyCollection<IRemoteFile> IUserProfile.CreatedFiles => throw new NotImplementedException();

        IReadOnlyCollection<IRemoteFileSystem> IUserProfile.CreatedFileSystems => throw new NotImplementedException();

        IReadOnlyCollection<IRemoteHashCalculation> IUserProfile.CreatedHashCalculations => throw new NotImplementedException();

        IReadOnlyCollection<IHostDevice> IUserProfile.CreatedHostDevices => throw new NotImplementedException();

        IReadOnlyCollection<IHostPlatform> IUserProfile.CreatedHostPlatforms => throw new NotImplementedException();

        IReadOnlyCollection<IRemoteRedundancy> IUserProfile.CreatedRedundancies => throw new NotImplementedException();

        IReadOnlyCollection<IUserProfile> IUserProfile.CreatedUserProfiles => throw new NotImplementedException();

        IReadOnlyCollection<IRemoteVolume> IUserProfile.CreatedVolumes => throw new NotImplementedException();

        IReadOnlyCollection<IRemoteVolume> IUserProfile.ModifiedVolumes => throw new NotImplementedException();

        IReadOnlyCollection<IUserProfile> IUserProfile.ModifiedUserProfiles => throw new NotImplementedException();

        IReadOnlyCollection<IRemoteRedundancy> IUserProfile.ModifiedRedundancies => throw new NotImplementedException();

        IReadOnlyCollection<IHostPlatform> IUserProfile.ModifiedHostPlatforms => throw new NotImplementedException();

        IReadOnlyCollection<IHostDevice> IUserProfile.ModifiedHostDevices => throw new NotImplementedException();

        IReadOnlyCollection<IRemoteHashCalculation> IUserProfile.ModifiedHashCalculations => throw new NotImplementedException();

        IReadOnlyCollection<IRemoteSymbolicName> IUserProfile.ModifiedSymbolicNames => throw new NotImplementedException();

        IReadOnlyCollection<IRemoteFileSystem> IUserProfile.ModifiedFileSystems => throw new NotImplementedException();

        IReadOnlyCollection<IRemoteFile> IUserProfile.ModifiedFiles => throw new NotImplementedException();

        IReadOnlyCollection<IRemoteSubDirectory> IUserProfile.ModifiedDirectories => throw new NotImplementedException();

        IReadOnlyCollection<IRemoteFileComparison> IUserProfile.ModifiedComparisons => throw new NotImplementedException();

        IReadOnlyCollection<IUserGroup> IUserProfile.AssignmentGroups => throw new NotImplementedException();

        IReadOnlyCollection<IDirectoryRelocateTask> IUserProfile.DirectoryRelocationTasks => throw new NotImplementedException();

        IReadOnlyCollection<IFileRelocateTask> IUserProfile.FileRelocationTasks => throw new NotImplementedException();

        IReadOnlyCollection<IDirectoryRelocateTask> IUserProfile.CreatedDirectoryRelocateTasks => throw new NotImplementedException();

        IReadOnlyCollection<IFileRelocateTask> IUserProfile.CreatedFileRelocateTasks => throw new NotImplementedException();

        IReadOnlyCollection<IUserGroup> IUserProfile.CreatedUserGroups => throw new NotImplementedException();

        IReadOnlyCollection<IDirectoryRelocateTask> IUserProfile.ModifiedDirectoryRelocateTasks => throw new NotImplementedException();

        IReadOnlyCollection<IFileRelocateTask> IUserProfile.ModifiedFileRelocateTasks => throw new NotImplementedException();

        IReadOnlyCollection<IUserGroup> IUserProfile.ModifiedUserGroups => throw new NotImplementedException();

        IUserProfile IRemoteTimeStampedEntity.CreatedBy => throw new NotImplementedException();

        IUserProfile IRemoteTimeStampedEntity.ModifiedBy => throw new NotImplementedException();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
