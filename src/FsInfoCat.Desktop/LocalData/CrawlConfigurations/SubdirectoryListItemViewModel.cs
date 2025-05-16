using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Desktop.LocalData.CrawlConfigurations
{
    public class SubdirectoryListItemViewModel([DisallowNull] SubdirectoryListItemWithAncestorNames entity) : SubdirectoryListItemWithAncestorNamesViewModel<SubdirectoryListItemWithAncestorNames>(entity)
    {
    }
}
