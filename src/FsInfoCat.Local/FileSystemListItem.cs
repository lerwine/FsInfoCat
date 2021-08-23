using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FsInfoCat.Local
{
    public class FileSystemListItem : FileSystemRow, ILocalFileSystemListItem
    {
        public const string VIEW_NAME = "vFileSystemListing";

        private readonly IPropertyChangeTracker<Guid?> _primarySymbolicNameId;
        private readonly IPropertyChangeTracker<string> _primarySymbolicName;
        private readonly IPropertyChangeTracker<long> _symbolicNameCount;
        private readonly IPropertyChangeTracker<long> _volumeCount;

        public Guid? PrimarySymbolicNameId { get => _primarySymbolicNameId.GetValue(); set => _primarySymbolicNameId.SetValue(value); }

        public string PrimarySymbolicName { get => _primarySymbolicName.GetValue(); set => _primarySymbolicName.SetValue(value); }

        public long SymbolicNameCount { get => _symbolicNameCount.GetValue(); set => _symbolicNameCount.SetValue(value); }

        public long VolumeCount { get => _volumeCount.GetValue(); set => _volumeCount.SetValue(value); }

        internal static void OnBuildEntity(EntityTypeBuilder<FileSystemListItem> builder) => builder.ToView(VIEW_NAME);

        public FileSystemListItem()
        {
            _primarySymbolicNameId = AddChangeTracker<Guid?>(nameof(PrimarySymbolicNameId), null);
            _primarySymbolicName = AddChangeTracker(nameof(PrimarySymbolicName), "", TrimmedNonNullStringCoersion.Default);
            _symbolicNameCount = AddChangeTracker(nameof(SymbolicNameCount), 0L);
            _volumeCount = AddChangeTracker(nameof(VolumeCount), 0L);
        }
    }
}
