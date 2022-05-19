using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;

namespace FsInfoCat.Desktop.LocalData.RecordedTVPropertySets
{
    public record ItemEditResult(RecordedTVPropertiesListItem ItemEntity, EntityEditResultState State) : IEntityEditResult<RecordedTVPropertiesListItem>;
}
