namespace FsInfoCat.Local
{
    public class SymbolicNameListItem : SymbolicNameRow, ILocalSymbolicNameListItem
    {
        private readonly IPropertyChangeTracker<string> _fileSystemDisplayName;

        public string FileSystemDisplayName { get => _fileSystemDisplayName.GetValue(); set => _fileSystemDisplayName.SetValue(value); }

        public SymbolicNameListItem()
        {
            _fileSystemDisplayName = AddChangeTracker(nameof(FileSystemDisplayName), "", TrimmedNonNullStringCoersion.Default);
        }
    }
}
