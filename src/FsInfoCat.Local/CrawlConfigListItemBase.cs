using System;

namespace FsInfoCat.Local
{
    public class CrawlConfigListItemBase : CrawlConfigurationRow, ILocalCrawlConfigurationListItem
    {
        private string _ancestorNames = string.Empty;
        private string _volumeDisplayName = string.Empty;
        private string _volumeName = string.Empty;
        private string _fileSystemDisplayName = string.Empty;
        private string _fileSystemSymbolicName = string.Empty;

        public string AncestorNames { get => _ancestorNames; set => _ancestorNames = value ?? ""; }

        public virtual Guid VolumeId { get; set; }

        public virtual string VolumeDisplayName { get => _volumeDisplayName; set => _volumeDisplayName = value ?? ""; }

        public virtual string VolumeName { get => _volumeName; set => _volumeName = value ?? ""; }

        public virtual VolumeIdentifier VolumeIdentifier { get; set; } = VolumeIdentifier.Empty;

        public string FileSystemDisplayName { get => _fileSystemDisplayName; set => _fileSystemDisplayName = value ?? ""; }

        public string FileSystemSymbolicName { get => _fileSystemSymbolicName; set => _fileSystemSymbolicName = value ?? ""; }
    }
}
