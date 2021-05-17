using FsInfoCat.Model;
using FsInfoCat.Model.Remote;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.RemoteDb
{
    public class FileRelocateTask : IFileRelocateTask
    {
        public FileRelocateTask()
        {
            Files = new HashSet<FsFile>();
        }

        public Guid Id { get; set; }

        public AppTaskStatus Status { get; set; }

        public PriorityLevel Priority { get; set; }

        private string _shortDescription = "";

        [DisplayName(Constants.DISPLAY_NAME_SHORT_DESCRIPTION)]
        [Required(AllowEmptyStrings = false, ErrorMessage = Constants.ERROR_MESSAGE_SHORT_DESCRIPTION)]
        [MaxLength(Constants.MAX_LENGTH_SHORT_DESCRIPTION)]
        public string ShortDescription { get => _shortDescription; set => _shortDescription = value ?? ""; }

        public Guid TargetDirectoryId { get; set; }

        private string _notes = "";

        public string Notes { get => _notes; set => _notes = value ?? ""; }

        [DisplayName(Constants.DISPLAY_NAME_MODIFIED_ON)]
        public DateTime CreatedOn { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MODIFIED_ON)]
        public DateTime ModifiedOn { get; set; }

        public virtual HashSet<FsFile> Files { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_TARGET_DIRECTORY)]
        [Required(ErrorMessage = Constants.ERROR_MESSAGE_TARGET_DIRECTORY)]
        public virtual FsDirectory TargetDirectory { get; set; }

        public Guid? AssignmentGroupId { get; set; }

        public Guid? AssignedToId { get; set; }

        IReadOnlyCollection<IRemoteFile> IFileRelocateTask.Files => Files;

        IRemoteSubDirectory IFileRelocateTask.TargetDirectory => TargetDirectory;

        public UserGroup AssignmentGroup { get; set; }

        public UserProfile AssignedTo { get; set; }

        public Guid CreatedById { get; set; }

        public Guid ModifiedById { get; set; }

        public UserProfile CreatedBy { get; set; }

        public UserProfile ModifiedBy { get; set; }

        IUserGroup IFileRelocateTask.AssignmentGroup => AssignmentGroup;

        IUserProfile IFileRelocateTask.AssignedTo => AssignedTo;

        IUserProfile IRemoteTimeStampedEntity.CreatedBy => CreatedBy;

        IUserProfile IRemoteTimeStampedEntity.ModifiedBy => ModifiedBy;

        internal static void BuildEntity(EntityTypeBuilder<FileRelocateTask> builder)
        {
            builder.HasKey(nameof(Id));
            builder.Property(nameof(ShortDescription)).HasMaxLength(Constants.MAX_LENGTH_SHORT_DESCRIPTION).IsRequired();
            builder.Property(nameof(Notes)).HasDefaultValue("");
            builder.HasOne(p => p.TargetDirectory).WithMany(d => d.FileRelocationTasks).IsRequired();
            builder.HasOne(t => t.AssignmentGroup).WithMany(u => u.FileRelocationTasks);
            builder.HasOne(t => t.AssignedTo).WithMany(u => u.FileRelocationTasks);
            builder.HasOne(d => d.CreatedBy).WithMany(u => u.CreatedFileRelocateTasks).IsRequired();
            builder.HasOne(d => d.ModifiedBy).WithMany(u => u.ModifiedFileRelocateTasks).IsRequired();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(ShortDescription, new ValidationContext(this, null, null) { MemberName = nameof(ShortDescription) }, results);
            Validator.TryValidateProperty(TargetDirectory, new ValidationContext(this, null, null) { MemberName = nameof(TargetDirectory) }, results);
            if (CreatedOn.CompareTo(ModifiedOn) > 0)
                results.Add(new ValidationResult(Constants.ERROR_MESSAGE_MODIFIED_ON, new string[] { nameof(ModifiedOn) }));
            return results;
        }
    }
}
