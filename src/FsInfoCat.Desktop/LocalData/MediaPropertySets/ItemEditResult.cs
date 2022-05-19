using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;

namespace FsInfoCat.Desktop.LocalData.MediaPropertySets
{
    public record ItemEditResult(MediaPropertiesListItem ItemEntity, EntityEditResultState State) : IEntityEditResult<MediaPropertiesListItem>;
}
