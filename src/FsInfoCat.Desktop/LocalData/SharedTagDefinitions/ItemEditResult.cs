using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.SharedTagDefinitions
{
    public record ItemEditResult(SharedTagDefinitionListItem ItemEntity, Microsoft.EntityFrameworkCore.EntityState Result);
}
