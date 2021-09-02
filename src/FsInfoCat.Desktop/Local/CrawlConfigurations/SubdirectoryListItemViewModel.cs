using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Desktop.Local.CrawlConfigurations
{
    public class SubdirectoryListItemViewModel : SubdirectoryListItemWithAncestorNamesViewModel<SubdirectoryListItemWithAncestorNames>
    {
        public SubdirectoryListItemViewModel([DisallowNull] SubdirectoryListItemWithAncestorNames entity) : base(entity) { }
    }
}
