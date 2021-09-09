using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.DocumentPropertySets
{
    public record ItemEditResult(DocumentPropertiesListItem ItemEntity, Microsoft.EntityFrameworkCore.EntityState Result);
}
