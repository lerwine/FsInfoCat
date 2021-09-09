using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.GPSPropertySets
{
    public record ItemEditResult(GPSPropertiesListItem ItemEntity, EntityEditResultState State) : IEntityEditResult<GPSPropertiesListItem>;
}
