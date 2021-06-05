using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FsInfoCat.Local
{
    public class RedundantSet : LocalDbEntity, ILocalRedundantSet
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<RedundancyRemediationStatus> _remediationStatus;
        private readonly IPropertyChangeTracker<string> _reference;
        private readonly IPropertyChangeTracker<string> _notes;
        private readonly IPropertyChangeTracker<Guid> _contentInfoId;
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
            _contentInfo = AddChangeTracker<ContentInfo>(nameof(ContentInfo), null);
        }

        internal static void BuildEntity(EntityTypeBuilder<RedundantSet> builder)
        {
            builder.HasOne(sn => sn.ContentInfo).WithMany(d => d.RedundantSets).HasForeignKey(nameof(ContentInfoId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }
    }
}
