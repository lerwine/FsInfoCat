using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace FsInfoCat.Local
{
    public class FileSystem : LocalDbEntity, ILocalFileSystem
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _displayName;
        private readonly IPropertyChangeTracker<bool> _caseSensitiveSearch;
        private readonly IPropertyChangeTracker<bool> _readOnly;
        private readonly IPropertyChangeTracker<int> _maxNameLength;
        private readonly IPropertyChangeTracker<DriveType?> _defaultDriveType;
        private readonly IPropertyChangeTracker<string> _notes;
        private readonly IPropertyChangeTracker<bool> _isInactive;
        private HashSet<Volume> _volumes = new();
        private HashSet<SymbolicName> _symbolicNames = new();

        #endregion

        #region Properties

        [Key]
        public virtual Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_DisplayName), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [StringLength(DbConstants.DbColMaxLen_LongName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual string DisplayName { get => _displayName.GetValue(); set => _displayName.SetValue(value); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_CaseSensitiveSearch), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual bool CaseSensitiveSearch { get => _caseSensitiveSearch.GetValue(); set => _caseSensitiveSearch.SetValue(value); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ReadOnly), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual bool ReadOnly { get => _readOnly.GetValue(); set => _readOnly.SetValue(value); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_MaxNameLength), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_MaxNameLengthInvalid),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual int MaxNameLength { get => _maxNameLength.GetValue(); set => _maxNameLength.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_DefaultDriveType), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DriveType? DefaultDriveType { get => _defaultDriveType.GetValue(); set => _defaultDriveType.SetValue(value); }

        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes.GetValue(); set => _notes.SetValue(value); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_IsInactive), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual bool IsInactive { get => _isInactive.GetValue(); set => _isInactive.SetValue(value); }

        public virtual HashSet<Volume> Volumes
        {
            get => _volumes;
            set => CheckHashSetChanged(_volumes, value, h => _volumes = h);
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_SymbolicNames), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual HashSet<SymbolicName> SymbolicNames
        {
            get => _symbolicNames;
            set => CheckHashSetChanged(_symbolicNames, value, h => _symbolicNames = h);
        }

        #endregion

        #region Explicit Members

        IEnumerable<ILocalVolume> ILocalFileSystem.Volumes => _volumes.Cast<ILocalVolume>();

        IEnumerable<ILocalSymbolicName> ILocalFileSystem.SymbolicNames => _volumes.Cast<ILocalSymbolicName>();

        IEnumerable<IVolume> IFileSystem.Volumes => _volumes.Cast<IVolume>();

        IEnumerable<ISymbolicName> IFileSystem.SymbolicNames => _volumes.Cast<ISymbolicName>();

        #endregion

        public FileSystem()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _displayName = AddChangeTracker(nameof(DisplayName), "", TrimmedNonNullStringCoersion.Default);
            _caseSensitiveSearch = AddChangeTracker(nameof(CaseSensitiveSearch), false);
            _readOnly = AddChangeTracker(nameof(ReadOnly), false);
            _maxNameLength = AddChangeTracker(nameof(MaxNameLength), DbConstants.DbColDefaultValue_MaxNameLength);
            _defaultDriveType = AddChangeTracker<DriveType?>(nameof(DefaultDriveType), null);
            _notes = AddChangeTracker(nameof(Notes), "", NonNullStringCoersion.Default);
            _isInactive = AddChangeTracker(nameof(IsInactive), false);
        }

        protected override void OnValidate(ValidationContext validationContext, List<ValidationResult> results)
        {
            base.OnValidate(validationContext, results);
            var driveType = DefaultDriveType;
            if (driveType.HasValue && !Enum.IsDefined(driveType.Value))
                results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_DriveTypeInvalid, new string[] { nameof(DefaultDriveType) }));
        }

        private void OnValidate(EntityEntry<FileSystem> entityEntry, LocalDbContext dbContext, List<ValidationResult> validationResults)
        {
            var driveType = DefaultDriveType;
            if (driveType.HasValue && !Enum.IsDefined(driveType.Value))
                validationResults.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_DriveTypeInvalid, new string[] { nameof(DefaultDriveType) }));
            string displayName = DisplayName;
            if (string.IsNullOrEmpty(displayName))
                return;
            Guid id = Id;
            var entities = from fs in dbContext.FileSystems where id != fs.Id && fs.DisplayName == displayName select fs;
            if (entities.Any())
                validationResults.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_DuplicateDisplayName, new string[] { nameof(DisplayName) }));
        }
    }
}
