using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace FsInfoCat.Local
{
    public class SymbolicName : NotifyPropertyChanged, ILocalSymbolicName
    {
        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _name;
        private readonly IPropertyChangeTracker<Guid> _fileSystemId;
        private readonly IPropertyChangeTracker<string> _notes;
        private readonly IPropertyChangeTracker<bool> _isInactive;
        private readonly IPropertyChangeTracker<Guid?> _upstreamId;
        private readonly IPropertyChangeTracker<DateTime?> _lastSynchronizedOn;
        private readonly IPropertyChangeTracker<DateTime> _createdOn;
        private readonly IPropertyChangeTracker<DateTime> _modifiedOn;
        private readonly IPropertyChangeTracker<FileSystem> _fileSystem;
        //private HashSet<FileSystem> _fileSystemDefaults = new HashSet<FileSystem>();

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id
        {
            get => _id.GetValue();
            set => _id.SetValue(value);
        }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired),
            ErrorMessageResourceType = typeof(Properties.Resources))]
        [MaxLength(DbConstants.DbColMaxLen_SimpleName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameLength),
            ErrorMessageResourceType = typeof(Properties.Resources))]
        public string Name { get => _name.GetValue(); set => _name.SetValue(value); }

        [Required(AllowEmptyStrings = true)]
        public string Notes { get => _notes.GetValue(); set => _notes.SetValue(value); }

        [Required]
        public bool IsInactive { get => _isInactive.GetValue(); set => _isInactive.SetValue(value); }

        public int Priority { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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

        ILocalFileSystem ILocalSymbolicName.FileSystem { get => FileSystem; set => FileSystem = (FileSystem)value; }
        IFileSystem ISymbolicName.FileSystem { get => FileSystem; set => FileSystem = (FileSystem)value; }

        //[Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_FileSystemDefaults), ResourceType = typeof(Properties.Resources))]
        //public virtual HashSet<FileSystem> FileSystemDefaults
        //{
        //    get => _fileSystemDefaults;
        //    set => CheckHashSetChanged(_fileSystemDefaults, value, h => _fileSystemDefaults = h);
        //}

        public SymbolicName()
        {
            _id = CreateChangeTracker(nameof(Id), Guid.Empty);
            _name = CreateChangeTracker(nameof(Name), "", new NonNullStringCoersion());
            _notes = CreateChangeTracker(nameof(Notes), "", new NonNullStringCoersion());
            _isInactive = CreateChangeTracker(nameof(IsInactive), false);
            _upstreamId = CreateChangeTracker<Guid?>(nameof(UpstreamId), null);
            _lastSynchronizedOn = CreateChangeTracker<DateTime?>(nameof(LastSynchronizedOn), null);
            //_fileSystemDefaults = new HashSet<FileSystem>();
            _modifiedOn = CreateChangeTracker(nameof(ModifiedOn), (_createdOn = CreateChangeTracker(nameof(CreatedOn), DateTime.Now)).GetValue());
            _fileSystemId = CreateChangeTracker(nameof(FileSystemId), Guid.Empty);
            _fileSystem = CreateChangeTracker<FileSystem>(nameof(FileSystem), null);
        }

        internal static void BuildEntity(EntityTypeBuilder<SymbolicName> builder)
        {
            builder.HasKey(nameof(Id));
            builder.Property(nameof(Name)).HasMaxLength(DbConstants.DbColMaxLen_SimpleName).IsRequired();
            builder.HasOne(sn => sn.FileSystem).WithMany(d => d.SymbolicNames).HasForeignKey(nameof(FileSystemId)).IsRequired();
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
