using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace FsInfoCat.Local
{
    public class Volume : NotifyPropertyChanged, ILocalVolume
    {
        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _displayName;
        private readonly IPropertyChangeTracker<string> _volumeName;
        private readonly IPropertyChangeTracker<string> _identifier;
        private readonly IPropertyChangeTracker<DriveType> _type;
        private readonly IPropertyChangeTracker<bool?> _caseSensitiveSearch;
        private readonly IPropertyChangeTracker<bool?> _readOnly;
        private readonly IPropertyChangeTracker<long?> _maxNameLength;
        private readonly IPropertyChangeTracker<string> _notes;
        private readonly IPropertyChangeTracker<bool> _isInactive;
        private readonly IPropertyChangeTracker<Guid> _fileSystemId;
        private readonly IPropertyChangeTracker<Guid?> _upstreamId;
        private readonly IPropertyChangeTracker<DateTime?> _lastSynchronizedOn;
        private readonly IPropertyChangeTracker<DateTime> _createdOn;

        internal static void BuildEntity(EntityTypeBuilder<Volume> builder)
        {
            builder.HasKey(nameof(Id));
            builder.Property(nameof(DisplayName)).HasMaxLength(DbConstants.DbColMaxLen_DisplayName).IsRequired();
            builder.Property(nameof(VolumeName)).HasMaxLength(DbConstants.DbColMaxLen_VolumeName).IsRequired();
            builder.Property(nameof(Identifier)).HasMaxLength(DbConstants.DbColMaxLen_Identifier).IsRequired();
            builder.HasOne(sn => sn.FileSystem).WithMany(d => d.Volumes).HasForeignKey(nameof(FileSystemId)).IsRequired();
        }

        private readonly IPropertyChangeTracker<DateTime> _modifiedOn;
        private readonly IPropertyChangeTracker<FileSystem> _fileSystem;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id
        {
            get => _id.GetValue();
            set => _id.SetValue(value);
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_DisplayName), ResourceType = typeof(Properties.Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameRequired),
            ErrorMessageResourceType = typeof(Properties.Resources))]
        [MaxLength(DbConstants.DbColMaxLen_DisplayName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameLength),
            ErrorMessageResourceType = typeof(Properties.Resources))]
        public string DisplayName { get => _displayName.GetValue(); set => _displayName.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_VolumeName), ResourceType = typeof(Properties.Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_VolumeNameRequired),
            ErrorMessageResourceType = typeof(Properties.Resources))]
        [MaxLength(DbConstants.DbColMaxLen_VolumeName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_VolumeNameLength),
            ErrorMessageResourceType = typeof(Properties.Resources))]
        public string VolumeName { get => _volumeName.GetValue(); set => _volumeName.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Identifier), ResourceType = typeof(Properties.Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_IdentifierRequired),
            ErrorMessageResourceType = typeof(Properties.Resources))]
        [MaxLength(DbConstants.DbColMaxLen_Identifier, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_IdentifierLength),
            ErrorMessageResourceType = typeof(Properties.Resources))]
        public string Identifier { get => _identifier.GetValue(); set => _identifier.SetValue(value); }

        [Required]
        public DriveType Type { get => _type.GetValue(); set => _type.SetValue(value); }

        public bool? CaseSensitiveSearch { get => _caseSensitiveSearch.GetValue(); set => _caseSensitiveSearch.SetValue(value); }

        public bool? ReadOnly { get => _readOnly.GetValue(); set => _readOnly.SetValue(value); }

        public long? MaxNameLength { get => _maxNameLength.GetValue(); set => _maxNameLength.SetValue(value); }

        [Required(AllowEmptyStrings = true)]
        public string Notes { get => _notes.GetValue(); set => _notes.SetValue(value); }

        [Required]
        public bool IsInactive { get => _isInactive.GetValue(); set => _isInactive.SetValue(value); }

        public Guid FileSystemId
        {
            get => _fileSystemId.GetValue();
            set
            {
                if (_fileSystemId.SetValue(value))
                {
                    FileSystem nav = _fileSystem.GetValue();
                    if (!(nav is null || nav.Id.Equals(value)))
                        _fileSystem.SetValue(null);
                }
            }
        }

        public Guid? UpstreamId { get => _upstreamId.GetValue(); set => _upstreamId.SetValue(value); }

        public DateTime? LastSynchronizedOn { get => _lastSynchronizedOn.GetValue(); set => _lastSynchronizedOn.SetValue(value); }

        [Required]
        public DateTime CreatedOn { get => _createdOn.GetValue(); set => _createdOn.SetValue(value); }

        [Required]
        public DateTime ModifiedOn { get => _modifiedOn.GetValue(); set => _modifiedOn.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_FileSystem), ResourceType = typeof(Properties.Resources))]
        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_FileSystemRequired),
            ErrorMessageResourceType = typeof(Properties.Resources))]
        public FileSystem FileSystem
        {
            get => _fileSystem.GetValue();
            set
            {
                if (_fileSystem.SetValue(value) && !(value is null || value.IsNew()))
                    _fileSystemId.SetValue(value.Id);
            }
        }

        ILocalFileSystem ILocalVolume.FileSystem => FileSystem;

        IFileSystem IVolume.FileSystem => FileSystem;

        public Volume()
        {
            _id = CreateChangeTracker(nameof(Id), Guid.Empty);
            _displayName = CreateChangeTracker(nameof(DisplayName), "", new NonNullStringCoersion());
            _volumeName = CreateChangeTracker(nameof(VolumeName), "", new NonNullStringCoersion());
            _identifier = CreateChangeTracker(nameof(Identifier), "", new NonNullStringCoersion());
            _caseSensitiveSearch = CreateChangeTracker<bool?>(nameof(CaseSensitiveSearch), null);
            _readOnly = CreateChangeTracker<bool?>(nameof(ReadOnly), null);
            _maxNameLength = CreateChangeTracker<long?>(nameof(MaxNameLength), null);
            _type = CreateChangeTracker(nameof(Type), DriveType.Unknown);
            _notes = CreateChangeTracker(nameof(Notes), "", new NonNullStringCoersion());
            _isInactive = CreateChangeTracker(nameof(IsInactive), false);
            _upstreamId = CreateChangeTracker<Guid?>(nameof(UpstreamId), null);
            _lastSynchronizedOn = CreateChangeTracker<DateTime?>(nameof(LastSynchronizedOn), null);
            _modifiedOn = CreateChangeTracker(nameof(ModifiedOn), (_createdOn = CreateChangeTracker(nameof(CreatedOn), DateTime.Now)).GetValue());
            _fileSystemId = CreateChangeTracker(nameof(FileSystemId), Guid.Empty);
            _fileSystem = CreateChangeTracker<FileSystem>(nameof(FileSystem), null);
        }

        public bool IsNew() => !_id.IsSet;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> result = new List<ValidationResult>();
            if (_createdOn.GetValue().CompareTo(_modifiedOn.GetValue()) > 0)
                result.Add(new ValidationResult($"{nameof(CreatedOn)} cannot be later than {nameof(ModifiedOn)}.", new string[] { nameof(CreatedOn) }));
            return result;
        }
    }
}
