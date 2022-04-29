using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Threading;

namespace FsInfoCat.Local
{
    public abstract class RedundantSetRow : LocalDbEntity, ILocalRedundantSetRow, ISimpleIdentityReference<RedundantSetRow>
    {
        #region Fields

        public static readonly Regex IPV4HostRegex = new(@"^((?<!\d)(0(?=\d))*(?!25[6-9]|([3-9]\d|1\d\d)\d)\d{1,3}\.?){4}(?<!\.)$", RegexOptions.Compiled);

        public static readonly Regex IPV6HostRegex = new(@"([a-f\d]{1,4}:){7}(:|[a-f\d]{1,4})|(?=(\w*:){2,7}\w*\]?$)(([a-f\d]{1,4}:)+|:):([a-f\d]{1,4}(:[a-f\d]{1,4})*)?", RegexOptions.Compiled);

        public static readonly Regex DnsRegex = new(@"[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?", RegexOptions.Compiled);

        public static readonly Regex HOST_NAME_OR_ADDRESS_FOR_URI_REGEX = new(@"^
(
    (?<ipv4>((?<!\d)(0(?=\d))*(?!25[6-9]|([3-9]\d|1\d\d)\d)\d{1,3}\.?){4}(?<!\.))
|
    (?=(\[[a-f\d:]+\]|[a-f\d:]+)$)
    \[?(?<ipv6>([a-f\d]{1,4}:){7}(:|[a-f\d]{1,4})|(?=(\w*:){2,7}\w*\]?$)(([a-f\d]{1,4}:)+|:):([a-f\d]{1,4}(:[a-f\d]{1,4})*)?)\]?
|
    (?=[\w-.]{1,255}(?![\w-.]))
    (?<dns>[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?)
)$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        private Guid? _id;
        private string _reference = string.Empty;
        private string _notes = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the primary key value.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database.</value>
        [Key]
        [BackingField(nameof(_id))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Id), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual Guid Id
        {
            get => _id ?? Guid.Empty;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_id.HasValue)
                    {
                        if (!_id.Value.Equals(value))
                            throw new InvalidOperationException();
                    }
                    else if (value.Equals(Guid.Empty))
                        return;
                    _id = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [Required(AllowEmptyStrings = true)]
        [StringLength(DbConstants.DbColMaxLen_ShortName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        [BackingField(nameof(_reference))]
        public virtual string Reference { get => _reference; set => _reference = value.AsWsNormalizedOrEmpty(); }

        public RedundancyRemediationStatus Status { get; set; } = RedundancyRemediationStatus.Unconfirmed;

        [Required(AllowEmptyStrings = true)]
        [NotNull]
        [BackingField(nameof(_notes))]
        public virtual string Notes { get => _notes; set => _notes = value.EmptyIfNullOrWhiteSpace(); }

        public virtual Guid BinaryPropertiesId { get; set; }

        RedundantSetRow IIdentityReference<RedundantSetRow>.Entity => this;

        IDbEntity IIdentityReference.Entity => this;

        #endregion

        protected bool ArePropertiesEqual([DisallowNull] ILocalRedundantSetRow other) => ArePropertiesEqual((IRedundantSetRow)other) && EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) && LastSynchronizedOn == other.LastSynchronizedOn;

        protected bool ArePropertiesEqual([DisallowNull] IRedundantSetRow other) => CreatedOn == other.CreatedOn &&
            ModifiedOn == other.ModifiedOn &&
            Reference == other.Reference &&
            Notes == other.Notes &&
            Status == other.Status &&
            BinaryPropertiesId.Equals(other.BinaryPropertiesId);

        IEnumerable<Guid> IIdentityReference.GetIdentifiers() { yield return Id; }

        public override int GetHashCode()
        {
            Guid? id = _id;
            if (id.HasValue) return id.Value.GetHashCode();
            return HashCode.Combine(_reference, Status, _notes, BinaryPropertiesId, UpstreamId, LastSynchronizedOn, CreatedOn, ModifiedOn);
        }

        public bool TryGetId(out Guid result)
        {
            Guid? id = _id;
            if (id.HasValue)
            {
                result = id.Value;
                return true;
            }
            result = Guid.Empty;
            return false;
        }
    }
}
