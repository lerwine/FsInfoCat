using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;

namespace FsInfoCat.Desktop.LocalData.GPSPropertySets
{
    public record ItemEditResult(GPSPropertiesListItem ItemEntity, EntityEditResultState State) : IEntityEditResult<GPSPropertiesListItem>;
}
