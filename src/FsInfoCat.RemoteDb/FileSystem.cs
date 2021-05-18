using FsInfoCat.Model;
using FsInfoCat.Model.Remote;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace FsInfoCat.RemoteDb
{
    public class FileSystem : IRemoteFileSystem
    {
        private string _displayName = "";
        private string _notes = "";

        public FileSystem()
        {
            Volumes = new HashSet<Volume>();
            SymbolicNames = new HashSet<SymbolicName>();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(DisplayName, new ValidationContext(this, null, null) { MemberName = nameof(DisplayName) }, results);
            Validator.TryValidateProperty(MaxNameLength, new ValidationContext(this, null, null) { MemberName = nameof(MaxNameLength) }, results);
            Validator.TryValidateProperty(DefaultSymbolicName, new ValidationContext(this, null, null) { MemberName = nameof(DefaultSymbolicName) }, results);
            if (CreatedOn.CompareTo(ModifiedOn) > 0)
                results.Add(new ValidationResult(ModelResources.ErrorMessage_ModifiedOn, new string[] { nameof(ModifiedOn) }));
            return results;
        }

        internal static void BuildEntity(EntityTypeBuilder<FileSystem> builder)
        {
            builder.HasKey(nameof(Id));
            builder.Property(nameof(DisplayName)).HasMaxLength(DBSettings.Default.DbColMaxLen_DisplayName).IsRequired();
            builder.Property(nameof(DefaultDriveType)).HasDefaultValue(DriveType.Unknown);
            builder.Property(nameof(Notes)).HasDefaultValue("");
            builder.HasOne(fs => fs.DefaultSymbolicName).WithMany(d => d.DefaultFileSystems).HasForeignKey(nameof(DefaultSymbolicNameId)).IsRequired();
            builder.HasOne(d => d.CreatedBy).WithMany(u => u.CreatedFileSystems).IsRequired();
            builder.HasOne(d => d.ModifiedBy).WithMany(u => u.ModifiedFileSystems).IsRequired();
        }

        #region Column Properties

        public Guid Id { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_DisplayName), ResourceType = typeof(ModelResources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_DisplayNameRequired), ErrorMessageResourceType = typeof(ModelResources))]
        [LengthValidationDbSettings(nameof(DBSettings.DbColMaxLen_DisplayName), ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_NameLength), ErrorMessageResourceType = typeof(ModelResources))]
        public string DisplayName { get => _displayName; set => _displayName = value ?? ""; }

        [Display(Name = nameof(ModelResources.DisplayName_CaseSensitiveSearch), ResourceType = typeof(ModelResources))]
        public bool CaseSensitiveSearch { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ReadOnly), ResourceType = typeof(ModelResources))]
        public bool ReadOnly { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_MaxNameLength), ResourceType = typeof(ModelResources))]
        [Range(0, int.MaxValue, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_MaxNameLengthNegative), ErrorMessageResourceType = typeof(ModelResources))]
        [DefaultValueDbSettings(nameof(DBSettings.DefaultValue_MaxFileSystemNameLength))]
        public long MaxNameLength { get; set; } = DBSettings.Default.DefaultValue_MaxFileSystemNameLength;

        [Display(Name = nameof(ModelResources.DisplayName_DefaultDriveType), ResourceType = typeof(ModelResources))]
        public DriveType? DefaultDriveType { get; set; }

        public string Notes { get => _notes; set => _notes = value ?? ""; }

        [Display(Name = nameof(ModelResources.DisplayName_IsInactive), ResourceType = typeof(ModelResources))]
        public bool IsInactive { get; set; }

        public Guid DefaultSymbolicNameId { get; set; }

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

        public HashSet<Volume> Volumes { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_DefaultSymbolicName), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_DefaultSymbolicNameRequired), ErrorMessageResourceType =typeof(ModelResources))]
        public SymbolicName DefaultSymbolicName { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_SymbolicName), ResourceType = typeof(ModelResources))]
        public HashSet<SymbolicName> SymbolicNames { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_CreatedBy), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_CreatedBy), ErrorMessageResourceType = typeof(ModelResources))]
        public UserProfile CreatedBy { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedBy), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_ModifiedBy), ErrorMessageResourceType = typeof(ModelResources))]
        public UserProfile ModifiedBy { get; set; }

        #endregion

        #region Explicit Members

        IReadOnlyCollection<IVolume> IFileSystem.Volumes => Volumes;

        IReadOnlyCollection<IRemoteVolume> IRemoteFileSystem.Volumes => Volumes;

        IReadOnlyCollection<IFsSymbolicName> IFileSystem.SymbolicNames => SymbolicNames;

        IReadOnlyCollection<IRemoteSymbolicName> IRemoteFileSystem.SymbolicNames => SymbolicNames;

        IFsSymbolicName IFileSystem.DefaultSymbolicName => DefaultSymbolicName;

        IRemoteSymbolicName IRemoteFileSystem.DefaultSymbolicName => DefaultSymbolicName;

        IUserProfile IRemoteTimeStampedEntity.CreatedBy => CreatedBy;

        IUserProfile IRemoteTimeStampedEntity.ModifiedBy => ModifiedBy;

        #endregion
    }
}
