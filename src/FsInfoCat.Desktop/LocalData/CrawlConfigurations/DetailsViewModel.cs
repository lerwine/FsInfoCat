using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.LocalData.CrawlConfigurations
{
    public class DetailsViewModel : CrawlConfigurationDetailsViewModel<CrawlConfiguration, SubdirectoryListItemWithAncestorNames, SubdirectoryListItemViewModel, CrawlJobLogListItem, CrawlJobListItemViewModel>
    {
        public DetailsViewModel([DisallowNull] CrawlConfiguration entity) : base(entity)
        {
        }

        protected override bool ConfirmCrawlJobLogDelete([DisallowNull] CrawlJobListItemViewModel item, object parameter)
        {
            // TODO: Implement ConfirmCrawlJobLogDelete(CrawlJobListItemViewModel, object)
            throw new NotImplementedException();
        }

        protected override CrawlJobListItemViewModel CreateCrawlJobLogViewModel([DisallowNull] CrawlJobLogListItem entity)
        {
            // TODO: Implement CreateCrawlJobLogViewModel(CrawlJobLogListItem)
            throw new NotImplementedException();
        }

        protected override Task<int> DeleteCrawlJobLogFromDbContextAsync([DisallowNull] CrawlJobLogListItem entity, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
        {
            // TODO: Implement DeleteCrawlJobLogFromDbContextAsync(CrawlJobLogListItem, LocalDbContext, IWindowsStatusListener)
            throw new NotImplementedException();
        }

        protected override IQueryable<CrawlJobLogListItem> GetQueryableCrawlJobLogListing([DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
        {
            // TODO: Implement GetQueryableCrawlJobLogListing(LocalDbContext, IWindowsStatusListener)
            throw new NotImplementedException();
        }

        protected override void OnAddNewCrawlJobLogCommand(object parameter)
        {
            // TODO: Implement OnAddNewCrawlJobLogCommand(parameter)
            throw new NotImplementedException();
        }

        protected override void OnCrawlJobLogEditCommand([DisallowNull] CrawlJobListItemViewModel item, object parameter)
        {
            // TODO: Implement OnCrawlJobLogEditCommand(CrawlJobListItemViewModel, object)
            throw new NotImplementedException();
        }

        protected override void OnReloadTaskFaulted(Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
