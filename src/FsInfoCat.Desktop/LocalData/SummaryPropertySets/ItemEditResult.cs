using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;

namespace FsInfoCat.Desktop.LocalData.SummaryPropertySets
{
    public record ItemEditResult(SummaryPropertiesListItem ItemEntity, EntityEditResultState State) : IEntityEditResult<SummaryPropertiesListItem>;
}
