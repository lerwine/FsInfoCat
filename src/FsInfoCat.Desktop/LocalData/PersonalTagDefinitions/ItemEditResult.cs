using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;

namespace FsInfoCat.Desktop.LocalData.PersonalTagDefinitions
{
    public record ItemEditResult(PersonalTagDefinitionListItem ItemEntity, EntityEditResultState State) : IEntityEditResult<PersonalTagDefinitionListItem>;
}
