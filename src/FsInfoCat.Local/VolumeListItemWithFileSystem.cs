namespace FsInfoCat.Local
{
    public class VolumeListItemWithFileSystem : VolumeListItem, ILocalVolumeListItemWithFileSystem
    {
        private readonly IPropertyChangeTracker<string> _fileSystemDisplayName;
        private readonly IPropertyChangeTracker<bool> _effectiveReadOnly;
        private readonly IPropertyChangeTracker<uint> _effectiveMaxNameLength;

        public string FileSystemDisplayName { get => _fileSystemDisplayName.GetValue(); set => _fileSystemDisplayName.SetValue(value); }

        public bool EffectiveReadOnly { get => _effectiveReadOnly.GetValue(); set => _effectiveReadOnly.SetValue(value); }

        public uint EffectiveMaxNameLength { get => _effectiveMaxNameLength.GetValue(); set => _effectiveMaxNameLength.SetValue(value); }

        public VolumeListItemWithFileSystem()
        {
            _fileSystemDisplayName = AddChangeTracker(nameof(FileSystemDisplayName), "", TrimmedNonNullStringCoersion.Default);
            _effectiveReadOnly = AddChangeTracker(nameof(EffectiveReadOnly), false);
            _effectiveMaxNameLength = AddChangeTracker(nameof(EffectiveMaxNameLength), 0u);
        }
    }
}
