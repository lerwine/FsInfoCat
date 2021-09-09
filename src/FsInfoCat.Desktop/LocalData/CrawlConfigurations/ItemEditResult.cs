using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.CrawlConfigurations
{
    public record ItemEditResult(CrawlConfigListItem ItemEntity, Microsoft.EntityFrameworkCore.EntityState Result);
}
