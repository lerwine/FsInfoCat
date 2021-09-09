using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.DRMPropertySets
{
    public record ItemEditResult(DRMPropertiesListItem ItemEntity, Microsoft.EntityFrameworkCore.EntityState Result);
}
