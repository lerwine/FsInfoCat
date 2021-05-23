using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace FsInfoCat.Local
{
    public class FileSystem : NotifyPropertyChanged, ILocalFileSystem
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _displayName;
        private readonly IPropertyChangeTracker<bool> _caseSensitiveSearch;
        private readonly IPropertyChangeTracker<bool> _readOnly;
        private readonly IPropertyChangeTracker<long> _maxNameLength;
        private readonly IPropertyChangeTracker<DriveType?> _defaultDriveType;
        private readonly IPropertyChangeTracker<string> _notes;
        private readonly IPropertyChangeTracker<bool> _isInactive;
        private readonly IPropertyChangeTracker<Guid?> _upstreamId;
        private readonly IPropertyChangeTracker<DateTime?> _lastSynchronizedOn;
        private readonly IPropertyChangeTracker<DateTime> _createdOn;
        private readonly IPropertyChangeTracker<DateTime> _modifiedOn;
        private HashSet<Volume> _volumes = new HashSet<Volume>();
        private HashSet<SymbolicName> _symbolicNames = new HashSet<SymbolicName>();

        #endregion

        #region Properties

        [Key]
        public virtual Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_DisplayName), ResourceType = typeof(Properties.Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameRequired),
            ErrorMessageResourceType = typeof(Properties.Resources))]
        [MaxLength(DbConstants.DbColMaxLen_DisplayName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameLength),
            ErrorMessageResourceType = typeof(Properties.Resources))]
        public virtual string DisplayName { get => _displayName.GetValue(); set => _displayName.SetValue(value); }

        [Required]
        public virtual bool CaseSensitiveSearch { get => _caseSensitiveSearch.GetValue(); set => _caseSensitiveSearch.SetValue(value); }

        [Required]
        public virtual bool ReadOnly { get => _readOnly.GetValue(); set => _readOnly.SetValue(value); }

        [Required]
        public virtual long MaxNameLength { get => _maxNameLength.GetValue(); set => _maxNameLength.SetValue(value); }

        public virtual DriveType? DefaultDriveType { get => _defaultDriveType.GetValue(); set => _defaultDriveType.SetValue(value); }

        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes.GetValue(); set => _notes.SetValue(value); }

        [Required]
        public virtual bool IsInactive { get => _isInactive.GetValue(); set => _isInactive.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_UpstreamId), ResourceType = typeof(Properties.Resources))]
        public virtual Guid? UpstreamId { get => _upstreamId.GetValue(); set => _upstreamId.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_LastSynchronizedOn), ResourceType = typeof(Properties.Resources))]
        public virtual DateTime? LastSynchronizedOn { get => _lastSynchronizedOn.GetValue(); set => _lastSynchronizedOn.SetValue(value); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_CreatedOn), ResourceType = typeof(Properties.Resources))]
        public virtual DateTime CreatedOn { get => _createdOn.GetValue(); set => _createdOn.SetValue(value); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ModifiedOn), ResourceType = typeof(Properties.Resources))]
        public virtual DateTime ModifiedOn { get => _modifiedOn.GetValue(); set => _modifiedOn.SetValue(value); }

        public virtual HashSet<Volume> Volumes
        {
            get => _volumes;
            set => CheckHashSetChanged(_volumes, value, h => _volumes = h);
        }

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
            _id = CreateChangeTracker(nameof(Id), Guid.Empty);
            _displayName = CreateChangeTracker(nameof(DisplayName), "", NonNullStringCoersion.Default);
            _caseSensitiveSearch = CreateChangeTracker(nameof(CaseSensitiveSearch), false);
            _readOnly = CreateChangeTracker(nameof(ReadOnly), false);
            _maxNameLength = CreateChangeTracker(nameof(MaxNameLength), 0L);
            _defaultDriveType = CreateChangeTracker<DriveType?>(nameof(DefaultDriveType), null);
            _notes = CreateChangeTracker(nameof(Notes), "", NonNullStringCoersion.Default);
            _isInactive = CreateChangeTracker(nameof(IsInactive), false);
            _upstreamId = CreateChangeTracker<Guid?>(nameof(UpstreamId), null);
            _lastSynchronizedOn = CreateChangeTracker<DateTime?>(nameof(LastSynchronizedOn), null);
            _modifiedOn = CreateChangeTracker(nameof(ModifiedOn), (_createdOn = CreateChangeTracker(nameof(CreatedOn), DateTime.Now)).GetValue());
        }

        public bool IsNew() => !_id.IsSet;

        public bool IsSameDbRow(IDbEntity other) => IsNew() ? ReferenceEquals(this, other) : (other is ILocalSymbolicName entity && Id.Equals(entity.Id));

        //internal static void BuildEntity(EntityTypeBuilder<FileSystem> builder)
        //{
        //    builder.HasKey(nameof(Id));
        //    builder.Property(nameof(DisplayName)).HasMaxLength(DbConstants.DbColMaxLen_DisplayName).IsRequired();
        //}

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> result = new List<ValidationResult>();
            if (_createdOn.GetValue().CompareTo(_modifiedOn.GetValue()) > 0)
                result.Add(new ValidationResult($"{nameof(CreatedOn)} cannot be later than {nameof(ModifiedOn)}.", new string[] { nameof(CreatedOn) }));
            // TODO: Complete validation
            return result;
        }
    }
}
