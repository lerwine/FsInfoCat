using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Base class for crawl configuration list item entities.
/// </summary>
/// <seealso cref="CrawlConfigListItem" />
/// <seealso cref="CrawlConfigReportItem" />
/// <seealso cref="LocalDbContext.CrawlConfigListing" />
/// <seealso cref="LocalDbContext.CrawlConfigReport" />
public abstract class CrawlConfigListItemBase : CrawlConfigurationRow, ILocalCrawlConfigurationListItem
{
    private string _ancestorNames = string.Empty;
    private string _volumeDisplayName = string.Empty;
    private string _volumeName = string.Empty;
    private string _fileSystemDisplayName = string.Empty;
    private string _fileSystemSymbolicName = string.Empty;

    /// <summary>
    /// Gets the root subdirectory ancestor path element names.
    /// </summary>
    /// <value>The result of a calculated column that contains the names of the root <see cref="Subdirectory"/> path elements, separated by slash (<c>/</c>) characters,
    /// and in reverse order from typical file system path segments.</value>
    [NotNull]
    [BackingField(nameof(_ancestorNames))]
    public string AncestorNames { get => _ancestorNames; set => _ancestorNames = value ?? ""; }

    /// <summary>
    /// Gets the primary key of the volume containing the root subdirectory for the current crawl configuration.
    /// </summary>
    /// <value>The <see cref="IHasSimpleIdentifier.Id"/> of the <see cref="VolumeRow"/> containing the
    /// root <see cref="Subdirectory"/> for the current crawl configuration.</value>
    public virtual Guid VolumeId { get; set; }

    /// <summary>
    /// Gets the display name of the volume containing the root subdirectory for the current crawl configuration.
    /// </summary>
    /// <value>The <see cref="VolumeRow.DisplayName"/> of the <see cref="VolumeRow"/> containing the
    /// root <see cref="Subdirectory"/> for the current crawl configuration.</value>
    [NotNull]
    [BackingField(nameof(_volumeDisplayName))]
    public virtual string VolumeDisplayName { get => _volumeDisplayName; set => _volumeDisplayName = value ?? ""; }

    /// <summary>
    /// Gets the name of the volume containing the root subdirectory for the current crawl configuration.
    /// </summary>
    /// <value>The <see cref="VolumeRow.VolumeName"/> of the <see cref="VolumeRow"/> containing the root <see cref="Subdirectory"/> for the current crawl configuration.</value>
    [NotNull]
    [BackingField(nameof(_volumeName))]
    public virtual string VolumeName { get => _volumeName; set => _volumeName = value ?? ""; }

    /// <summary>
    /// Gets the identifier of the volume containing the root subdirectory for the current crawl configuration.
    /// </summary>
    /// <value>The <see cref="VolumeRow.Identifier"/> of the <see cref="VolumeRow"/> containing the root <see cref="Subdirectory"/> for the current crawl configuration.</value>
    public virtual VolumeIdentifier VolumeIdentifier { get; set; } = VolumeIdentifier.Empty;

    /// <summary>
    /// Gets the display name of the file system for the volume containing the root subdirectory for the current crawl configuration.
    /// </summary>
    /// <value>The <see cref="FileSystemRow.DisplayName"/> of the <see cref="FileSystemRow">Filesystem</see> for the <see cref="VolumeRow"/> containing the
    /// root <see cref="Subdirectory"/> for the current crawl configuration.</value>
    [NotNull]
    [BackingField(nameof(_fileSystemDisplayName))]
    public string FileSystemDisplayName { get => _fileSystemDisplayName; set => _fileSystemDisplayName = value ?? ""; }

    /// <summary>
    /// Gets the symbolic name of the file system for the volume containing the root subdirectory for the current crawl configuration.
    /// </summary>
    /// <value>The <see cref="SymbolicNameRow.Name">symbolic name</see> of the <see cref="FileSystemRow">Filesystem</see> for the <see cref="VolumeRow"/> containing the
    /// root <see cref="Subdirectory"/> for the current crawl configuration.</value>
    [NotNull]
    [BackingField(nameof(_fileSystemSymbolicName))]
    public string FileSystemSymbolicName { get => _fileSystemSymbolicName; set => _fileSystemSymbolicName = value ?? ""; }

    /// <summary>
    /// Gets the primary key of the root directory entity.
    /// </summary>
    /// <value>The primary key value of the root <see cref="Subdirectory"/> entity.</value>
    public override Guid RootId { get; set; }

    /// <summary>
    /// Checks for equality by comparing property values.
    /// </summary>
    /// <param name="other">The other <see cref="ILocalCrawlConfigurationListItem" /> to compare to.</param>
    /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
    protected bool ArePropertiesEqual([DisallowNull] ILocalCrawlConfigurationListItem other) => ArePropertiesEqual((ILocalCrawlConfigurationRow)other) &&
        VolumeId.Equals(other.VolumeId) &&
        VolumeIdentifier.Equals(other.VolumeIdentifier) &&
        _ancestorNames == other.AncestorNames &&
        _volumeDisplayName == other.VolumeDisplayName &&
        _volumeName == other.VolumeName &&
        _fileSystemDisplayName == other.FileSystemDisplayName &&
        _fileSystemSymbolicName == other.FileSystemSymbolicName;

    /// <summary>
    /// Checks for equality by comparing property values.
    /// </summary>
    /// <param name="other">The other <see cref="ICrawlConfigurationListItem" /> to compare to.</param>
    /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
    protected bool ArePropertiesEqual([DisallowNull] ICrawlConfigurationListItem other) => ArePropertiesEqual((ICrawlConfigurationRow)other) &&
        VolumeId.Equals(other.VolumeId) &&
        VolumeIdentifier.Equals(other.VolumeIdentifier) &&
        _ancestorNames == other.AncestorNames &&
        _volumeDisplayName == other.VolumeDisplayName &&
        _volumeName == other.VolumeName &&
        _fileSystemDisplayName == other.FileSystemDisplayName &&
        _fileSystemSymbolicName == other.FileSystemSymbolicName;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public abstract bool Equals(ICrawlConfigurationListItem other);

    protected override string PropertiesToString() => $@"RootId{RootId},
    AncestorNames={ExtensionMethods.EscapeCsString(_ancestorNames)},
    VolumeDisplayName={ExtensionMethods.EscapeCsString(_volumeDisplayName)}, VolumeName={ExtensionMethods.EscapeCsString(_volumeName)}, FileSystemDisplayName={ExtensionMethods.EscapeCsString(_fileSystemDisplayName)}, FileSystemSymbolicName={ExtensionMethods.EscapeCsString(_fileSystemSymbolicName)},
    {base.PropertiesToString()}";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
