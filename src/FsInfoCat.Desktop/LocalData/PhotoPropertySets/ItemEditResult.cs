using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.PhotoPropertySets
{
    public record ItemEditResult(PhotoPropertiesListItem ItemEntity, Microsoft.EntityFrameworkCore.EntityState Result);
}
