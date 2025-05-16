using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Generic interface for file list item entities which also includes length and hash information.
/// </summary>
/// <seealso cref="DbFile" />
/// <seealso cref="FileWithAncestorNames" />
/// <seealso cref="FileWithBinaryPropertiesAndAncestorNames" />
/// <seealso cref="LocalDbContext.FileListingWithBinaryProperties" />
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
// CodeQL [cs/inconsistent-equals-and-gethashcode]: GetHashCode() of base class is sufficient
public class FileWithBinaryProperties : DbFileRow, ILocalFileListItemWithBinaryProperties, IEquatable<FileWithBinaryProperties>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
{
    private const string VIEW_NAME = "vFileListingWithBinaryProperties";

    /// <summary>
    /// Gets the length of the file.
    /// </summary>
    /// <value>The length of the file, in bytes.</value>
    public long Length { get; set; }

    /// <summary>
    /// Gets the MD5 checksum hash.
    /// </summary>
    /// <value>The MD5 checksum hash value or <see langword="null"/> if no hash value has been calculated.</value>
    public MD5Hash? Hash { get; set; }

    /// <summary>
    /// Gets the redundancy count.
    /// </summary>
    /// <value>The number of files that are considered redundant to this file.</value>
    public long RedundancyCount { get; set; }

    /// <summary>
    /// Gets the comparison count.
    /// </summary>
    /// <value>The number of files that have been compared to this file.</value>
    public long ComparisonCount { get; set; }

    /// <summary>
    /// Gets the access error count.
    /// </summary>
    /// <value>Gets the number of access errors that occurred while attempting to access the current filesystem node.</value>
    public long AccessErrorCount { get; set; }

    /// <summary>
    /// Gets the personal tag count.
    /// </summary>
    /// <value>The number personal personal tags associated with the current filesystem node.</value>
    public long PersonalTagCount { get; set; }

    /// <summary>
    /// Gets the shared tag count.
    /// </summary>
    /// <value>The number shared tags associated with the current filesystem node.</value>
    public long SharedTagCount { get; set; }

    internal static void OnBuildEntity(EntityTypeBuilder<FileWithBinaryProperties> builder)
    {
        _ = builder.ToView(VIEW_NAME);
        _ = builder.Property(nameof(Hash)).HasConversion(MD5Hash.Converter);
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    protected override string PropertiesToString()
    {
        return $@"Length={Length}, Hash={Hash}, PersonalTagCount={RedundancyCount}, SharedTagCount={ComparisonCount},
    AccessErrorCount={AccessErrorCount}, PersonalTagCount={PersonalTagCount}, SharedTagCount={SharedTagCount},
    {base.PropertiesToString()}";
    }

    public virtual bool Equals(FileWithBinaryProperties other) => other is not null && (ReferenceEquals(this, other) ||
        (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

    public virtual bool Equals(IFileListItemWithBinaryProperties other)
    {
        if (other is null) return false;
        if (other is FileWithBinaryProperties listItem) return Equals(listItem);
        if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
        return !other.TryGetId(out _) && (other is ILocalFileListItemWithBinaryProperties local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (obj is FileWithBinaryProperties listItem) return Equals(listItem);
        if (obj is IFileListItemWithBinaryProperties other)
        {
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalFileListItemWithBinaryProperties local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }
        return false;
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
