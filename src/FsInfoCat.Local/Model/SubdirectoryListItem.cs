using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// List item DB entity that represents a subdirectory.
    /// </summary>
    /// <seealso cref="Subdirectory" />
    /// <seealso cref="LocalDbContext.SubdirectoryListing" />
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    // CodeQL [cs/inconsistent-equals-and-gethashcode]: GetHashCode() of base class is sufficient
    public class SubdirectoryListItem : SubdirectoryRow, ILocalSubdirectoryListItem, IEquatable<SubdirectoryListItem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        private const string VIEW_NAME = "vSubdirectoryListing";

        private string _crawlConfigDisplayName;

        public long SubdirectoryCount { get; set; }

        public long FileCount { get; set; }

        public long AccessErrorCount { get; set; }

        public long PersonalTagCount { get; set; }

        public long SharedTagCount { get; set; }

        [NotNull]
        [BackingField(nameof(_crawlConfigDisplayName))]
        public string CrawlConfigDisplayName { get => _crawlConfigDisplayName; set => _crawlConfigDisplayName = value.AsNonNullTrimmed(); }

        internal static void OnBuildEntity(EntityTypeBuilder<SubdirectoryListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public virtual bool Equals(SubdirectoryListItem other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public virtual bool Equals(ISubdirectoryListItem other)
        {
            if (other is null) return false;
            if (other is SubdirectoryListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalSubdirectoryListItem local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is SubdirectoryListItem listItem) return Equals(listItem);
            if (obj is ISubdirectoryListItem other)
            {
                if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
                return !other.TryGetId(out _) && (other is ILocalSubdirectoryListItem local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
            }
            return false;
        }

        protected override string PropertiesToString()
        {
            return $@"CrawlConfigDisplayName={ExtensionMethods.EscapeCsString(_crawlConfigDisplayName)},
    FileCount={FileCount}, SubdirectoryCount={FileCount}, SubdirectoryCount={AccessErrorCount}, PersonalTagCount={PersonalTagCount}, SharedTagCount={SharedTagCount},
    {base.PropertiesToString()}";
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
