using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;

namespace FsInfoCat.Desktop.LocalData.VideoPropertySets
{
    public record ItemEditResult(VideoPropertiesListItem ItemEntity, EntityEditResultState State) : IEntityEditResult<VideoPropertiesListItem>;
}
