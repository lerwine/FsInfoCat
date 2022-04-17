using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public abstract class CrawlConfigListItemBase : CrawlConfigurationRow, ILocalCrawlConfigurationListItem
    {
        private string _ancestorNames = string.Empty;
        private string _volumeDisplayName = string.Empty;
        private string _volumeName = string.Empty;
        private string _fileSystemDisplayName = string.Empty;
        private string _fileSystemSymbolicName = string.Empty;

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

        protected bool ArePropertiesEqual([DisallowNull] ILocalCrawlConfigurationListItem other) => ArePropertiesEqual((ILocalCrawlConfigurationRow)other) &&
            VolumeId.Equals(other.VolumeId) &&
            VolumeIdentifier.Equals(other.VolumeIdentifier) &&
            _ancestorNames == other.AncestorNames &&
            _volumeDisplayName == other.VolumeDisplayName &&
            _volumeName == other.VolumeName &&
            _fileSystemDisplayName == other.FileSystemDisplayName &&
            _fileSystemSymbolicName == other.FileSystemSymbolicName;

        protected bool ArePropertiesEqual([DisallowNull] ICrawlConfigurationListItem other) => ArePropertiesEqual((ICrawlConfigurationRow)other) &&
            VolumeId.Equals(other.VolumeId) &&
            VolumeIdentifier.Equals(other.VolumeIdentifier) &&
            _ancestorNames == other.AncestorNames &&
            _volumeDisplayName == other.VolumeDisplayName &&
            _volumeName == other.VolumeName &&
            _fileSystemDisplayName == other.FileSystemDisplayName &&
            _fileSystemSymbolicName == other.FileSystemSymbolicName;

        public abstract bool Equals(ICrawlConfigurationListItem other);
    }
}
