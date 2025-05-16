using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Desktop.LocalData.CrawlConfigurations
{
    public class CrawlJobListItemViewModel([DisallowNull] CrawlJobLogListItem entity) : CrawlJobListItemViewModel<CrawlJobLogListItem>(entity)
    {
    }
}
