using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;

namespace FsInfoCat.Desktop.LocalData.ImagePropertySets
{
    public record ItemEditResult(ImagePropertiesListItem ItemEntity, EntityEditResultState State) : IEntityEditResult<ImagePropertiesListItem>;
}
