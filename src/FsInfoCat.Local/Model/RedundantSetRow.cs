using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Threading;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for an entity representing set of files that have the same size, Hash and remediation status.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IRedundantSetRow" />
    public abstract partial class RedundantSetRow : LocalDbEntity, ILocalRedundantSetRow
    {
        #region Fields

        /// <summary>
        /// Regular expression that matches an IPV4 string.
        /// </summary>
        public static readonly Regex IPV4HostRegex = GetIPV4HostRegex();

        /// <summary>
        /// Regular expression that matches an IPV6 string.
        /// </summary>
        public static readonly Regex IPV6HostRegex = GetIPV6HostRegex();

        /// <summary>
        /// Regular expression that matches a DNS host name.
        /// </summary>
        public static readonly Regex DnsRegex = GetDnsRegex();

        /// <summary>
        /// Regular expression that matches the host name or IP address for a URI.
        /// </summary>
        public static readonly Regex HostNameOrAddressForUriRegex = GetHostNameOrAddressForUriRegex();

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
        [Display(Name = nameof(FsInfoCat.Properties.Resources.UniqueIdentifier), ResourceType = typeof(FsInfoCat.Properties.Resources))]
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

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected virtual string PropertiesToString() => $"BinaryPropertiesId={BinaryPropertiesId}";

        public override string ToString() => $@"{{ Id={_id}, Reference=""{ExtensionMethods.EscapeCsString(_reference)}"", {PropertiesToString()},
    CreatedOn={CreatedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, ModifiedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, LastSynchronizedOn={LastSynchronizedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, UpstreamId={UpstreamId},
    Notes=""{ExtensionMethods.EscapeCsString(_notes)}"" }}";

        #endregion

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ILocalRedundantSetRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] ILocalRedundantSetRow other) => ArePropertiesEqual((IRedundantSetRow)other) && EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) && LastSynchronizedOn == other.LastSynchronizedOn;

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="IRedundantSetRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] IRedundantSetRow other) => CreatedOn == other.CreatedOn &&
            ModifiedOn == other.ModifiedOn &&
            Reference == other.Reference &&
            Notes == other.Notes &&
            Status == other.Status &&
            BinaryPropertiesId.Equals(other.BinaryPropertiesId);

        public override int GetHashCode()
        {
            Guid? id = _id;
            if (id.HasValue) return id.Value.GetHashCode();
            return HashCode.Combine(_reference, Status, _notes, BinaryPropertiesId, UpstreamId, LastSynchronizedOn, CreatedOn, ModifiedOn);
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Gets the unique identifier of the current entity if it has been assigned.
        /// </summary>
        /// <param name="result">Receives the unique identifier value.</param>
        /// <returns><see langword="true" /> if the <see cref="Id" /> property has been set; otherwise, <see langword="false" />.</returns>
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

        [GeneratedRegex(@"^
(
    (?<ipv4>((?<!\d)(0(?=\d))*(?!25[6-9]|([3-9]\d|1\d\d)\d)\d{1,3}\.?){4}(?<!\.))
|
    (?=(\[[a-f\d:]+\]|[a-f\d:]+)$)
    \[?(?<ipv6>([a-f\d]{1,4}:){7}(:|[a-f\d]{1,4})|(?=(\w*:){2,7}\w*\]?$)(([a-f\d]{1,4}:)+|:):([a-f\d]{1,4}(:[a-f\d]{1,4})*)?)\]?
|
    (?=[\w-.]{1,255}(?![\w-.]))
    (?<dns>[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?)
)$", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace, "en-US")]
        private static partial Regex GetHostNameOrAddressForUriRegex();

        [GeneratedRegex(@"[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?", RegexOptions.Compiled)]
        private static partial Regex GetDnsRegex();

        [GeneratedRegex(@"([a-f\d]{1,4}:){7}(:|[a-f\d]{1,4})|(?=(\w*:){2,7}\w*\]?$)(([a-f\d]{1,4}:)+|:):([a-f\d]{1,4}(:[a-f\d]{1,4})*)?", RegexOptions.Compiled)]
        private static partial Regex GetIPV6HostRegex();

        [GeneratedRegex(@"^((?<!\d)(0(?=\d))*(?!25[6-9]|([3-9]\d|1\d\d)\d)\d{1,3}\.?){4}(?<!\.)$", RegexOptions.Compiled)]
        private static partial Regex GetIPV4HostRegex();
    }
}
