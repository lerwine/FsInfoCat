using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.DRMPropertySets
{
    public record ItemEditResult(DRMPropertiesListItem ItemEntity, EntityEditResultState State) : IEntityEditResult<DRMPropertiesListItem>;
}
