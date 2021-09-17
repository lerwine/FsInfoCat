using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FsInfoCat.Local
{
    public class SymbolicNameListItem : SymbolicNameRow, ILocalSymbolicNameListItem
    {
        public const string VIEW_NAME = "vSymbolicNameListing";

        private readonly IPropertyChangeTracker<string> _fileSystemDisplayName;

        public string FileSystemDisplayName { get => _fileSystemDisplayName.GetValue(); set => _fileSystemDisplayName.SetValue(value); }

        internal static void OnBuildEntity(EntityTypeBuilder<SymbolicNameListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));

        public SymbolicNameListItem()
        {
            _fileSystemDisplayName = AddChangeTracker(nameof(FileSystemDisplayName), "", TrimmedNonNullStringCoersion.Default);
        }
    }
}
