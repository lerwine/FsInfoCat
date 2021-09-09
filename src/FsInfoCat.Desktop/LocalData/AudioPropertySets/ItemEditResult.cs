using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.AudioPropertySets
{
    public record ItemEditResult(AudioPropertiesListItem ItemEntity, Microsoft.EntityFrameworkCore.EntityState Result);
}
