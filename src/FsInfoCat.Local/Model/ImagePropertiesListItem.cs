using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local.Model;

/// <summary>
/// List item DB entity containing extended file properties for image files.
/// </summary>
/// <seealso cref="ImagePropertySet" />
/// <seealso cref="LocalDbContext.ImagePropertiesListing" />
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
// CodeQL [cs/inconsistent-equals-and-gethashcode]: GetHashCode() of base class is sufficient
public class ImagePropertiesListItem : ImagePropertiesRow, ILocalImagePropertiesListItem, IEquatable<ImagePropertiesListItem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
{
    private const string VIEW_NAME = "vImagePropertiesListing";

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

    internal static void OnBuildEntity(EntityTypeBuilder<ImagePropertiesListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public bool Equals(ImagePropertiesListItem other) => other is not null && (ReferenceEquals(this, other) ||
        (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

    public bool Equals(IImagePropertiesListItem other)
    {
        if (other is null) return false;
        if (other is ImagePropertiesListItem listItem) return Equals(listItem);
        if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
        return !other.TryGetId(out _) && (other is ILocalImagePropertiesListItem local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
    }

    public override bool Equals(IImagePropertiesRow other)
    {
        if (other is null) return false;
        if (other is ImagePropertiesListItem listItem) return Equals(listItem);
        if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
        return !other.TryGetId(out _) && (other is ILocalImagePropertiesRow local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
    }

    public override bool Equals(IImageProperties other)
    {
        if (other is null) return false;
        if (other is ImagePropertiesListItem listItem) return Equals(listItem);
        if (other is IImagePropertiesRow row)
        {
            if (TryGetId(out Guid id)) return row.TryGetId(out Guid id2) && id.Equals(id2);
            return !row.TryGetId(out _) && (row is ILocalImagePropertiesRow localRow) ? ArePropertiesEqual(localRow) : ArePropertiesEqual(row);
        }
        return ArePropertiesEqual(other);
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj is ImagePropertiesListItem listItem) return Equals(listItem);
        if (obj is IImagePropertiesRow row)
        {
            if (TryGetId(out Guid id)) return row.TryGetId(out Guid id2) && id.Equals(id2);
            return !row.TryGetId(out _) && (row is ILocalImagePropertiesRow localRow) ? ArePropertiesEqual(localRow) : ArePropertiesEqual(row);
        }
        return obj is IImageProperties properties && ArePropertiesEqual(properties);
    }

    protected override string PropertiesToString() => $@"{base.PropertiesToString()},
    ExistingFileCount={ExistingFileCount}, TotalFileCount={TotalFileCount}";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
