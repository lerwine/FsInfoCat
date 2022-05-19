using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;

namespace FsInfoCat.Desktop.LocalData.CrawlConfigurations
{
    public record ItemEditResult(CrawlConfigListItem ItemEntity, EntityEditResultState State) : IEntityEditResult<CrawlConfigListItem>;
}
