using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.PersonalTagDefinitions
{
    public record ItemEditResult(PersonalTagDefinitionListItem ItemEntity, EntityEditResultState State) : IEntityEditResult<PersonalTagDefinitionListItem>;
}
