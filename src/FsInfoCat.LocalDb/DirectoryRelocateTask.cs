using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.LocalDb
{
    [Obsolete("This should be remote DB only")]
    public class DirectoryRelocateTask : IValidatableObject
    {

        public DirectoryRelocateTask()
        {
            SourceDirectories = new HashSet<FsDirectory>();
        }

        public Guid Id { get; set; }

        public AppTaskStatus Status { get; set; }

        public PriorityLevel Priority { get; set; }

        private string _shortDescription = "";

        [DisplayName(Constants.DISPLAY_NAME_SHORT_DESCRIPTION)]
        [Required(AllowEmptyStrings = false, ErrorMessage = Constants.ERROR_MESSAGE_SHORT_DESCRIPTION)]
        [MaxLength(Constants.MAX_LENGTH_SHORT_DESCRIPTION)]
        public string ShortDescription { get => _shortDescription; set => _shortDescription = value ?? ""; }

        private string _notes = "";

        public string Notes { get => _notes; set => _notes = value ?? ""; }

        public bool IsInactive { get; set; }

        public Guid TargetDirectoryId { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MODIFIED_ON)]
        public DateTime CreatedOn { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MODIFIED_ON)]
        public DateTime ModifiedOn { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_SOURCE_DIRECTORIES)]
        public virtual HashSet<FsDirectory> SourceDirectories { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_TARGET_DIRECTORY)]
        [Required(ErrorMessage = Constants.ERROR_MESSAGE_TARGET_DIRECTORY)]
        public virtual FsDirectory TargetDirectory { get; set; }

        internal static void BuildEntity(EntityTypeBuilder<DirectoryRelocateTask> builder)
        {
            builder.HasKey(nameof(Id));
            builder.Property(nameof(ShortDescription)).HasMaxLength(Constants.MAX_LENGTH_SHORT_DESCRIPTION).IsRequired();
            builder.Property(nameof(Notes)).HasDefaultValue("");
            builder.HasOne(p => p.TargetDirectory).WithMany(d => d.TargetDirectoryRelocationTasks).IsRequired();
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
