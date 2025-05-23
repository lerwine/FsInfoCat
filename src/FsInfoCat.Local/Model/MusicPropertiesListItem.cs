using FsInfoCat.Model;
using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local.Model;

/// <summary>
/// List item DB entity containing extended file properties for music files.
/// </summary>
/// <seealso cref="MusicPropertySet" />
/// <seealso cref="LocalDbContext.MusicPropertiesListing" />
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
// CodeQL [cs/inconsistent-equals-and-gethashcode]: GetHashCode() of base class is sufficient
public class MusicPropertiesListItem : MusicPropertiesRow, ILocalMusicPropertiesListItem, IEquatable<MusicPropertiesListItem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
{
    private const string VIEW_NAME = "vMusicPropertiesListing";

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

    internal static void OnBuildEntity(EntityTypeBuilder<MusicPropertiesListItem> builder)
    {
        _ = builder.ToView(VIEW_NAME);
        _ = builder.Property(nameof(Artist)).HasConversion(MultiStringValue.Converter, MultiStringValue.Comparer);
        _ = builder.Property(nameof(Composer)).HasConversion(MultiStringValue.Converter, MultiStringValue.Comparer);
        _ = builder.Property(nameof(Conductor)).HasConversion(MultiStringValue.Converter, MultiStringValue.Comparer);
        _ = builder.Property(nameof(Genre)).HasConversion(MultiStringValue.Converter, MultiStringValue.Comparer);
        _ = builder.Property(nameof(Genre)).HasConversion(MultiStringValue.Converter, MultiStringValue.Comparer);
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public bool Equals(MusicPropertiesListItem other) => other is not null && (ReferenceEquals(this, other) ||
        (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

    public bool Equals(IMusicPropertiesListItem other)
    {
        if (other is null) return false;
        if (other is MusicPropertiesListItem listItem) return Equals(listItem);
        if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
        return !other.TryGetId(out _) && (other is ILocalMusicPropertiesListItem local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
    }

    public override bool Equals(IMusicPropertiesRow other)
    {
        if (other is null) return false;
        if (other is MusicPropertiesListItem listItem) return Equals(listItem);
        if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
        return !other.TryGetId(out _) && (other is ILocalMusicPropertiesRow local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
    }

    public override bool Equals(IMusicProperties other)
    {
        if (other is null) return false;
        if (other is MusicPropertiesListItem listItem) return Equals(listItem);
        if (other is IMusicPropertiesRow row)
        {
            if (TryGetId(out Guid id)) return row.TryGetId(out Guid id2) && id.Equals(id2);
            return !row.TryGetId(out _) && (row is ILocalMusicPropertiesRow localRow) ? ArePropertiesEqual(localRow) : ArePropertiesEqual(row);
        }
        return ArePropertiesEqual(other);
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj is MusicPropertiesListItem listItem) return Equals(listItem);
        if (obj is IMusicPropertiesRow row)
        {
            if (TryGetId(out Guid id)) return row.TryGetId(out Guid id2) && id.Equals(id2);
            return !row.TryGetId(out _) && (row is ILocalMusicPropertiesRow localRow) ? ArePropertiesEqual(localRow) : ArePropertiesEqual(row);
        }
        return obj is IMusicProperties properties && ArePropertiesEqual(properties);
    }

    protected override string PropertiesToString() => $@"{base.PropertiesToString()},
    ExistingFileCount={ExistingFileCount}, TotalFileCount={TotalFileCount}";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
