﻿using System;

namespace FsInfoCat.Local
{
    public class CrawlConfigListItemBase : CrawlConfigurationRow, ILocalCrawlConfigurationListItem
    {
        private readonly IPropertyChangeTracker<string> _ancestorNames;
        private readonly IPropertyChangeTracker<Guid> _volumeId;
        private readonly IPropertyChangeTracker<string> _volumeDisplayName;
        private readonly IPropertyChangeTracker<string> _volumeName;
        private readonly IPropertyChangeTracker<VolumeIdentifier> _volumeIdentifier;
        private readonly IPropertyChangeTracker<string> _fileSystemDisplayName;
        private readonly IPropertyChangeTracker<string> _fileSystemSymbolicName;

        public string AncestorNames { get => _ancestorNames.GetValue(); set => _ancestorNames.SetValue(value); }

        public Guid VolumeId { get => _volumeId.GetValue(); set => _volumeId.SetValue(value); }

        public string VolumeDisplayName { get => _volumeDisplayName.GetValue(); set => _volumeDisplayName.SetValue(value); }

        public string VolumeName { get => _volumeName.GetValue(); set => _volumeName.SetValue(value); }

        public VolumeIdentifier VolumeIdentifier { get => _volumeIdentifier.GetValue(); set => _volumeIdentifier.SetValue(value); }

        public string FileSystemDisplayName { get => _fileSystemDisplayName.GetValue(); set => _fileSystemDisplayName.SetValue(value); }

        public string FileSystemSymbolicName { get => _fileSystemSymbolicName.GetValue(); set => _fileSystemSymbolicName.SetValue(value); }

        public CrawlConfigListItemBase()
        {
            _ancestorNames = AddChangeTracker(nameof(AncestorNames), "", NonNullStringCoersion.Default);
            _volumeId = AddChangeTracker(nameof(VolumeId), Guid.Empty);
            _volumeDisplayName = AddChangeTracker(nameof(VolumeDisplayName), "", NonNullStringCoersion.Default);
            _volumeName = AddChangeTracker(nameof(VolumeName), "", NonNullStringCoersion.Default);
            _volumeIdentifier = AddChangeTracker(nameof(VolumeIdentifier), VolumeIdentifier.Empty);
            _fileSystemDisplayName = AddChangeTracker(nameof(FileSystemDisplayName), "", NonNullStringCoersion.Default);
            _fileSystemSymbolicName = AddChangeTracker(nameof(FileSystemSymbolicName), "", NonNullStringCoersion.Default);
        }
    }
}