using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.LocalDb
{
    public class FsDirectory : ISubDirectory, IValidatableObject
    {
        public FsDirectory()
        {
            SubDirectories = new HashSet<FsDirectory>();
            Files = new HashSet<FsFile>();
            FileRelocationTasks = new HashSet<FileRelocateTask>();
            TargetDirectoryRelocationTasks = new HashSet<DirectoryRelocateTask>();
            Services.ICollectionsService collectionsService = Extensions.GetCollectionsService();
        }

        public Guid Id { get; set; }

        private string _name = "";

        [Required(AllowEmptyStrings = false, ErrorMessage = Constants.ERROR_MESSAGE_NAME_REQUIRED)]
        [MaxLength(Constants.MAX_LENGTH_FS_NAME, ErrorMessage = Constants.ERROR_MESSAGE_NAME_LENGTH)]
        public string Name { get => _name; set => _name = value ?? ""; }

        public DirectoryCrawlFlags CrawlFlags { get; set; }

        public Guid? ParentId { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MODIFIED_ON)]
        public DateTime CreatedOn { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MODIFIED_ON)]
        public DateTime ModifiedOn { get; set; }

        public Guid? SourceRelocationTaskId { get; set; }

        public virtual Volume Volume { get; set; }

        IVolume ISubDirectory.Volume => Volume;

        public virtual HashSet<FsDirectory> SubDirectories { get; set; }

        IReadOnlyCollection<ISubDirectory> ISubDirectory.SubDirectories => SubDirectories;

        [DisplayName(Constants.DISPLAY_NAME_PARENT_DIRECTORY)]
        public virtual FsDirectory Parent { get; set; }

        ISubDirectory ISubDirectory.Parent => Parent;

        public virtual HashSet<FsFile> Files { get; set; }

        IReadOnlyCollection<IFile> ISubDirectory.Files => Files;

        [DisplayName(Constants.DISPLAY_NAME_FILE_SOURCE_RELOCATION_TASK)]
        public virtual DirectoryRelocateTask SourceRelocationTask { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_FILE_RELOCATION_TASKS)]
        public virtual HashSet<FileRelocateTask> FileRelocationTasks { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_TARGET_DIRECTORY_RELOCATION_TASKS)]
        public virtual HashSet<DirectoryRelocateTask> TargetDirectoryRelocationTasks { get; set; }

        internal static void BuildEntity(EntityTypeBuilder<FsDirectory> builder)
        {
            builder.HasKey(nameof(Id));
            builder.Property(nameof(Name)).HasMaxLength(Constants.MAX_LENGTH_FS_NAME).IsRequired();
            builder.HasOne(p => p.Parent).WithMany(d => d.SubDirectories).HasForeignKey(nameof(ParentId));
            builder.HasOne(p => p.SourceRelocationTask).WithMany(d => d.SourceDirectories).HasForeignKey(nameof(SourceRelocationTaskId));
        }

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
    }
}
