using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.MusicPropertySets
{
    public record ItemEditResult(MusicPropertiesListItem ItemEntity, Microsoft.EntityFrameworkCore.EntityState Result);
}
