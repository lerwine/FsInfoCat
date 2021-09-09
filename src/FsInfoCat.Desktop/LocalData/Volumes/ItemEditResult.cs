using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.Volumes
{
    public record ItemEditResult(VolumeListItemWithFileSystem ItemEntity, Microsoft.EntityFrameworkCore.EntityState Result);
}
