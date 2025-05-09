using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// List item DB entity for set of files that have the same size, Hash and remediation status.
    /// </summary>
    /// <seealso cref="IRedundantSetListItem" />
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class RedundantSetListItem : RedundantSetRow, ILocalRedundantSetListItem, IEquatable<RedundantSetListItem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        private const string VIEW_NAME = "vRedundantSetListing";

        public long Length { get; set; }

        public MD5Hash? Hash { get; set; }

        public long RedundancyCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<RedundantSetListItem> builder)
        {
            _ = builder.ToView(VIEW_NAME);
            _ = builder.Property(nameof(Hash)).HasConversion(MD5Hash.Converter);
        }

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ILocalRedundantSetListItem" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] ILocalRedundantSetListItem other) => ArePropertiesEqual((ILocalRedundantSetRow)other) && Length == other.Length && EqualityComparer<MD5Hash?>.Default.Equals(Hash, other.Hash);

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="IRedundantSetListItem" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] IRedundantSetListItem other) => ArePropertiesEqual((IRedundantSetRow)other) && Length == other.Length && EqualityComparer<MD5Hash?>.Default.Equals(Hash, other.Hash);

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public bool Equals(RedundantSetListItem other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public bool Equals(IRedundantSetListItem other)
        {
            if (other is null) return false;
            if (other is RedundantSetListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalRedundantSetListItem local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is RedundantSetListItem listItem) return Equals(listItem);
            if (obj is IRedundantSetListItem other)
            {
                if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
                return !other.TryGetId(out _) && (other is ILocalRedundantSetListItem local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
            }
            return false;
        }

        protected override string PropertiesToString() => @$"{base.PropertiesToString()},
    Length={Length}, Hash={Hash}, RedundancyCount={RedundancyCount}";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
