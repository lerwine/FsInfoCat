using FsInfoCat.Model;
using FsInfoCat.Model.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.LocalDb
{
    public class FileSystem : ILocalFileSystem
    {
        public FileSystem()
        {
            Volumes = new HashSet<Volume>();
            SymbolicNames = new HashSet<SymbolicName>();
        }

        public Guid Id { get; set; }

        private string _displayName = "";

        [Required(AllowEmptyStrings = false, ErrorMessage = Constants.ERROR_MESSAGE_DISPAY_NAME_REQUIRED)]
        [DisplayName(Constants.DISPLAY_NAME_DISPLAY_NAME)]
        [MaxLength(Constants.MAX_LENGTH_DISPLAY_NAME, ErrorMessage = Constants.ERROR_MESSAGE_DISPAY_NAME_LENGTH)]
        public string DisplayName { get => _displayName; set => _displayName = value ?? ""; }

        [DisplayName(Constants.DISPLAY_NAME_CASE_SENSITIVE_SEARCH)]
        public bool CaseSensitiveSearch { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_CASE_READ_ONLY)]
        public bool ReadOnly { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MAX_NAME_LENGTH)]
        [Range(0, int.MaxValue, ErrorMessage = Constants.ERROR_MESSAGE_MAXNAMELENGTH)]
        public long MaxNameLength { get; set; }

        public System.IO.DriveType? DefaultDriveType { get; set; }

        private string _notes = "";

        public string Notes { get => _notes; set => _notes = value ?? ""; }

        [DisplayName(Constants.DISPLAY_NAME_IS_INACTIVE)]
        public bool IsInactive { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_CREATED_ON)]
        public DateTime CreatedOn { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MODIFIED_ON)]
        public DateTime ModifiedOn { get; set; }

        public Guid DefaultSymbolicNameId { get; set; }

        public virtual HashSet<Volume> Volumes { get; set; }

        IReadOnlyCollection<IVolume> IFileSystem.Volumes => Volumes;

        [DisplayName(Constants.DISPLAY_NAME_SYMBOLIC_NAMES)]
        public virtual HashSet<SymbolicName> SymbolicNames { get; set; }

        IReadOnlyCollection<IFsSymbolicName> IFileSystem.SymbolicNames => SymbolicNames;

        [DisplayName(Constants.DISPLAY_NAME_DEFAULT_SYMBOLIC_NAME)]
        [Required(ErrorMessage = Constants.ERROR_MESSAGE_DEFAULT_SYMBOLIC_NAME)]
        public virtual SymbolicName DefaultSymbolicName { get; set; }

        IFsSymbolicName IFileSystem.DefaultSymbolicName => DefaultSymbolicName;

        IReadOnlyCollection<ILocalVolume> ILocalFileSystem.Volumes => Volumes;

        IReadOnlyCollection<ILocalSymbolicName> ILocalFileSystem.SymbolicNames => SymbolicNames;

        ILocalSymbolicName ILocalFileSystem.DefaultSymbolicName => DefaultSymbolicName;

        internal static void BuildEntity(EntityTypeBuilder<FileSystem> builder)
        {
            builder.HasKey(nameof(Id));
            builder.Property(nameof(DisplayName)).HasMaxLength(Constants.MAX_LENGTH_DISPLAY_NAME).IsRequired();
            builder.Property(nameof(DefaultDriveType)).HasDefaultValue(System.IO.DriveType.Unknown);
            builder.Property(nameof(Notes)).HasDefaultValue("");
            builder.HasOne(fs => fs.DefaultSymbolicName).WithMany(d => d.DefaultFileSystems).HasForeignKey(nameof(DefaultSymbolicNameId)).IsRequired();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(DisplayName, new ValidationContext(this, null, null) { MemberName = nameof(DisplayName) }, results);
            Validator.TryValidateProperty(MaxNameLength, new ValidationContext(this, null, null) { MemberName = nameof(MaxNameLength) }, results);
            Validator.TryValidateProperty(DefaultSymbolicName, new ValidationContext(this, null, null) { MemberName = nameof(DefaultSymbolicName) }, results);
            if (CreatedOn.CompareTo(ModifiedOn) > 0)
                results.Add(new ValidationResult(Constants.ERROR_MESSAGE_MODIFIED_ON, new string[] { nameof(ModifiedOn) }));
            return results;
        }
    }
}
