using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;

namespace FsInfoCat.Desktop.LocalData.PhotoPropertySets
{
    public record ItemEditResult(PhotoPropertiesListItem ItemEntity, EntityEditResultState State) : IEntityEditResult<PhotoPropertiesListItem>;
}
