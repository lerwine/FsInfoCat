using FsInfoCat.Model;
using FsInfoCat.Model.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace FsInfoCat.LocalDb
{
    public class Volume : ILocalVolume, IValidatableObject
    {
        private string _displayName = "";
        private string _volumeName = "";
        private string _identifier = "";
        private string _notes = "";

        internal static void BuildEntity(EntityTypeBuilder<Volume> builder)
        {
            builder.HasKey(nameof(Id));
            builder.Property(nameof(DisplayName)).HasMaxLength(DbConstants.DbColMaxLen_DisplayName).IsRequired();
            builder.Property(nameof(VolumeName)).HasMaxLength(DbConstants.DbColMaxLen_VolumeName).IsRequired();
            builder.Property(nameof(Identifier)).HasMaxLength(DbConstants.DbColMaxLen_VolumeIdentifier).IsRequired();
            builder.Property(nameof(CaseSensitiveSearch)).HasDefaultValue(false);
            builder.Property(nameof(ReadOnly)).HasDefaultValue(false);
            builder.Property(nameof(MaxNameLength)).HasDefaultValue(DbConstants.DefaultValue_MaxFileSystemNameLength);
            builder.Property(nameof(Notes)).HasDefaultValue("").HasColumnType("nvarchar(max)").IsRequired();
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
                results.Add(new ValidationResult(ModelResources.ErrorMessage_ModifiedOn, new string[] { nameof(ModifiedOn) }));
            return results;
        }

        #region Column Properties

        public Guid Id { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_DisplayName), ResourceType = typeof(ModelResources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_DisplayNameRequired), ErrorMessageResourceType = typeof(ModelResources))]
        [MaxLength(DbConstants.DbColMaxLen_DisplayName, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_NameLength), ErrorMessageResourceType = typeof(ModelResources))]
        public string DisplayName { get => _displayName; set => _displayName = value ?? ""; }

        [Display(Name = nameof(ModelResources.DisplayName_VolumeName), ResourceType = typeof(ModelResources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_VolumeNameRequired), ErrorMessageResourceType = typeof(ModelResources))]
        [MaxLength(DbConstants.DbColMaxLen_VolumeName, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_NameLength), ErrorMessageResourceType = typeof(ModelResources))]
        public string VolumeName { get => _volumeName; set => _volumeName = value ?? ""; }

        [Display(Name = nameof(ModelResources.DisplayName_VolumeIdentifier), ResourceType = typeof(ModelResources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_VolumeIdentifierRequired), ErrorMessageResourceType = typeof(ModelResources))]
        [MaxLength(DbConstants.DbColMaxLen_VolumeIdentifier, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_NameLength), ErrorMessageResourceType = typeof(ModelResources))]
        public string Identifier { get => _identifier; set => _identifier = value ?? ""; }

        public Guid FileSystemId { get; set; }

        [Required]
        [Display(Name = nameof(ModelResources.DisplayName_DriveType), ResourceType = typeof(ModelResources))]
        public DriveType Type { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_CaseSensitiveSearch), ResourceType = typeof(ModelResources))]
        public bool? CaseSensitiveSearch { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ReadOnly), ResourceType = typeof(ModelResources))]
        public bool? ReadOnly { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_MaxNameLength), ResourceType = typeof(ModelResources))]
        [Range(0, int.MaxValue, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_MaxNameLengthNegative), ErrorMessageResourceType = typeof(ModelResources))]
        public long? MaxNameLength { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Notes { get => _notes; set => _notes = value ?? ""; }

        [Required]
        [Display(Name = nameof(ModelResources.DisplayName_IsInactive), ResourceType = typeof(ModelResources))]
        public bool IsInactive { get; set; }

        public Guid? UpstreamId { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_LastSynchronized), ResourceType = typeof(ModelResources))]
        public DateTime? LastSynchronized { get; set; }

        [Required]
        [Display(Name = nameof(ModelResources.DisplayName_CreatedOn), ResourceType = typeof(ModelResources))]
        public DateTime CreatedOn { get; set; }

        [Required]
        [Display(Name = nameof(ModelResources.DisplayName_ModifiedOn), ResourceType = typeof(ModelResources))]
        public DateTime ModifiedOn { get; set; }

        #endregion

        #region Navigation Properties

        [Display(Name = nameof(ModelResources.DisplayName_FileSystem), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_FileSystemRequired), ErrorMessageResourceType = typeof(ModelResources))]
        public virtual FileSystem FileSystem { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_RootDirectory), ResourceType = typeof(ModelResources))]
        public virtual FsDirectory RootDirectory { get; set; }

        #endregion

        #region Explicit Members

        IFileSystem IVolume.FileSystem => FileSystem;

        ISubDirectory IVolume.RootDirectory => RootDirectory;

        ILocalSubDirectory ILocalVolume.RootDirectory => RootDirectory;

        ILocalFileSystem ILocalVolume.FileSystem => FileSystem;

        #endregion
    }
}
