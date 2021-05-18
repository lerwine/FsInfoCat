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
            CreatedHashCalculations = new HashSet<HashCalculation>();
            CreatedHostDevices = new HashSet<HostDevice>();
            CreatedHostPlatforms = new HashSet<HostPlatform>();
            CreatedRedundancies = new HashSet<Redundancy>();
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
            ModifiedHashCalculations = new HashSet<HashCalculation>();
            ModifiedHostDevices = new HashSet<HostDevice>();
            ModifiedHostPlatforms = new HashSet<HostPlatform>();
            ModifiedRedundancies = new HashSet<Redundancy>();
            ModifiedUserProfiles = new HashSet<UserProfile>();
            ModifiedVolumes = new HashSet<Volume>();
            ModifiedDirectoryRelocateTasks = new HashSet<DirectoryRelocateTask>();
            ModifiedFileRelocateTasks = new HashSet<FileRelocateTask>();
            ModifiedUserGroups = new HashSet<UserGroup>();
        }

        #region Column Properties

        // TODO: [Id] uniqueidentifier  NOT NULL,
        public Guid Id { get; set; }

        // [DisplayName] nvarchar(128)  NOT NULL,
        [Display(Name = nameof(ModelResources.DisplayName_DisplayName), ResourceType = typeof(ModelResources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_DisplayNameRequired), ErrorMessageResourceType = typeof(ModelResources))]
        [LengthValidationDbSettings(nameof(DBSettings.DbColMaxLen_DisplayName), ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_NameLength), ErrorMessageResourceType = typeof(ModelResources))]
        public string DisplayName { get => _displayName; set => _displayName = value ?? ""; }

        // TODO: [FirstName] nvarchar(32)  NULL,
        public string FirstName { get; set; }

        // TODO: [LastName] nvarchar(64)  NOT NULL,
        public string LastName { get; set; }

        // TODO: [MI] nchar(1)  NULL,
        public string MI { get; set; }

        // TODO: [Suffix] nvarchar(32)  NULL,
        public string Suffix { get; set; }

        // TODO: [Title] nvarchar(32)  NULL,
        public string Title { get; set; }

        // [DbPrincipalId] int  NULL,
        public int? DbPrincipalId { get; set; }

        // TODO: [SID] varbinary(85)  NOT NULL,
        public byte[] SID { get; set; }

        // TODO: [LoginName] nvarchar(128)  NOT NULL,
        public string LoginName { get; set; }

        // TODO: [ExplicitRoles] tinyint  NOT NULL,
        public UserRole ExplicitRoles { get; set; }

        // TODO: [Notes] nvarchar(max)  NOT NULL,
        public string Notes { get => _notes; set => _notes = value ?? ""; }

        // TODO: [IsInactive] bit  NOT NULL,
        public bool IsInactive { get; set; }

        // TODO: [CreatedOn] datetime  NOT NULL,
        [Required]
        [Display(Name = nameof(ModelResources.DisplayName_CreatedOn), ResourceType = typeof(ModelResources))]
        public DateTime CreatedOn { get; set; }

        // [CreatedById] uniqueidentifier  NOT NULL,
        public Guid CreatedById { get; set; }

        // TODO: [ModifiedOn] datetime  NOT NULL
        [Required]
        [Display(Name = nameof(ModelResources.DisplayName_ModifiedOn), ResourceType = typeof(ModelResources))]
        public DateTime ModifiedOn { get; set; }

        // [ModifiedById] uniqueidentifier  NOT NULL,
        public Guid ModifiedById { get; set; }

        #endregion

        #region Navigation Properties

        public HashSet<UserGroupMembership> AssignmentGroups { get; set; }

        public HashSet<DirectoryRelocateTask> DirectoryRelocationTasks { get; set; }

        public HashSet<FileRelocateTask> FileRelocationTasks { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_CreatedBy), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_CreatedBy), ErrorMessageResourceType = typeof(ModelResources))]
        public UserProfile CreatedBy { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedBy), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_ModifiedBy), ErrorMessageResourceType = typeof(ModelResources))]
        public UserProfile ModifiedBy { get; set; }

        public HashSet<SymbolicName> CreatedSymbolicNames { get; set; }

        public HashSet<FileComparison> CreatedComparisons { get; set; }

        public HashSet<FsDirectory> CreatedDirectories { get; set; }

        public HashSet<FsFile> CreatedFiles { get; set; }

        public HashSet<FileSystem> CreatedFileSystems { get; set; }

        public HashSet<HashCalculation> CreatedHashCalculations { get; set; }

        public HashSet<HostDevice> CreatedHostDevices { get; set; }

        public HashSet<HostPlatform> CreatedHostPlatforms { get; set; }

        public HashSet<Redundancy> CreatedRedundancies { get; set; }

        public HashSet<UserProfile> CreatedUserProfiles { get; set; }

        public HashSet<Volume> CreatedVolumes { get; set; }

        public HashSet<Volume> ModifiedVolumes { get; set; }

        public HashSet<UserProfile> ModifiedUserProfiles { get; set; }

        public HashSet<Redundancy> ModifiedRedundancies { get; set; }

        public HashSet<HostPlatform> ModifiedHostPlatforms { get; set; }

        public HashSet<HostDevice> ModifiedHostDevices { get; set; }

        public HashSet<HashCalculation> ModifiedHashCalculations { get; set; }

        public HashSet<SymbolicName> ModifiedSymbolicNames { get; set; }

        public HashSet<FileSystem> ModifiedFileSystems { get; set; }

        public HashSet<FsFile> ModifiedFiles { get; set; }

        public HashSet<FsDirectory> ModifiedDirectories { get; set; }

        public HashSet<FileComparison> ModifiedComparisons { get; set; }

        public HashSet<DirectoryRelocateTask> CreatedDirectoryRelocateTasks { get; set; }

        public HashSet<FileRelocateTask> CreatedFileRelocateTasks { get; set; }

        public HashSet<UserGroup> CreatedUserGroups { get; set; }

        public HashSet<UserGroupMembership> CreatedMemberships { get; set; }

        public HashSet<DirectoryRelocateTask> ModifiedDirectoryRelocateTasks { get; set; }

        public HashSet<FileRelocateTask> ModifiedFileRelocateTasks { get; set; }

        public HashSet<UserGroup> ModifiedUserGroups { get; set; }

        public HashSet<UserGroupMembership> ModifiedMemberships { get; set; }

        #endregion

        #region Explicit Members

        IReadOnlyCollection<byte> IUserProfile.SID => SID;

        IReadOnlyCollection<IUpstreamSymbolicName> IUserProfile.CreatedSymbolicNames => CreatedSymbolicNames;

        IReadOnlyCollection<IUpstreamFileComparison> IUserProfile.CreatedComparisons => CreatedComparisons;

        IReadOnlyCollection<IUpstreamSubDirectory> IUserProfile.CreatedDirectories => CreatedDirectories;

        IReadOnlyCollection<IUpstreamFile> IUserProfile.CreatedFiles => CreatedFiles;

        IReadOnlyCollection<IUpstreamFileSystem> IUserProfile.CreatedFileSystems => CreatedFileSystems;

        IReadOnlyCollection<IUpstreamHashCalculation> IUserProfile.CreatedHashCalculations => CreatedHashCalculations;

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

        IReadOnlyCollection<IUpstreamHashCalculation> IUserProfile.ModifiedHashCalculations => ModifiedHashCalculations;

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
