using System;

namespace FsInfoCat.Local
{
    public class FileSystemListItem : FileSystemRow, ILocalFileSystemListItem
    {
        private readonly IPropertyChangeTracker<Guid?> _primarySymbolicNameId;
        private readonly IPropertyChangeTracker<string> _primarySymbolicName;
        private readonly IPropertyChangeTracker<long> _symbolicNameCount;

        public Guid? PrimarySymbolicNameId { get => _primarySymbolicNameId.GetValue(); set => _primarySymbolicNameId.SetValue(value); }

        public string PrimarySymbolicName { get => _primarySymbolicName.GetValue(); set => _primarySymbolicName.SetValue(value); }

        public long SymbolicNameCount { get => _symbolicNameCount.GetValue(); set => _symbolicNameCount.SetValue(value); }

        public FileSystemListItem()
        {
            _primarySymbolicNameId = AddChangeTracker<Guid?>(nameof(PrimarySymbolicNameId), null);
            _primarySymbolicName = AddChangeTracker(nameof(PrimarySymbolicName), "", TrimmedNonNullStringCoersion.Default);
            _symbolicNameCount = AddChangeTracker(nameof(SymbolicNameCount), 0L);
        }
    }
}
