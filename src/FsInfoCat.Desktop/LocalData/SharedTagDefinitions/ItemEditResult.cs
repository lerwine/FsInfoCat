using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;

namespace FsInfoCat.Desktop.LocalData.SharedTagDefinitions
{
    public record ItemEditResult(SharedTagDefinitionListItem ItemEntity, EntityEditResultState State) : IEntityEditResult<SharedTagDefinitionListItem>;
}
