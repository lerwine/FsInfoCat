using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.GPSPropertySets
{
    public record ItemEditResult(GPSPropertiesListItem ItemEntity, Microsoft.EntityFrameworkCore.EntityState Result);
}
