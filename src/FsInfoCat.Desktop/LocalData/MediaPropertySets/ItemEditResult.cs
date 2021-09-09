using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.MediaPropertySets
{
    public record ItemEditResult(MediaPropertiesListItem ItemEntity, EntityEditResultState State) : IEntityEditResult<MediaPropertiesListItem>;
}
