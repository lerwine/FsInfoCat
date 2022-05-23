using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Model
{
    // TODO: Document CrawlJobLogListItem class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class CrawlJobLogListItem : CrawlJobLogRow, ILocalCrawlJobListItem, IEquatable<CrawlJobLogListItem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        private string _configurationDisplayName = string.Empty;
        private const string VIEW_NAME = "vCrawlJobListing";

        [NotNull]
        [BackingField(nameof(_configurationDisplayName))]
        public string ConfigurationDisplayName { get => _configurationDisplayName; set => _configurationDisplayName = value.AsNonNullTrimmed(); }

        internal static void OnBuildEntity([DisallowNull] EntityTypeBuilder<CrawlJobLogListItem> builder) => (builder ?? throw new ArgumentOutOfRangeException(nameof(builder)))
            .ToView(VIEW_NAME);

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ILocalCrawlJobListItem" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] ILocalCrawlJobListItem other) => ArePropertiesEqual((ICrawlJobListItem)other) && EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) && LastSynchronizedOn == other.LastSynchronizedOn;

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ICrawlJobListItem" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] ICrawlJobListItem other) => ArePropertiesEqual((ICrawlJobLogRow)other) && ConfigurationId.Equals(other.ConfigurationId);

        public bool Equals(CrawlJobLogListItem other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)) &&
            ConfigurationId.Equals(other.ConfigurationId));

        public override bool Equals(ICrawlJob other)
        {
            if (other is null) return false;
            if (other is CrawlJobLogListItem crawlJobLogListItem) return Equals(crawlJobLogListItem);
            if (other is ICrawlJobLogRow row)
            {
                if (TryGetId(out Guid id)) id.Equals(row.Id);
                if (!row.Id.Equals(Guid.Empty)) return false;
                if (row is ILocalCrawlJobListItem localListItem)
                    return ArePropertiesEqual(localListItem);
                if (row is ICrawlJobListItem listItem)
                    return ArePropertiesEqual(listItem);
                if (row is (ILocalCrawlJobLogRow localRow))
                    return ArePropertiesEqual(localRow);
                return ArePropertiesEqual(row);
            }
            return ArePropertiesEqual(other);
        }

        public bool Equals(ICrawlJobListItem other)
        {
            if (other is null) return false;
            if (other is CrawlJobLogListItem crawlJobLogListItem) return Equals(crawlJobLogListItem);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            if (other.TryGetId(out _)) return false;
            if (other is ILocalCrawlJobListItem localListItem)
                return ArePropertiesEqual(localListItem);
            if (other is ICrawlJobListItem listItem)
                return ArePropertiesEqual(listItem);
            if (other is (ILocalCrawlJobLogRow localRow))
                return ArePropertiesEqual(localRow);
            return ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is CrawlJobLogListItem crawlJobLogListItem) return Equals(crawlJobLogListItem);
            if (obj is ICrawlJobLogRow row)
            {
                if (TryGetId(out Guid id)) id.Equals(row.Id);
                if (!row.Id.Equals(Guid.Empty)) return false;
                if (row is ILocalCrawlJobListItem localListItem)
                    return ArePropertiesEqual(localListItem);
                if (row is ICrawlJobListItem listItem)
                    return ArePropertiesEqual(listItem);
                if (row is (ILocalCrawlJobLogRow localRow))
                    return ArePropertiesEqual(localRow);
                return ArePropertiesEqual(row);
            }
            return obj is ICrawlJob job && ArePropertiesEqual(job);
        }

        protected override string PropertiesToString() => $@"ConfigurationDisplayName=""{ExtensionMethods.EscapeCsString(_configurationDisplayName)}"",
    {base.PropertiesToString()}";
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
