using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.MediaPropertySets
{
    public record ItemEditResult(MediaPropertiesListItem ItemEntity, Microsoft.EntityFrameworkCore.EntityState Result);
}
