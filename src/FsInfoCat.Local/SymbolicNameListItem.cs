using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FsInfoCat.Local
{
    public class SymbolicNameListItem : SymbolicNameRow, ILocalSymbolicNameListItem
    {
        public const string VIEW_NAME = "vSymbolicNameListing";

        private string _fileSystemDisplayName = string.Empty;

        public string FileSystemDisplayName { get => _fileSystemDisplayName; set => _fileSystemDisplayName = value.AsWsNormalizedOrEmpty(); }

        internal static void OnBuildEntity(EntityTypeBuilder<SymbolicNameListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));
    }
}
