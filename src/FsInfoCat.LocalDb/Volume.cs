using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.LocalDb
{
    public class Volume : IVolume, IValidatableObject
    {

        public Guid Id { get; set; }

        private string _displayName = "";

        [DisplayName(Constants.DISPLAY_NAME_DISPLAY_NAME)]
        [Required(AllowEmptyStrings = false, ErrorMessage = Constants.ERROR_MESSAGE_DISPAY_NAME_REQUIRED)]
        [MaxLength(Constants.MAX_LENGTH_DISPLAY_NAME, ErrorMessage = Constants.ERROR_MESSAGE_DISPAY_NAME_LENGTH)]
        public string DisplayName { get => _displayName; set => _displayName = value ?? ""; }

        private string _volumeName = "";

        [DisplayName(Constants.DISPLAY_NAME_VOLUME_NAME)]
        [Required(AllowEmptyStrings = false, ErrorMessage = Constants.ERROR_MESSAGE_VOLUME_NAME_REQUIRED)]
        [MaxLength(Constants.MAX_LENGTH_VOLUME_NAME, ErrorMessage = Constants.ERROR_MESSAGE_VOLUME_NAME_LENGTH)]
        public string VolumeName { get => _volumeName; set => _volumeName = value ?? ""; }

        private string _identifier = "";

        [Required(AllowEmptyStrings = false, ErrorMessage = Constants.ERROR_MESSAGE_IDENTIFIER_REQUIRED)]
        [MaxLength(Constants.MAX_LENGTH_IDENTIFIER, ErrorMessage = Constants.ERROR_MESSAGE_IDENTIFIER_LENGTH)]
        public string Identifier { get => _identifier; set => _identifier = value ?? ""; }

        public Guid FileSystemId { get; set; }

        public System.IO.DriveType Type { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_CASE_SENSITIVE_SEARCH)]
        public bool? CaseSensitiveSearch { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_CASE_READ_ONLY)]
        public bool? ReadOnly { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MAX_NAME_LENGTH)]
        [Range(0, int.MaxValue, ErrorMessage = Constants.ERROR_MESSAGE_MAXNAMELENGTH)]
        [DefaultValue(Constants.DEFAULT_VALUE_MAX_NAME_LENGTH)]
        public long? MaxNameLength { get; set; } = Constants.DEFAULT_VALUE_MAX_NAME_LENGTH;

        private string _notes = "";

        public string Notes { get => _notes; set => _notes = value ?? ""; }

        [DisplayName(Constants.DISPLAY_NAME_IS_INACTIVE)]
        public bool IsInactive { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_CREATED_ON)]
        public DateTime CreatedOn { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MODIFIED_ON)]
        public DateTime ModifiedOn { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_FILE_SYSTEM)]
        [Required(ErrorMessage = Constants.ERROR_MESSAGE_FILE_SYSTEM)]
        public virtual FileSystem FileSystem { get; set; }

        IFileSystem IVolume.FileSystem => FileSystem;

        [DisplayName(Constants.DISPLAY_NAME_ROOT_DIRECTORY)]
        public virtual FsDirectory RootDirectory { get; set; }

        ISubDirectory IVolume.RootDirectory => RootDirectory;

        internal static void BuildEntity(EntityTypeBuilder<Volume> builder)
        {
            builder.HasKey(nameof(Id));
            builder.Property(nameof(DisplayName)).HasMaxLength(Constants.MAX_LENGTH_DISPLAY_NAME).IsRequired();
            builder.Property(nameof(VolumeName)).HasMaxLength(Constants.MAX_LENGTH_VOLUME_NAME).IsRequired();
            builder.Property(nameof(Identifier)).HasMaxLength(Constants.MAX_LENGTH_IDENTIFIER).IsRequired();
            builder.Property(nameof(CaseSensitiveSearch)).HasDefaultValue(false);
            builder.Property(nameof(ReadOnly)).HasDefaultValue(false);
            builder.Property(nameof(MaxNameLength)).HasDefaultValue(Constants.DEFAULT_VALUE_MAX_NAME_LENGTH);
            builder.Property(nameof(Notes)).HasDefaultValue("");
            builder.HasOne(p => p.FileSystem).WithMany(d => d.Volumes).IsRequired();
            builder.HasOne(p => p.RootDirectory).WithOne(d => d.Volume);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(DisplayName, new ValidationContext(this, null, null) { MemberName = nameof(DisplayName) }, results);
            Validator.TryValidateProperty(VolumeName, new ValidationContext(this, null, null) { MemberName = nameof(VolumeName) }, results);
            Validator.TryValidateProperty(Identifier, new ValidationContext(this, null, null) { MemberName = nameof(Identifier) }, results);
            Validator.TryValidateProperty(MaxNameLength, new ValidationContext(this, null, null) { MemberName = nameof(MaxNameLength) }, results);
            Validator.TryValidateProperty(FileSystem, new ValidationContext(this, null, null) { MemberName = nameof(FileSystem) }, results);
            if (CreatedOn.CompareTo(ModifiedOn) > 0)
                results.Add(new ValidationResult(Constants.ERROR_MESSAGE_MODIFIED_ON, new string[] { nameof(ModifiedOn) }));
            return results;
        }
    }
}
