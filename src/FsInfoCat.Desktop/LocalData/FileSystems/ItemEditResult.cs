using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.FileSystems
{
    public record ItemEditResult(FileSystemListItem ItemEntity, Microsoft.EntityFrameworkCore.EntityState Result);
}
