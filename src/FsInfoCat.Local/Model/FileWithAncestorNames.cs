using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Generic interface for file list item entities which also includes the ancestor subdirectory names.
/// </summary>
/// <seealso cref="DbFile" />
/// <seealso cref="FileWithBinaryProperties" />
/// <seealso cref="FileWithBinaryPropertiesAndAncestorNames" />
/// <seealso cref="LocalDbContext.FileListingWithAncestorNames" />
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
// CodeQL [cs/inconsistent-equals-and-gethashcode]: GetHashCode() of base class is sufficient
public class FileWithAncestorNames : DbFileRow, ILocalFileListItemWithAncestorNames, IEquatable<FileWithAncestorNames>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
{
    private const string VIEW_NAME = "vFileListingWithAncestorNames";

    private string _ancestorNames = string.Empty;
    private string _volumeDisplayName = string.Empty;
    private string _volumeName = string.Empty;
    private string _fileSystemDisplayName = string.Empty;
    private string _fileSystemSymbolicName = string.Empty;

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

    /// <summary>
    /// Gets the ancestor subdirectory names.
    /// </summary>
    /// <value>The result of a calculated column that contains the names of the parent subdirectories, separated by slash (<c>/</c>) characters, and in reverse order from
    /// typical file system path segments.</value>
    [NotNull]
    [BackingField(nameof(_ancestorNames))]
    public string AncestorNames { get => _ancestorNames; set => _ancestorNames = value.EmptyIfNullOrWhiteSpace(); }

    /// <summary>
    /// Gets the primary key of the parent volume.
    /// </summary>
    /// <value>The <see cref="IHasSimpleIdentifier.Id"/> of the parent <see cref="Volume"/>.</value>
    public Guid EffectiveVolumeId { get; set; }

    /// <summary>
    /// Gets the display name of the parent volume.
    /// </summary>
    /// <value>The <see cref="VolumeRow.DisplayName"/> of the parent <see cref="Volume"/>.</value>
    [NotNull]
    [BackingField(nameof(_volumeDisplayName))]
    public string VolumeDisplayName { get => _volumeDisplayName; set => _volumeDisplayName = value.EmptyIfNullOrWhiteSpace(); }

    /// <summary>
    /// Gets the name of the parent volume.
    /// </summary>
    /// <value>The <see cref="VolumeRow.VolumeName"/> of the parent <see cref="Volume"/>.</value>
    [NotNull]
    [BackingField(nameof(_volumeName))]
    public string VolumeName { get => _volumeName; set => _volumeName = value.EmptyIfNullOrWhiteSpace(); }

    /// <summary>
    /// Gets the parent volume identifier.
    /// </summary>
    /// <value>The <see cref="VolumeRow.Identifier"/> of the parent <see cref="Volume"/>.</value>
    public VolumeIdentifier VolumeIdentifier { get; set; } = VolumeIdentifier.Empty;

    /// <summary>
    /// Gets the display name of the file system for the parent volume.
    /// </summary>
    /// <value>The <see cref="FileSystemRow.DisplayName"/> of the <see cref="FileSystemRow">Filesystem</see> for the parent <see cref="Volume"/>.</value>
    [NotNull]
    [BackingField(nameof(_fileSystemDisplayName))]
    public string FileSystemDisplayName { get => _fileSystemDisplayName; set => _fileSystemDisplayName = value.EmptyIfNullOrWhiteSpace(); }

    /// <summary>
    /// Gets the symbolic name of the file system for the parent volume.
    /// </summary>
    /// <value>The <see cref="SymbolicNameRow.Name">symbolic name</see> of the <see cref="FileSystemRow">Filesystem</see> for the parent <see cref="Volume"/>.</value>
    [NotNull]
    [BackingField(nameof(_fileSystemSymbolicName))]
    public string FileSystemSymbolicName { get => _fileSystemSymbolicName; set => _fileSystemSymbolicName = value.EmptyIfNullOrWhiteSpace(); }

    internal static void OnBuildEntity(EntityTypeBuilder<FileWithAncestorNames> builder)
    {
        _ = builder.ToView(VIEW_NAME);
        _ = builder.Property(nameof(VolumeIdentifier)).HasConversion(VolumeIdentifier.Converter);
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public bool Equals(FileWithAncestorNames other) => other is not null && (ReferenceEquals(this, other) ||
        (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

    public bool Equals(IFileListItemWithAncestorNames other)
    {
        if (other is null) return false;
        if (other is FileWithAncestorNames listItem) return Equals(listItem);
        if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
        return !other.TryGetId(out _) && (other is ILocalFileListItemWithAncestorNames local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (obj is FileWithAncestorNames listItem) return Equals(listItem);
        if (obj is IFileListItemWithAncestorNames other)
        {
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalFileListItemWithAncestorNames local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }
        return false;
    }

    protected override string PropertiesToString()
    {
        return $@"AncestorNames={ExtensionMethods.EscapeCsString(_ancestorNames)},
    VolumeIdentifier={VolumeIdentifier}, EffectiveVolumeId={EffectiveVolumeId},
    VolumeDisplayName={ExtensionMethods.EscapeCsString(_volumeDisplayName)}, VolumeName={ExtensionMethods.EscapeCsString(_volumeName)},
    FileSystemDisplayName={ExtensionMethods.EscapeCsString(_fileSystemDisplayName)}, FileSystemSymbolicName={ExtensionMethods.EscapeCsString(_fileSystemSymbolicName)},
    AccessErrorCount={AccessErrorCount}, PersonalTagCount={PersonalTagCount}, SharedTagCount={SharedTagCount},
    {base.PropertiesToString()}";
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
