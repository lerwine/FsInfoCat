using FsInfoCat.Activities;
using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
namespace FsInfoCat.Desktop.LocalData.CrawlLogs
{
    public class ListingViewModel : ListingViewModel<CrawlJobLogListItem, ListItemViewModel, ListingViewModel.FilterOptions>, INavigatedToNotifiable
    {
        private FilterOptions _currentOptions = new(null, true);
        private readonly EnumChoiceItem<Model.CrawlStatus> _allOption;
        private readonly EnumChoiceItem<Model.CrawlStatus> _failedOption;

        #region StatusOptions Property Members

        private static readonly DependencyPropertyKey StatusOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(StatusOptions), typeof(EnumValuePickerVM<Model.CrawlStatus>), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="StatusOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusOptionsProperty = StatusOptionsPropertyKey.DependencyProperty;

        public EnumValuePickerVM<Model.CrawlStatus> StatusOptions => (EnumValuePickerVM<Model.CrawlStatus>)GetValue(StatusOptionsProperty);

        #endregion

        public ListingViewModel()
        {
            string[] names = new[] { FsInfoCat.Properties.Resources.DisplayName_AllItems, FsInfoCat.Properties.Resources.DisplayName_AllFailedItems };
            EnumValuePickerVM<Model.CrawlStatus> viewOptions = new(names);
            _allOption = viewOptions.Choices.First(o => o.DisplayName == FsInfoCat.Properties.Resources.DisplayName_AllItems);
            _failedOption = viewOptions.Choices.First(o => o.DisplayName == FsInfoCat.Properties.Resources.DisplayName_AllFailedItems);
            SetValue(StatusOptionsPropertyKey, viewOptions);
            viewOptions.SelectedItem = FromFilterOptions(_currentOptions);
            UpdatePageTitle(_currentOptions);
        }

        private void UpdatePageTitle(FilterOptions options) => PageTitle = options.Status.HasValue ?
            string.Format(FsInfoCat.Properties.Resources.FormatDisplayName_CrawlLog_Status, options.Status.Value.GetDisplayName()) :
            options.ShowAll ? FsInfoCat.Properties.Resources.DisplayName_CrawlLog_All : FsInfoCat.Properties.Resources.DisplayName_CrawlLog_Failed;

        protected override IAsyncAction<IActivityEvent> RefreshAsync(FilterOptions options)
        {
            UpdatePageTitle(options);
            return base.RefreshAsync(options);
        }

        public FilterOptions ToFilterOptions(EnumChoiceItem<Model.CrawlStatus> item) => ReferenceEquals(_allOption, item) ? (new(null, true)) : (new(item?.Value, false));

        public EnumChoiceItem<Model.CrawlStatus> FromFilterOptions(FilterOptions value) => value.Status.HasValue
                ? StatusOptions.Choices.FirstOrDefault(c => c.Value == value.Status)
                : value.ShowAll ? _allOption : _failedOption;

        protected override bool EntityMatchesCurrentFilter([DisallowNull] CrawlJobLogListItem entity) => _currentOptions.Status.HasValue ? entity.StatusCode == _currentOptions.Status.Value :
            (_currentOptions.ShowAll || entity.StatusCode switch
            {
                Model.CrawlStatus.Completed or Model.CrawlStatus.Disabled or Model.CrawlStatus.InProgress or Model.CrawlStatus.NotRunning => true,
                _ => false,
            });

        protected override IQueryable<CrawlJobLogListItem> GetQueryableListing(FilterOptions options, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IActivityProgress progress)
        {
            progress.Report("Reading crawl result log entries from database");
            if (options.Status.HasValue)
            {
                Model.CrawlStatus status = options.Status.Value;
                return dbContext.CrawlJobListing.Where(c => c.StatusCode == status);
            }
            if (options.ShowAll)
                return dbContext.CrawlJobListing;
            return dbContext.CrawlJobListing.Where(c => c.StatusCode != Model.CrawlStatus.Completed && c.StatusCode != Model.CrawlStatus.Disabled && c.StatusCode != Model.CrawlStatus.InProgress &&
                c.StatusCode != Model.CrawlStatus.NotRunning);
        }

        protected override ListItemViewModel CreateItemViewModel([DisallowNull] CrawlJobLogListItem entity) => new(entity);

