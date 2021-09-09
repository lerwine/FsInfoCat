using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.CrawlLogs
{
    public record ItemEditResult(CrawlJobLogListItem ItemEntity, Microsoft.EntityFrameworkCore.EntityState Result);
}
