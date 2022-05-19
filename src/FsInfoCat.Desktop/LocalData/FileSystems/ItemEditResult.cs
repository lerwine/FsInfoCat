using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;

namespace FsInfoCat.Desktop.LocalData.FileSystems
{
    public record ItemEditResult(FileSystemListItem ItemEntity, EntityEditResultState State) : IEntityEditResult<FileSystemListItem>;
}
