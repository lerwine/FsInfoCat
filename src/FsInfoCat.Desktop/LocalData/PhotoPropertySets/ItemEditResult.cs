using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.PhotoPropertySets
{
    public record ItemEditResult(PhotoPropertiesListItem ItemEntity, EntityEditResultState State) : IEntityEditResult<PhotoPropertiesListItem>;
}
