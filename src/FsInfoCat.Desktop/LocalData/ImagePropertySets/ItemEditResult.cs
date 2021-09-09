using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.ImagePropertySets
{
    public record ItemEditResult(ImagePropertiesListItem ItemEntity, Microsoft.EntityFrameworkCore.EntityState Result);
}
