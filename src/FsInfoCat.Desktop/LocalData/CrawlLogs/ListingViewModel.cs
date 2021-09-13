using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
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
    public class ListingViewModel : ListingViewModel<CrawlJobLogListItem, ListItemViewModel, ListingViewModel.FilterOptions, CrawlJobLog, ItemEditResult>, INavigatedToNotifiable
    {
        private FilterOptions _currentOptions = new(null, true);
        private readonly EnumChoiceItem<CrawlStatus> _allOption;
        private readonly EnumChoiceItem<CrawlStatus> _failedOption;

        #region StatusOptions Property Members

        private static readonly DependencyPropertyKey StatusOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(StatusOptions), typeof(EnumValuePickerVM<CrawlStatus>), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="StatusOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusOptionsProperty = StatusOptionsPropertyKey.DependencyProperty;

        public EnumValuePickerVM<CrawlStatus> StatusOptions => (EnumValuePickerVM<CrawlStatus>)GetValue(StatusOptionsProperty);

        #endregion

        public ListingViewModel()
        {
            string[] names = new[] { FsInfoCat.Properties.Resources.DisplayName_AllItems, FsInfoCat.Properties.Resources.DisplayName_AllFailedItems };
            EnumValuePickerVM<CrawlStatus> viewOptions = new(names);
            _allOption = viewOptions.Choices.First(o => o.DisplayName == FsInfoCat.Properties.Resources.DisplayName_AllItems);
            _failedOption = viewOptions.Choices.First(o => o.DisplayName == FsInfoCat.Properties.Resources.DisplayName_AllFailedItems);
            SetValue(StatusOptionsPropertyKey, viewOptions);
            viewOptions.SelectedItem = FromFilterOptions(_currentOptions);
            UpdatePageTitle(_currentOptions);
        }

        private void UpdatePageTitle(FilterOptions options) => PageTitle = options.Status.HasValue ?
            string.Format(FsInfoCat.Properties.Resources.FormatDisplayName_CrawlLog_Status, options.Status.Value.GetDisplayName()) :
            options.ShowAll ? FsInfoCat.Properties.Resources.DisplayName_CrawlLog_All : FsInfoCat.Properties.Resources.DisplayName_CrawlLog_Failed;

        protected override IAsyncJob ReloadAsync(FilterOptions options)
        {
            UpdatePageTitle(options);
            return base.ReloadAsync(options);
        }

        public FilterOptions ToFilterOptions(EnumChoiceItem<CrawlStatus> item) => ReferenceEquals(_allOption, item) ? (new(null, true)) : (new(item?.Value, false));

        public EnumChoiceItem<CrawlStatus> FromFilterOptions(FilterOptions value) => value.Status.HasValue
                ? StatusOptions.Choices.FirstOrDefault(c => c.Value == value.Status)
                : value.ShowAll ? _allOption : _failedOption;

        protected override bool EntityMatchesCurrentFilter([DisallowNull] CrawlJobLogListItem entity) => _currentOptions.Status.HasValue ? entity.StatusCode == _currentOptions.Status.Value :
            (_currentOptions.ShowAll || entity.StatusCode switch
            {
                CrawlStatus.Completed or CrawlStatus.Disabled or CrawlStatus.InProgress or CrawlStatus.NotRunning => true,
                _ => false,
            });

        protected override IQueryable<CrawlJobLogListItem> GetQueryableListing(FilterOptions options, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            statusListener.SetMessage("Reading crawl result log entries from database");
            if (options.Status.HasValue)
            {
                CrawlStatus status = options.Status.Value;
                return dbContext.CrawlJobListing.Where(c => c.StatusCode == status);
            }
            if (options.ShowAll)
                return dbContext.CrawlJobListing;
            return dbContext.CrawlJobListing.Where(c => c.StatusCode != CrawlStatus.Completed && c.StatusCode != CrawlStatus.Disabled && c.StatusCode != CrawlStatus.InProgress &&
                c.StatusCode != CrawlStatus.NotRunning);
        }

        protected override ListItemViewModel CreateItemViewModel([DisallowNull] CrawlJobLogListItem entity) => new(entity);

        protected override void OnApplyFilterOptionsCommand(object parameter)
        {
            FilterOptions newOptions = ToFilterOptions(StatusOptions.SelectedItem);
            if (newOptions.Status != _currentOptions.Status || newOptions.ShowAll != _currentOptions.ShowAll)
                ReloadAsync(newOptions);
        }

        protected override void OnCancelFilterOptionsCommand(object parameter)
        {
            UpdatePageTitle(_currentOptions);
            StatusOptions.SelectedItem = FromFilterOptions(_currentOptions);
            base.OnCancelFilterOptionsCommand(parameter);
        }

        protected override void OnRefreshCommand(object parameter) => ReloadAsync(_currentOptions);

        protected override bool ConfirmItemDelete(ListItemViewModel item, object parameter) => MessageBox.Show(Application.Current.MainWindow,
            "This action cannot be undone!\n\nAre you sure you want to remove this crawl result log entry from the database?",
            "Delete Crawl Result Log Entry", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes;

        protected override async Task<EntityEntry> DeleteEntityFromDbContextAsync([DisallowNull] CrawlJobLogListItem entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            CrawlJobLog target = await dbContext.CrawlJobLogs.FindAsync(new object[] { entity.Id }, statusListener.CancellationToken);
            if (target is null)
                return null;
            EntityEntry entry = dbContext.CrawlJobLogs.Remove(target);
            await dbContext.SaveChangesAsync(statusListener.CancellationToken);
            return entry;
        }

        void INavigatedToNotifiable.OnNavigatedTo() => ReloadAsync(_currentOptions);

        protected override void OnReloadTaskCompleted(FilterOptions options) => _currentOptions = options;

        protected override void OnReloadTaskFaulted([DisallowNull] Exception exception, FilterOptions options)
        {
            UpdatePageTitle(_currentOptions);
            StatusOptions.SelectedItem = FromFilterOptions(_currentOptions);
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is AsyncOperationFailureException aExc) ? aExc.UserMessage.NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<AsyncOperationFailureException>().Select(e => e.UserMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while loading items from the databse.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override void OnReloadTaskCanceled(FilterOptions options)
        {
            UpdatePageTitle(_currentOptions);
            StatusOptions.SelectedItem = FromFilterOptions(_currentOptions);
        }

        protected override async Task<PageFunction<ItemEditResult>> GetEditPageAsync(CrawlJobLog args, [DisallowNull] IWindowsStatusListener statusListener)
        {
            EditViewModel viewModel;
            if (args is null)
                viewModel = new(new CrawlJobLog(), true);
            else
                viewModel = new EditViewModel(args, false);
            return new EditPage(viewModel);
        }

        protected override async Task<CrawlJobLog> LoadItemAsync([DisallowNull] CrawlJobLogListItem item, [DisallowNull] IWindowsStatusListener statusListener)
        {
            using IServiceScope serviceScope = Services.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Guid id = item.Id;
            statusListener.SetMessage("Reading data");
            return await dbContext.CrawlJobLogs.Include(e => e.Configuration).FirstOrDefaultAsync(e => e.Id == id, statusListener.CancellationToken);
        }

        protected override void OnEditTaskFaulted([DisallowNull] Exception exception, ListItemViewModel item)
        {
            UpdatePageTitle(_currentOptions);
            StatusOptions.SelectedItem = FromFilterOptions(_currentOptions);
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is AsyncOperationFailureException aExc) ? aExc.UserMessage.NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<AsyncOperationFailureException>().Select(e => e.UserMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while loading items from the databse.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override void OnDeleteTaskFaulted([DisallowNull] Exception exception, [DisallowNull] ListItemViewModel item)
        {
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is AsyncOperationFailureException aExc) ? aExc.UserMessage.NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<AsyncOperationFailureException>().Select(e => e.UserMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while deleting the item from the databse.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public record FilterOptions(CrawlStatus? Status, bool ShowAll);
    }
}
