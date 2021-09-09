using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.PersonalTagDefinitions
{
    public record ItemEditResult(PersonalTagDefinitionListItem ItemEntity, Microsoft.EntityFrameworkCore.EntityState Result);
}