        protected override void OnApplyFilterOptionsCommand(object parameter)
        {
            FilterOptions newOptions = ToFilterOptions(StatusOptions.SelectedItem);
            if (newOptions.Status != _currentOptions.Status || newOptions.ShowAll != _currentOptions.ShowAll)
                RefreshAsync(newOptions);
        }

        protected override void OnCancelFilterOptionsCommand(object parameter)
        {
            UpdatePageTitle(_currentOptions);
            StatusOptions.SelectedItem = FromFilterOptions(_currentOptions);
            base.OnCancelFilterOptionsCommand(parameter);
        }

        protected override void OnRefreshCommand(object parameter) => RefreshAsync(_currentOptions);

        protected override bool ConfirmItemDelete(ListItemViewModel item, object parameter) => MessageBox.Show(Application.Current.MainWindow,
            "This action cannot be undone!\n\nAre you sure you want to remove this crawl result log entry from the database?",
            "Delete Crawl Result Log Entry", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes;

        protected override async Task<EntityEntry> DeleteEntityFromDbContextAsync([DisallowNull] CrawlJobLogListItem entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IActivityProgress progress)
        {
            CrawlJobLog target = await dbContext.CrawlJobLogs.FindAsync(new object[] { entity.Id }, progress.Token);
            if (target is null)
                return null;
            EntityEntry entry = dbContext.CrawlJobLogs.Remove(target);
            await dbContext.SaveChangesAsync(progress.Token);
            return entry;
        }

        void INavigatedToNotifiable.OnNavigatedTo() => RefreshAsync(_currentOptions);

        protected override void OnReloadTaskCompleted(FilterOptions options) => _currentOptions = options;

        protected override void OnReloadTaskFaulted([DisallowNull] Exception exception, FilterOptions options)
        {
            UpdatePageTitle(_currentOptions);
            StatusOptions.SelectedItem = FromFilterOptions(_currentOptions);
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is ActivityException aExc) ? aExc.ToString().NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<ActivityException>().Select(e => e.ToString())
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while loading items from the database.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override void OnReloadTaskCanceled(FilterOptions options)
        {
            UpdatePageTitle(_currentOptions);
            StatusOptions.SelectedItem = FromFilterOptions(_currentOptions);
        }

        protected override Task<PageFunction<ItemFunctionResultEventArgs>> GetDetailPageAsync([DisallowNull] ListItemViewModel item, [DisallowNull] IActivityProgress progress) => GetEditPageAsync(item, progress);

        protected async override Task<PageFunction<ItemFunctionResultEventArgs>> GetEditPageAsync(ListItemViewModel item, [DisallowNull] IActivityProgress progress)
        {
            if (item is null)
                return await Dispatcher.InvokeAsync<PageFunction<ItemFunctionResultEventArgs>>(() => new EditPage(new(new(), null)));
            using IServiceScope serviceScope = Hosting.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Guid id = item.Entity.Id;
            CrawlJobLog crawlJobLog = await dbContext.CrawlJobLogs.FirstOrDefaultAsync(j => j.Id == id, progress.Token);
            if (crawlJobLog is null)
            {
                await Dispatcher.ShowMessageBoxAsync("Item not found in database. Click OK to refresh listing.", "Security Exception", MessageBoxButton.OK, MessageBoxImage.Error, progress.Token);
                RefreshAsync(_currentOptions);
                return null;
            }
            return await Dispatcher.InvokeAsync<PageFunction<ItemFunctionResultEventArgs>>(() => new EditPage(new(crawlJobLog, item.Entity)));
        }

        protected override void OnEditTaskFaulted([DisallowNull] Exception exception, ListItemViewModel item)
        {
            UpdatePageTitle(_currentOptions);
            StatusOptions.SelectedItem = FromFilterOptions(_currentOptions);
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is ActivityException aExc) ? aExc.ToString().NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<ActivityException>().Select(e => e.ToString())
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while loading items from the database.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override void OnDeleteTaskFaulted([DisallowNull] Exception exception, [DisallowNull] ListItemViewModel item)
        {
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is ActivityException aExc) ? aExc.ToString().NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<ActivityException>().Select(e => e.ToString())
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while deleting the item from the database.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public record FilterOptions(Model.CrawlStatus? Status, bool ShowAll);
    }
}
