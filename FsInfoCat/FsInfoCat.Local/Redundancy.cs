using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{
    public class Redundancy : NotifyPropertyChanged, ILocalRedundancy
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _fileId;
        private readonly IPropertyChangeTracker<Guid> _redundantSetId;
        private readonly IPropertyChangeTracker<string> _reference;
        private readonly IPropertyChangeTracker<FileRedundancyStatus> _status;
        private readonly IPropertyChangeTracker<string> _notes;
        private readonly IPropertyChangeTracker<Guid?> _upstreamId;
        private readonly IPropertyChangeTracker<DateTime?> _lastSynchronizedOn;
        private readonly IPropertyChangeTracker<DateTime> _createdOn;
        private readonly IPropertyChangeTracker<DateTime> _modifiedOn;
        private readonly IPropertyChangeTracker<DbFile> _file;
        private readonly IPropertyChangeTracker<RedundantSet> _redundantSet;

        #endregion

        #region Properties

        public virtual Guid FileId
        {
            get => _fileId.GetValue();
            set
            {
                if (_fileId.SetValue(value))
                {
                    DbFile nav = _file.GetValue();
                    if (!(nav is null || nav.Id.Equals(value)))
                        _file.SetValue(null);
                }
            }
        }

        public virtual Guid RedundantSetId
        {
            get => _redundantSetId.GetValue();
            set
            {
                if (_redundantSetId.SetValue(value))
                {
                    RedundantSet nav = _redundantSet.GetValue();
                    if (!(nav is null || nav.Id.Equals(value)))
                        _redundantSet.SetValue(null);
                }
            }
        }

        public virtual string Reference { get => _reference.GetValue(); set => _reference.SetValue(value); }

        [Required]
        public virtual FileRedundancyStatus Status { get => _status.GetValue(); set => _status.SetValue(value); }

        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes.GetValue(); set => _notes.SetValue(value); }

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

        public virtual DbFile File
        {
            get => _file.GetValue();
            set
            {
                if (_file.SetValue(value))
                {
                    if (value is null)
                        _fileId.SetValue(Guid.Empty);
                    else
                        _fileId.SetValue(value.Id);
                }
            }
        }

        public virtual RedundantSet RedundantSet
        {
            get => _redundantSet.GetValue();
            set
            {
                if (_redundantSet.SetValue(value))
                {
                    if (value is null)
                        _redundantSetId.SetValue(Guid.Empty);
                    else
                        _redundantSetId.SetValue(value.Id);
                }
            }
        }

        #endregion

        #region Explicit Members

        ILocalFile ILocalRedundancy.File { get => File; set => File = (DbFile)value; }

        IFile IRedundancy.File { get => File; set => File = (DbFile)value; }

        ILocalRedundantSet ILocalRedundancy.RedundantSet { get => RedundantSet; set => RedundantSet = (RedundantSet)value; }

        IRedundantSet IRedundancy.RedundantSet { get => RedundantSet; set => RedundantSet = (RedundantSet)value; }

        #endregion

        public Redundancy()
        {
            _fileId = CreateChangeTracker(nameof(FileId), Guid.Empty);
            _redundantSetId = CreateChangeTracker(nameof(RedundantSetId), Guid.Empty);
            _reference = CreateChangeTracker(nameof(Reference), "", NonNullStringCoersion.Default);
            _status = CreateChangeTracker(nameof(Status), FileRedundancyStatus.Unconfirmed);
            _notes = CreateChangeTracker(nameof(Notes), "", NonNullStringCoersion.Default);
            _upstreamId = CreateChangeTracker<Guid?>(nameof(UpstreamId), null);
            _lastSynchronizedOn = CreateChangeTracker<DateTime?>(nameof(LastSynchronizedOn), null);
            _modifiedOn = CreateChangeTracker(nameof(ModifiedOn), (_createdOn = CreateChangeTracker(nameof(CreatedOn), DateTime.Now)).GetValue());
            _file = CreateChangeTracker<DbFile>(nameof(File), null);
            _redundantSet = CreateChangeTracker<RedundantSet>(nameof(RedundantSet), null);
        }

        public bool IsNew() => !(_fileId.IsSet & _redundantSetId.IsSet);


        public bool IsSameDbRow(IDbEntity other) => IsNew() ? ReferenceEquals(this, other) : (other is ILocalRedundancy entity && FileId.Equals(entity.FileId) &&
            RedundantSetId.Equals(entity.RedundantSetId));

        internal static void BuildEntity(EntityTypeBuilder<Redundancy> builder)
        {
            builder.HasKey(nameof(FileId), nameof(RedundantSetId));
            builder.HasOne(sn => sn.File).WithOne(d => d.Redundancy).HasForeignKey(nameof(FileId)).IsRequired();
            builder.HasOne(sn => sn.RedundantSet).WithMany(d => d.Redundancies).HasForeignKey(nameof(RedundantSetId)).IsRequired();
        }

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
