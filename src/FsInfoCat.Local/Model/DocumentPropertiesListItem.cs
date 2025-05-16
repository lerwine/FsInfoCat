using FsInfoCat.Model;
using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// List item DB entity containing extended file properties for document files.
    /// </summary>
    /// <seealso cref="DocumentPropertySet" />
    /// <seealso cref="LocalDbContext.DocumentPropertiesListing" />
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    // CodeQL [cs/inconsistent-equals-and-gethashcode]: GetHashCode() of base class is sufficient
    public class DocumentPropertiesListItem : DocumentPropertiesRow, ILocalDocumentPropertiesListItem, IEquatable<DocumentPropertiesListItem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        private const string VIEW_NAME = "vDocumentPropertiesListing";

        /// <summary>
        /// Gets the number of non-deleted files associated with the current property set.
        /// </summary>
        /// <value>The number of non-deleted files associated with the current property set.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.Files), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public long ExistingFileCount { get; set; }

        /// <summary>
        /// Gets the total number of file entities associated with the current property set.
        /// </summary>
        /// <value>The number of files associated with the current property set, including entities representing deleted files.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.TotalFileCount), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<DocumentPropertiesListItem> builder)
        {
            _ = (builder ?? throw new ArgumentOutOfRangeException(nameof(builder))).ToView(VIEW_NAME).Property(nameof(Contributor))
                .HasConversion(MultiStringValue.Converter, MultiStringValue.Comparer);
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public bool Equals(DocumentPropertiesListItem other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public bool Equals(IDocumentPropertiesListItem other)
        {
            if (other is null) return false;
            if (other is DocumentPropertiesListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalDocumentPropertiesListItem local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(IDocumentPropertiesRow other)
        {
            if (other is null) return false;
            if (other is DocumentPropertiesListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalDocumentPropertiesRow local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(IDocumentProperties other)
        {
            if (other is null) return false;
            if (other is DocumentPropertiesListItem listItem) return Equals(listItem);
            if (other is IDocumentPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return row.TryGetId(out Guid id2) && id.Equals(id2);
                return !row.TryGetId(out _) && (row is ILocalDocumentPropertiesRow localRow) ? ArePropertiesEqual(localRow) : ArePropertiesEqual(row);
            }
            return ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is DocumentPropertiesListItem listItem) return Equals(listItem);
            if (obj is IDocumentPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return row.TryGetId(out Guid id2) && id.Equals(id2);
                return !row.TryGetId(out _) && (row is ILocalDocumentPropertiesRow localRow) ? ArePropertiesEqual(localRow) : ArePropertiesEqual(row);
            }
            return obj is IDocumentProperties properties && ArePropertiesEqual(properties);
        }

        protected override string PropertiesToString() => $@"ExistingFileCount={ExistingFileCount}, TotalFileCount={TotalFileCount},
    {base.PropertiesToString()}";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
