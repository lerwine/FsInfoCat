using FsInfoCat.Model;
using FsInfoCat.Model.Remote;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.RemoteDb
{
    public class FsDirectory : IRemoteSubDirectory
    {
        public FsDirectory()
        {
            SubDirectories = new HashSet<FsDirectory>();
            Files = new HashSet<FsFile>();
            FileRelocationTasks = new HashSet<FileRelocateTask>();
            TargetDirectoryRelocationTasks = new HashSet<DirectoryRelocateTask>();
        }

        public Guid Id { get; set; }

        private string _name = "";

        [Required(AllowEmptyStrings = false, ErrorMessage = Constants.ERROR_MESSAGE_NAME_REQUIRED)]
        [MaxLength(Constants.MAX_LENGTH_FS_NAME, ErrorMessage = Constants.ERROR_MESSAGE_NAME_LENGTH)]
        public string Name { get => _name; set => _name = value ?? ""; }

        // TODO: Put this in interface
        public DirectoryCrawlFlags CrawlFlags { get; set; }

        // TODO: Add this to interface
        public Guid? ParentId { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_PARENT_DIRECTORY)]
        public FsDirectory Parent { get; set; }

        public HashSet<FsDirectory> SubDirectories { get; set; }

        public Volume Volume { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_CREATED_ON)]
        public DateTime CreatedOn { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MODIFIED_ON)]
        public DateTime ModifiedOn { get; set; }

        public HashSet<FsFile> Files { get; set; }

        // TODO: Add this to interface
        public Guid? SourceRelocationTaskId { get; set; }

        // TODO: Add this to interface
        [DisplayName(Constants.DISPLAY_NAME_FILE_SOURCE_RELOCATION_TASK)]
        public virtual DirectoryRelocateTask SourceRelocationTask { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_TARGET_DIRECTORY_RELOCATION_TASKS)]
        public HashSet<FileRelocateTask> FileRelocationTasks { get; private set; }

        [DisplayName(Constants.DISPLAY_NAME_FILE_SOURCE_RELOCATION_TASK)]
        public HashSet<DirectoryRelocateTask> TargetDirectoryRelocationTasks { get; private set; }

        public Guid CreatedById { get; set; }

        public Guid ModifiedById { get; set; }

        public UserProfile CreatedBy { get; set; }

        public UserProfile ModifiedBy { get; set; }

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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(Name, new ValidationContext(this, null, null) { MemberName = nameof(Name) }, results);
            if (Volume is null && Parent is null)
                results.Add(new ValidationResult(Constants.ERROR_MESSAGE_VOLUME_PARENT_REQUIRED, new string[] { nameof(Volume), nameof(Parent) }));
            if (CreatedOn.CompareTo(ModifiedOn) > 0)
                results.Add(new ValidationResult(Constants.ERROR_MESSAGE_MODIFIED_ON, new string[] { nameof(ModifiedOn) }));
            return results;
        }

        internal static void BuildEntity(EntityTypeBuilder<FsDirectory> builder)
        {
            builder.HasKey(nameof(Id));
            builder.Property(nameof(Name)).HasMaxLength(Constants.MAX_LENGTH_FS_NAME).IsRequired();
            builder.HasOne(p => p.Parent).WithMany(d => d.SubDirectories).HasForeignKey(nameof(ParentId));
            builder.HasOne(p => p.SourceRelocationTask).WithMany(d => d.SourceDirectories).HasForeignKey(nameof(SourceRelocationTaskId));
        }
    }
}
