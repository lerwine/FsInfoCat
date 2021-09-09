using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.DocumentPropertySets
{
    public record ItemEditResult(DocumentPropertiesListItem ItemEntity, EntityEditResultState State) : IEntityEditResult<DocumentPropertiesListItem>;
}
