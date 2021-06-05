using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
        private readonly IPropertyChangeTracker<RedundancyRemediationStatus> _remediationStatus;
        private readonly IPropertyChangeTracker<string> _reference;
        private readonly IPropertyChangeTracker<string> _notes;
        private readonly IPropertyChangeTracker<Guid> _contentInfoId;
        private readonly IPropertyChangeTracker<Guid?> _upstreamId;
        private readonly IPropertyChangeTracker<DateTime?> _lastSynchronizedOn;
        private readonly IPropertyChangeTracker<DateTime> _createdOn;
        private readonly IPropertyChangeTracker<DateTime> _modifiedOn;
        private readonly IPropertyChangeTracker<ContentInfo> _contentInfo;
        private HashSet<Redundancy> _redundancies = new();

        #endregion

        #region Properties

        [Key]
        public virtual Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        [Required]
        public virtual RedundancyRemediationStatus RemediationStatus { get => _remediationStatus.GetValue(); set => _remediationStatus.SetValue(value); }

        [Required(AllowEmptyStrings = true)]
        [StringLength(DbConstants.DbColMaxLen_ShortName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual string Reference { get => _reference.GetValue(); set => _reference.SetValue(value); }

        /// <remarks>TEXT NOT NULL DEFAULT ''</remarks>
        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes.GetValue(); set => _notes.SetValue(value); }

        /// <remarks>UNIQUEIDENTIFIER NOT NULL</remarks>
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

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_UpstreamId), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual Guid? UpstreamId { get => _upstreamId.GetValue(); set => _upstreamId.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_LastSynchronizedOn), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DateTime? LastSynchronizedOn { get => _lastSynchronizedOn.GetValue(); set => _lastSynchronizedOn.SetValue(value); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_CreatedOn), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DateTime CreatedOn { get => _createdOn.GetValue(); set => _createdOn.SetValue(value); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ModifiedOn), ResourceType = typeof(FsInfoCat.Properties.Resources))]
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
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _remediationStatus = AddChangeTracker(nameof(RemediationStatus), RedundancyRemediationStatus.Unconfirmed);
            _reference = AddChangeTracker(nameof(Reference), "", NonNullStringCoersion.Default);
            _notes = AddChangeTracker(nameof(Notes), "", NonNullStringCoersion.Default);
            _contentInfoId = AddChangeTracker(nameof(ContentInfoId), Guid.Empty);
            _upstreamId = AddChangeTracker<Guid?>(nameof(UpstreamId), null);
            _lastSynchronizedOn = AddChangeTracker<DateTime?>(nameof(LastSynchronizedOn), null);
            _modifiedOn = AddChangeTracker(nameof(ModifiedOn), (_createdOn = AddChangeTracker(nameof(CreatedOn), DateTime.Now)).GetValue());
            _contentInfo = AddChangeTracker<ContentInfo>(nameof(ContentInfo), null);
        }

        public bool IsNew() => !_id.IsSet;

        public bool IsSameDbRow(IDbEntity other) => IsNew() ? ReferenceEquals(this, other) : (other is ILocalRedundantSet entity && Id.Equals(entity.Id));

        internal static void BuildEntity(EntityTypeBuilder<RedundantSet> builder)
        {
            builder.HasOne(sn => sn.ContentInfo).WithMany(d => d.RedundantSets).HasForeignKey(nameof(ContentInfoId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) => LocalDbContext.GetBasicLocalDbEntityValidationResult(this, OnValidate);

        private void OnValidate(EntityEntry<RedundantSet> entityEntry, LocalDbContext dbContext, List<ValidationResult> validationResults)
        {
            // TODO: Implement OnValidate(EntityEntry{RedundantSet}, LocalDbContext, List{ValidationResult})
            throw new NotImplementedException();
        }
    }
}
