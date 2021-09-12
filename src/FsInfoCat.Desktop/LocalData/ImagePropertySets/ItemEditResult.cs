using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.ImagePropertySets
{
    public record ItemEditResult(ImagePropertiesListItem ItemEntity, EntityEditResultState State) : IEntityEditResult<ImagePropertiesListItem>;
}
