using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.VideoPropertySets
{
    public record ItemEditResult(VideoPropertiesListItem ItemEntity, EntityEditResultState State) : IEntityEditResult<VideoPropertiesListItem>;
}
