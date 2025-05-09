using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Base class for crawl configuration list item entities.
    /// </summary>
    /// <seealso cref="CrawlConfigListItem" />
    /// <seealso cref="CrawlConfigReportItem" />
    public abstract class CrawlConfigListItemBase : CrawlConfigurationRow, ILocalCrawlConfigurationListItem
    {
        private string _ancestorNames = string.Empty;
        private string _volumeDisplayName = string.Empty;
        private string _volumeName = string.Empty;
        private string _fileSystemDisplayName = string.Empty;
        private string _fileSystemSymbolicName = string.Empty;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        [NotNull]
        [BackingField(nameof(_ancestorNames))]
        public string AncestorNames { get => _ancestorNames; set => _ancestorNames = value ?? ""; }

        public virtual Guid VolumeId { get; set; }

        [NotNull]
        [BackingField(nameof(_volumeDisplayName))]
        public virtual string VolumeDisplayName { get => _volumeDisplayName; set => _volumeDisplayName = value ?? ""; }

        [NotNull]
        [BackingField(nameof(_volumeName))]
        public virtual string VolumeName { get => _volumeName; set => _volumeName = value ?? ""; }

        public virtual VolumeIdentifier VolumeIdentifier { get; set; } = VolumeIdentifier.Empty;

        [NotNull]
        [BackingField(nameof(_fileSystemDisplayName))]
        public string FileSystemDisplayName { get => _fileSystemDisplayName; set => _fileSystemDisplayName = value ?? ""; }

        [NotNull]
        [BackingField(nameof(_fileSystemSymbolicName))]
        public string FileSystemSymbolicName { get => _fileSystemSymbolicName; set => _fileSystemSymbolicName = value ?? ""; }

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

        public abstract bool Equals(ICrawlConfigurationListItem other);

        protected override string PropertiesToString() => $@"RootId{RootId},
    AncestorNames={ExtensionMethods.EscapeCsString(_ancestorNames)},
    VolumeDisplayName={ExtensionMethods.EscapeCsString(_volumeDisplayName)}, VolumeName={ExtensionMethods.EscapeCsString(_volumeName)}, FileSystemDisplayName={ExtensionMethods.EscapeCsString(_fileSystemDisplayName)}, FileSystemSymbolicName={ExtensionMethods.EscapeCsString(_fileSystemSymbolicName)},
    {base.PropertiesToString()}";
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
