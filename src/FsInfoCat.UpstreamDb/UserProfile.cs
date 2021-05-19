using FsInfoCat.Model;
using FsInfoCat.Model.Upstream;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.UpstreamDb
{
    public class UserProfile : IUserProfile
    {
        private string _displayName = "";
        private string _notes = "";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        internal static void BuildEntity(EntityTypeBuilder<UserProfile> builder)
        {
            builder.HasOne(d => d.CreatedBy).WithMany(u => u.CreatedUserProfiles).HasForeignKey(nameof(CreatedById)).IsRequired();
            builder.HasOne(d => d.ModifiedBy).WithMany(u => u.ModifiedUserProfiles).HasForeignKey(nameof(ModifiedById)).IsRequired();
            throw new NotImplementedException();
        }

        public UserProfile()
        {
            AssignmentGroups = new HashSet<UserGroupMembership>();
            DirectoryRelocationTasks = new HashSet<DirectoryRelocateTask>();
            FileRelocationTasks = new HashSet<FileRelocateTask>();
            CreatedSymbolicNames = new HashSet<SymbolicName>();
            CreatedComparisons = new HashSet<FileComparison>();
            CreatedDirectories = new HashSet<FsDirectory>();
            CreatedFiles = new HashSet<FsFile>();
            CreatedFileSystems = new HashSet<FileSystem>();
            CreatedHashCalculations = new HashSet<ContentInfo>();
            CreatedHostDevices = new HashSet<HostDevice>();
            CreatedHostPlatforms = new HashSet<HostPlatform>();
            CreatedRedundancies = new HashSet<Redundancy>();
            CreatedRedundantSets = new HashSet<RedundantSet>();
            CreatedUserProfiles = new HashSet<UserProfile>();
            CreatedVolumes = new HashSet<Volume>();
            CreatedDirectoryRelocateTasks = new HashSet<DirectoryRelocateTask>();
            CreatedFileRelocateTasks = new HashSet<FileRelocateTask>();
            CreatedUserGroups = new HashSet<UserGroup>();
            ModifiedSymbolicNames = new HashSet<SymbolicName>();
            ModifiedComparisons = new HashSet<FileComparison>();
            ModifiedDirectories = new HashSet<FsDirectory>();
            ModifiedFiles = new HashSet<FsFile>();
            ModifiedFileSystems = new HashSet<FileSystem>();
            ModifiedHashCalculations = new HashSet<ContentInfo>();
            ModifiedHostDevices = new HashSet<HostDevice>();
            ModifiedHostPlatforms = new HashSet<HostPlatform>();
            ModifiedRedundancies = new HashSet<Redundancy>();
            ModifiedRedundantSets = new HashSet<RedundantSet>();
            ModifiedUserProfiles = new HashSet<UserProfile>();
            ModifiedVolumes = new HashSet<Volume>();
            ModifiedDirectoryRelocateTasks = new HashSet<DirectoryRelocateTask>();
            ModifiedFileRelocateTasks = new HashSet<FileRelocateTask>();
            ModifiedUserGroups = new HashSet<UserGroup>();
        }

        #region Column Properties

        public Guid Id { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_DisplayName), ResourceType = typeof(ModelResources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_DisplayNameRequired), ErrorMessageResourceType = typeof(ModelResources))]
        [MaxLength(DbConstants.DbColMaxLen_DisplayName, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_NameLength), ErrorMessageResourceType = typeof(ModelResources))]
        public string DisplayName { get => _displayName; set => _displayName = value ?? ""; }

        [Display(Name = nameof(ModelResources.DisplayName_FirstName), ResourceType = typeof(ModelResources))]
        [Required(AllowEmptyStrings = true)]
        [MaxLength(DbConstants.DbColMaxLen_FirstName, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_FirstNameLength),
            ErrorMessageResourceType = typeof(ModelResources))]
        public string FirstName { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_LastName), ResourceType = typeof(ModelResources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_LastNameRequired),
            ErrorMessageResourceType = typeof(ModelResources))]
        [MaxLength(DbConstants.DbColMaxLen_LastName, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_LastNameLength),
            ErrorMessageResourceType = typeof(ModelResources))]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(DbConstants.DbColMaxLen_MI, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_MILength),
            ErrorMessageResourceType = typeof(ModelResources))]
        public string MI { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(DbConstants.DbColMaxLen_Suffix, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_SuffixLength), ErrorMessageResourceType = typeof(ModelResources))]
        public string Suffix { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_Title), ResourceType = typeof(ModelResources))]
        [Required(AllowEmptyStrings = true)]
        [MaxLength(DbConstants.DbColMaxLen_Title, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_TitleLength), ErrorMessageResourceType = typeof(ModelResources))]
        public string Title { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_DbPrincipalId), ResourceType = typeof(ModelResources))]
        public int? DbPrincipalId { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_SID), ResourceType = typeof(ModelResources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_SID),
            ErrorMessageResourceType = typeof(ModelResources))]
        [MaxLength(DbConstants.DbColMaxLen_SID, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_SIDLength),
            ErrorMessageResourceType = typeof(ModelResources))]
        public byte[] SID { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_LoginName), ResourceType = typeof(ModelResources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_LoginName),
            ErrorMessageResourceType = typeof(ModelResources))]
        [MaxLength(DbConstants.DbColMaxLen_LoginName, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_LoginNameLength),
            ErrorMessageResourceType = typeof(ModelResources))]
        public string LoginName { get; set; }

        [Required]
        public UserRole ExplicitRoles { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Notes { get => _notes; set => _notes = value ?? ""; }

        [Required]
        public bool IsInactive { get; set; }

        [Required]
        [Display(Name = nameof(ModelResources.DisplayName_CreatedOn), ResourceType = typeof(ModelResources))]
        public DateTime CreatedOn { get; set; }

        public Guid CreatedById { get; set; }

        [Required]
        [Display(Name = nameof(ModelResources.DisplayName_ModifiedOn), ResourceType = typeof(ModelResources))]
        public DateTime ModifiedOn { get; set; }

        public Guid ModifiedById { get; set; }

        #endregion

        #region Navigation Properties

        [Display(Name = nameof(ModelResources.DisplayName_AssignmentGroups), ResourceType = typeof(ModelResources))]
        public HashSet<UserGroupMembership> AssignmentGroups { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_DirectoryRelocationTasks), ResourceType = typeof(ModelResources))]
        public HashSet<DirectoryRelocateTask> DirectoryRelocationTasks { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_FileRelocationTasks), ResourceType = typeof(ModelResources))]
        public HashSet<FileRelocateTask> FileRelocationTasks { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_CreatedBy), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_CreatedBy), ErrorMessageResourceType = typeof(ModelResources))]
        public UserProfile CreatedBy { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedBy), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_ModifiedBy), ErrorMessageResourceType = typeof(ModelResources))]
        public UserProfile ModifiedBy { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_CreatedSymbolicNames), ResourceType = typeof(ModelResources))]
        public HashSet<SymbolicName> CreatedSymbolicNames { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_CreatedComparisons), ResourceType = typeof(ModelResources))]
        public HashSet<FileComparison> CreatedComparisons { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_CreatedDirectories), ResourceType = typeof(ModelResources))]
        public HashSet<FsDirectory> CreatedDirectories { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_CreatedFiles), ResourceType = typeof(ModelResources))]
        public HashSet<FsFile> CreatedFiles { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_CreatedFileSystems), ResourceType = typeof(ModelResources))]
        public HashSet<FileSystem> CreatedFileSystems { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_CreatedHashCalculations), ResourceType = typeof(ModelResources))]
        public HashSet<ContentInfo> CreatedHashCalculations { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_CreatedHostDevices), ResourceType = typeof(ModelResources))]
        public HashSet<HostDevice> CreatedHostDevices { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_CreatedHostPlatforms), ResourceType = typeof(ModelResources))]
        public HashSet<HostPlatform> CreatedHostPlatforms { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_CreatedRedundancies), ResourceType = typeof(ModelResources))]
        public HashSet<Redundancy> CreatedRedundancies { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_CreatedRedundantSets), ResourceType = typeof(ModelResources))]
        public HashSet<RedundantSet> CreatedRedundantSets { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_CreatedUserProfiles), ResourceType = typeof(ModelResources))]
        public HashSet<UserProfile> CreatedUserProfiles { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_CreatedVolumes), ResourceType = typeof(ModelResources))]
        public HashSet<Volume> CreatedVolumes { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedVolumes), ResourceType = typeof(ModelResources))]
        public HashSet<Volume> ModifiedVolumes { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedUserProfiles), ResourceType = typeof(ModelResources))]
        public HashSet<UserProfile> ModifiedUserProfiles { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedRedundancies), ResourceType = typeof(ModelResources))]
        public HashSet<Redundancy> ModifiedRedundancies { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedRedundantSets), ResourceType = typeof(ModelResources))]
        public HashSet<RedundantSet> ModifiedRedundantSets { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedHostPlatforms), ResourceType = typeof(ModelResources))]
        public HashSet<HostPlatform> ModifiedHostPlatforms { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedHostDevices), ResourceType = typeof(ModelResources))]
        public HashSet<HostDevice> ModifiedHostDevices { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedHashCalculations), ResourceType = typeof(ModelResources))]
        public HashSet<ContentInfo> ModifiedHashCalculations { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedSymbolicNames), ResourceType = typeof(ModelResources))]
        public HashSet<SymbolicName> ModifiedSymbolicNames { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedFileSystems), ResourceType = typeof(ModelResources))]
        public HashSet<FileSystem> ModifiedFileSystems { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedFiles), ResourceType = typeof(ModelResources))]
        public HashSet<FsFile> ModifiedFiles { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedDirectories), ResourceType = typeof(ModelResources))]
        public HashSet<FsDirectory> ModifiedDirectories { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedComparisons), ResourceType = typeof(ModelResources))]
        public HashSet<FileComparison> ModifiedComparisons { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_CreatedDirectoryRelocateTasks), ResourceType = typeof(ModelResources))]
        public HashSet<DirectoryRelocateTask> CreatedDirectoryRelocateTasks { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_CreatedFileRelocateTasks), ResourceType = typeof(ModelResources))]
        public HashSet<FileRelocateTask> CreatedFileRelocateTasks { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_CreatedUserGroups), ResourceType = typeof(ModelResources))]
        public HashSet<UserGroup> CreatedUserGroups { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_CreatedMemberships), ResourceType = typeof(ModelResources))]
        public HashSet<UserGroupMembership> CreatedMemberships { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedDirectoryRelocateTasks), ResourceType = typeof(ModelResources))]
        public HashSet<DirectoryRelocateTask> ModifiedDirectoryRelocateTasks { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedFileRelocateTasks), ResourceType = typeof(ModelResources))]
        public HashSet<FileRelocateTask> ModifiedFileRelocateTasks { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedUserGroups), ResourceType = typeof(ModelResources))]
        public HashSet<UserGroup> ModifiedUserGroups { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedMemberships), ResourceType = typeof(ModelResources))]
        public HashSet<UserGroupMembership> ModifiedMemberships { get; set; }

        #endregion

        #region Explicit Members

        IReadOnlyCollection<byte> IUserProfile.SID => SID;

        IReadOnlyCollection<IUpstreamSymbolicName> IUserProfile.CreatedSymbolicNames => CreatedSymbolicNames;

        IReadOnlyCollection<IUpstreamFileComparison> IUserProfile.CreatedComparisons => CreatedComparisons;

        IReadOnlyCollection<IUpstreamSubDirectory> IUserProfile.CreatedDirectories => CreatedDirectories;

        IReadOnlyCollection<IUpstreamFile> IUserProfile.CreatedFiles => CreatedFiles;

        IReadOnlyCollection<IUpstreamFileSystem> IUserProfile.CreatedFileSystems => CreatedFileSystems;

        IReadOnlyCollection<IUpstreamContentInfo> IUserProfile.CreatedHashCalculations => CreatedHashCalculations;

        IReadOnlyCollection<IHostDevice> IUserProfile.CreatedHostDevices => CreatedHostDevices;

        IReadOnlyCollection<IHostPlatform> IUserProfile.CreatedHostPlatforms => CreatedHostPlatforms;

        IReadOnlyCollection<IUpstreamRedundancy> IUserProfile.CreatedRedundancies => CreatedRedundancies;

        IReadOnlyCollection<IUserProfile> IUserProfile.CreatedUserProfiles => CreatedUserProfiles;

        IReadOnlyCollection<IUpstreamVolume> IUserProfile.CreatedVolumes => CreatedVolumes;

        IReadOnlyCollection<IUpstreamVolume> IUserProfile.ModifiedVolumes => ModifiedVolumes;

        IReadOnlyCollection<IUserProfile> IUserProfile.ModifiedUserProfiles => ModifiedUserProfiles;

        IReadOnlyCollection<IUpstreamRedundancy> IUserProfile.ModifiedRedundancies => ModifiedRedundancies;

        IReadOnlyCollection<IHostPlatform> IUserProfile.ModifiedHostPlatforms => ModifiedHostPlatforms;

        IReadOnlyCollection<IHostDevice> IUserProfile.ModifiedHostDevices => ModifiedHostDevices;

        IReadOnlyCollection<IUpstreamContentInfo> IUserProfile.ModifiedHashCalculations => ModifiedHashCalculations;

        IReadOnlyCollection<IUpstreamSymbolicName> IUserProfile.ModifiedSymbolicNames => ModifiedSymbolicNames;

        IReadOnlyCollection<IUpstreamFileSystem> IUserProfile.ModifiedFileSystems => ModifiedFileSystems;

        IReadOnlyCollection<IUpstreamFile> IUserProfile.ModifiedFiles => ModifiedFiles;

        IReadOnlyCollection<IUpstreamSubDirectory> IUserProfile.ModifiedDirectories => ModifiedDirectories;

        IReadOnlyCollection<IUpstreamFileComparison> IUserProfile.ModifiedComparisons => ModifiedComparisons;

        IReadOnlyCollection<IUserGroupMembership> IUserProfile.AssignmentGroups => AssignmentGroups;

        IReadOnlyCollection<IDirectoryRelocateTask> IUserProfile.DirectoryRelocationTasks => DirectoryRelocationTasks;

        IReadOnlyCollection<IFileRelocateTask> IUserProfile.FileRelocationTasks => FileRelocationTasks;

        IReadOnlyCollection<IDirectoryRelocateTask> IUserProfile.CreatedDirectoryRelocateTasks => CreatedDirectoryRelocateTasks;

        IReadOnlyCollection<IFileRelocateTask> IUserProfile.CreatedFileRelocateTasks => CreatedFileRelocateTasks;

        IReadOnlyCollection<IUserGroup> IUserProfile.CreatedUserGroups => CreatedUserGroups;

        IReadOnlyCollection<IDirectoryRelocateTask> IUserProfile.ModifiedDirectoryRelocateTasks => ModifiedDirectoryRelocateTasks;

        IReadOnlyCollection<IFileRelocateTask> IUserProfile.ModifiedFileRelocateTasks => ModifiedFileRelocateTasks;

        IReadOnlyCollection<IUserGroup> IUserProfile.ModifiedUserGroups => ModifiedUserGroups;

        IUserProfile IUpstreamTimeStampedEntity.CreatedBy => CreatedBy;

        IUserProfile IUpstreamTimeStampedEntity.ModifiedBy => ModifiedBy;

        IReadOnlyCollection<IUserGroupMembership> IUserProfile.CreatedMemberships => CreatedMemberships;

        IReadOnlyCollection<IUserGroupMembership> IUserProfile.ModifiedMemberships => ModifiedMemberships;

        #endregion
    }
}
