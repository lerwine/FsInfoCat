using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.RecordedTVPropertySets
{
    public record ItemEditResult(RecordedTVPropertiesListItem ItemEntity, EntityEditResultState State) : IEntityEditResult<RecordedTVPropertiesListItem>;
}
