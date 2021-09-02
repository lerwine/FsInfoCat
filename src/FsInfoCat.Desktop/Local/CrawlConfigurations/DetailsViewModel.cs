using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.Local.CrawlConfigurations
{
    public class DetailsViewModel : CrawlConfigurationDetailsViewModel<CrawlConfiguration, SubdirectoryListItemWithAncestorNames, SubdirectoryListItemViewModel, CrawlJobLogListItem, CrawlJobListItemViewModel>
    {
        public DetailsViewModel([DisallowNull] CrawlConfiguration entity, [DisallowNull] SubdirectoryListItemViewModel root) : base(entity, root)
        {
            if (root is null)
                throw new ArgumentNullException(nameof(root));
        }

        protected override bool ConfirmCrawlJobLogDelete([DisallowNull] CrawlJobListItemViewModel item, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override CrawlJobListItemViewModel CreateCrawlJobLogViewModel([DisallowNull] CrawlJobLogListItem entity)
        {
            throw new NotImplementedException();
        }

        protected override Task<int> DeleteCrawlJobLogFromDbContextAsync([DisallowNull] CrawlJobLogListItem entity, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
        {
            throw new NotImplementedException();
        }

        protected override IQueryable<CrawlJobLogListItem> GetQueryableCrawlJobLogListing([DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
        {
            throw new NotImplementedException();
        }

        protected override void OnAddNewCrawlJobLogCommand(object parameter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCrawlJobLogEditCommand([DisallowNull] CrawlJobListItemViewModel item, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override void OnRefreshCrawlJobLogsCommand(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
