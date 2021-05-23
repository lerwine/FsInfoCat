using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FsInfoCat.Local
{
    public class RedundantSet : NotifyPropertyChanged, ILocalRedundantSet
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _reference;
        private readonly IPropertyChangeTracker<string> _notes;
        private readonly IPropertyChangeTracker<Guid> _contentInfoId;
        private readonly IPropertyChangeTracker<Guid?> _upstreamId;
        private readonly IPropertyChangeTracker<DateTime?> _lastSynchronizedOn;
        private readonly IPropertyChangeTracker<DateTime> _createdOn;
        private readonly IPropertyChangeTracker<DateTime> _modifiedOn;
        private readonly IPropertyChangeTracker<ContentInfo> _contentInfo;
        private HashSet<Redundancy> _redundancies = new HashSet<Redundancy>();

        #endregion

        #region Properties

        [Key]
        public virtual Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        public virtual string Reference { get => _reference.GetValue(); set => _reference.SetValue(value); }

        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes.GetValue(); set => _notes.SetValue(value); }

        public virtual Guid ContentInfoId
        {
            get => _contentInfoId.GetValue();
            set
            {
                if (_contentInfoId.SetValue(value))
                {
                    ContentInfo nav = _contentInfo.GetValue();
                    if (!(nav is null || nav.Id.Equals(value)))
                        _contentInfo.SetValue(null);
                }
            }
        }

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

        public virtual ContentInfo ContentInfo
        {
            get => _contentInfo.GetValue();
            set
            {
                if (_contentInfo.SetValue(value))
                {
                    if (value is null)
                        _contentInfoId.SetValue(Guid.Empty);
                    else
                        _contentInfoId.SetValue(value.Id);
                }
            }
        }

        public virtual HashSet<Redundancy> Redundancies
        {
            get => _redundancies;
            set => CheckHashSetChanged(_redundancies, value, h => _redundancies = h);
        }

        #endregion

        #region Explicit Members

        ILocalContentInfo ILocalRedundantSet.ContentInfo { get => ContentInfo; set => ContentInfo = (ContentInfo)value; }

        IContentInfo IRedundantSet.ContentInfo { get => ContentInfo; set => ContentInfo = (ContentInfo)value; }

        IEnumerable<ILocalRedundancy> ILocalRedundantSet.Redundancies => Redundancies.Cast<ILocalRedundancy>();

        IEnumerable<IRedundancy> IRedundantSet.Redundancies => Redundancies.Cast<IRedundancy>();

        #endregion

        public RedundantSet()
        {
            _id = CreateChangeTracker(nameof(Id), Guid.Empty);
            _reference = CreateChangeTracker(nameof(Reference), "", NonNullStringCoersion.Default);
            _notes = CreateChangeTracker(nameof(Notes), "", NonNullStringCoersion.Default);
            _contentInfoId = CreateChangeTracker(nameof(ContentInfoId), Guid.Empty);
            _upstreamId = CreateChangeTracker<Guid?>(nameof(UpstreamId), null);
            _lastSynchronizedOn = CreateChangeTracker<DateTime?>(nameof(LastSynchronizedOn), null);
            _modifiedOn = CreateChangeTracker(nameof(ModifiedOn), (_createdOn = CreateChangeTracker(nameof(CreatedOn), DateTime.Now)).GetValue());
            _contentInfo = CreateChangeTracker<ContentInfo>(nameof(ContentInfo), null);
        }

        public bool IsNew() => !_id.IsSet;

        public bool IsSameDbRow(IDbEntity other) => IsNew() ? ReferenceEquals(this, other) : (other is ILocalRedundantSet entity && Id.Equals(entity.Id));

        internal static void BuildEntity(EntityTypeBuilder<RedundantSet> builder)
        {
            builder.HasOne(sn => sn.ContentInfo).WithMany(d => d.RedundantSets).HasForeignKey(nameof(ContentInfoId)).IsRequired();
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
