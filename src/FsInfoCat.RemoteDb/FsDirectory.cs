using FsInfoCat.Model;
using FsInfoCat.Model.Remote;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.RemoteDb
{
    public class FsDirectory : IRemoteSubDirectory
    {
        private string _name = "";

        public FsDirectory()
        {
            Files = new HashSet<FsFile>();
            SubDirectories = new HashSet<FsDirectory>();
            FileRelocationTasks = new HashSet<FileRelocateTask>();
            TargetDirectoryRelocationTasks = new HashSet<DirectoryRelocateTask>();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(Name, new ValidationContext(this, null, null) { MemberName = nameof(Name) }, results);
            if (Volume is null && Parent is null)
                results.Add(new ValidationResult(ModelResources.ErrorMessage_NoParentOrVolume, new string[] { nameof(ModifiedOn) }));
            if (CreatedOn.CompareTo(ModifiedOn) > 0)
                results.Add(new ValidationResult(ModelResources.ErrorMessage_ModifiedOn, new string[] { nameof(ModifiedOn) }));
            return results;
        }

        internal static void BuildEntity(EntityTypeBuilder<FsDirectory> builder)
        {
            builder.HasKey(nameof(Id));
            builder.Property(nameof(Name)).HasMaxLength(DBSettings.Default.DbColMaxLen_FileSystemName).IsRequired();
            builder.HasOne(p => p.Parent).WithMany(d => d.SubDirectories).HasForeignKey(nameof(ParentId));
            builder.HasOne(p => p.SourceRelocationTask).WithMany(d => d.SourceDirectories).HasForeignKey(nameof(SourceRelocationTaskId));
            builder.HasOne(d => d.CreatedBy).WithMany(u => u.CreatedDirectories).IsRequired();
            builder.HasOne(d => d.ModifiedBy).WithMany(u => u.ModifiedDirectories).IsRequired();
        }

        #region Column Properties

        // TODO: [Id] uniqueidentifier  NOT NULL,
        public Guid Id { get; set; }

        // [Name] nvarchar(128)  NOT NULL,
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_NameRequired), ErrorMessageResourceType = typeof(ModelResources))]
        [LengthValidationDbSettings(nameof(DBSettings.DbColMaxLen_FileSystemName), ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_NameLength), ErrorMessageResourceType = typeof(ModelResources))]
        public string Name { get => _name; set => _name = value ?? ""; }

        // TODO: [CrawlFlags] tinyint  NOT NULL,
        public DirectoryCrawlFlags CrawlFlags { get; set; }

        // [ParentId] uniqueidentifier  NULL,
        public Guid? ParentId { get; set; }

        // [SourceRelocationTaskId] uniqueidentifier  NULL
        public Guid? SourceRelocationTaskId { get; set; }

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

        [Display(Name = nameof(ModelResources.DisplayName_ParentDirectory), ResourceType = typeof(ModelResources))]
        public FsDirectory Parent { get; set; }

        public Volume Volume { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_DirectoryRelocationTask), ResourceType = typeof(ModelResources))]
        public virtual DirectoryRelocateTask SourceRelocationTask { get; set; }

        public HashSet<FsFile> Files { get; set; }

        public HashSet<FsDirectory> SubDirectories { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_FileRelocationTasks), ResourceType = typeof(ModelResources))]
        public HashSet<FileRelocateTask> FileRelocationTasks { get; private set; }

        [Display(Name = nameof(ModelResources.DisplayName_TargetDirectoryRelocationTasks), ResourceType = typeof(ModelResources))]
        public HashSet<DirectoryRelocateTask> TargetDirectoryRelocationTasks { get; private set; }

        [Display(Name = nameof(ModelResources.DisplayName_CreatedBy), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_CreatedBy), ErrorMessageResourceType = typeof(ModelResources))]
        public UserProfile CreatedBy { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedBy), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_ModifiedBy), ErrorMessageResourceType = typeof(ModelResources))]
        public UserProfile ModifiedBy { get; set; }

        #endregion

        #region Explicit Members

        IReadOnlyCollection<IFile> ISubDirectory.Files => Files;

        IReadOnlyCollection<IRemoteFile> IRemoteSubDirectory.Files => Files;

        IReadOnlyCollection<ISubDirectory> ISubDirectory.SubDirectories => SubDirectories;

        IReadOnlyCollection<IRemoteSubDirectory> IRemoteSubDirectory.SubDirectories => SubDirectories;

        IRemoteSubDirectory IRemoteSubDirectory.Parent => Parent;

        ISubDirectory ISubDirectory.Parent => Parent;

        IRemoteVolume IRemoteSubDirectory.Volume => Volume;

        IVolume ISubDirectory.Volume => Volume;

        IUserProfile IRemoteTimeStampedEntity.CreatedBy => CreatedBy;

        IUserProfile IRemoteTimeStampedEntity.ModifiedBy => ModifiedBy;

        IDirectoryRelocateTask IRemoteSubDirectory.SourceRelocationTask => SourceRelocationTask;

        #endregion
    }
}
