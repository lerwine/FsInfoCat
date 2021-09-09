using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.CrawlLogs
{
    public record ItemEditResult(CrawlJobLogListItem ItemEntity, EntityEditResultState State) : IEntityEditResult<CrawlJobLogListItem>;
}
