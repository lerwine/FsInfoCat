using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.FileSystems
{
    public record ItemEditResult(FileSystemListItem ItemEntity, EntityEditResultState State) : IEntityEditResult<FileSystemListItem>;
}
