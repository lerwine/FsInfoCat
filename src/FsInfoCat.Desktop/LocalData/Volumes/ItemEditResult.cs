using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.Volumes
{
    public record ItemEditResult(VolumeListItemWithFileSystem ItemEntity, EntityEditResultState State) : IEntityEditResult<VolumeListItemWithFileSystem>;
}
