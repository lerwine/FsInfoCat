using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Desktop.LocalData.CrawlConfigurations
{
    public class SubdirectoryListItemViewModel : SubdirectoryListItemWithAncestorNamesViewModel<SubdirectoryListItemWithAncestorNames>
    {
        public SubdirectoryListItemViewModel([DisallowNull] SubdirectoryListItemWithAncestorNames entity) : base(entity) { }
    }
}
