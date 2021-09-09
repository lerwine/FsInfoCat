using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.VideoPropertySets
{
    public record ItemEditResult(VideoPropertiesListItem ItemEntity, Microsoft.EntityFrameworkCore.EntityState Result);
}
