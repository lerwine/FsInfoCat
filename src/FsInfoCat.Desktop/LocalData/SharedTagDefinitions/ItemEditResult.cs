using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.SharedTagDefinitions
{
    public record ItemEditResult(SharedTagDefinitionListItem ItemEntity, EntityEditResultState State) : IEntityEditResult<SharedTagDefinitionListItem>;
}
