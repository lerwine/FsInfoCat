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

namespace FsInfoCat.Desktop.LocalData.CrawlConfigurations
{
    public class ListingViewModel : ListingViewModel<CrawlConfigListItem, ListItemViewModel, ListingViewModel.FilterOptions, CrawlConfiguration, ItemEditResult>, INavigatedToNotifiable
    {
        private readonly EnumChoiceItem<CrawlStatus> _allOption;
        private readonly EnumChoiceItem<CrawlStatus> _allFailedOption;
        private FilterOptions _currentStatusOptions = new(null, true, false, null, null);

        #region StatusOptions Property Members

        private static readonly DependencyPropertyKey StatusOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(StatusOptions), typeof(EnumValuePickerVM<CrawlStatus>), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="StatusOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusOptionsProperty = StatusOptionsPropertyKey.DependencyProperty;

        public EnumValuePickerVM<CrawlStatus> StatusOptions => (EnumValuePickerVM<CrawlStatus>)GetValue(StatusOptionsProperty);

        #endregion
        #region EditingStatusOptions Property Members

        private static readonly DependencyPropertyKey EditingStatusOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EditingStatusOptions), typeof(EnumValuePickerVM<CrawlStatus>), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="EditingStatusOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EditingStatusOptionsProperty = EditingStatusOptionsPropertyKey.DependencyProperty;

        public EnumValuePickerVM<CrawlStatus> EditingStatusOptions => (EnumValuePickerVM<CrawlStatus>)GetValue(EditingStatusOptionsProperty);

        #endregion
        #region SchedulingOptions Property Members

        private static readonly DependencyPropertyKey SchedulingOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SchedulingOptions), typeof(ThreeStateViewModel), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="SchedulingOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SchedulingOptionsProperty = SchedulingOptionsPropertyKey.DependencyProperty;

        public ThreeStateViewModel SchedulingOptions => (ThreeStateViewModel)GetValue(SchedulingOptionsProperty);

        #endregion
        #region ScheduleRangeStart Property Members

        private static readonly DependencyPropertyKey ScheduleRangeStartPropertyKey = DependencyPropertyBuilder<ListingViewModel, DateTimeViewModel>
            .Register(nameof(ScheduleRangeStart))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ScheduleRangeStart"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ScheduleRangeStartProperty = ScheduleRangeStartPropertyKey.DependencyProperty;

        public DateTimeViewModel ScheduleRangeStart => (DateTimeViewModel)GetValue(ScheduleRangeStartProperty);

        #endregion
        #region ScheduleRangeEnd Property Members

        private static readonly DependencyPropertyKey ScheduleRangeEndPropertyKey = DependencyPropertyBuilder<ListingViewModel, DateTimeViewModel>
            .Register(nameof(ScheduleRangeEnd))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ScheduleRangeEnd"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ScheduleRangeEndProperty = ScheduleRangeEndPropertyKey.DependencyProperty;

        public DateTimeViewModel ScheduleRangeEnd => (DateTimeViewModel)GetValue(ScheduleRangeEndProperty);

        #endregion

        public ListingViewModel()
        {
            string[] names = new[] { FsInfoCat.Properties.Resources.DisplayName_AllItems, FsInfoCat.Properties.Resources.DisplayName_AllFailedItems };
            EnumValuePickerVM<CrawlStatus> viewOptions = new(names);
            _allOption = viewOptions.Choices.First(o => o.DisplayName == FsInfoCat.Properties.Resources.DisplayName_AllItems);
            _allFailedOption = viewOptions.Choices.First(o => o.DisplayName == FsInfoCat.Properties.Resources.DisplayName_AllFailedItems);
            SetValue(StatusOptionsPropertyKey, viewOptions);
            SetValue(EditingStatusOptionsPropertyKey, new EnumValuePickerVM<CrawlStatus>(names) { SelectedIndex = viewOptions.SelectedIndex });
            SetValue(SchedulingOptionsPropertyKey, new ThreeStateViewModel(_currentStatusOptions.IsScheduled));
            SetValue(ScheduleRangeStartPropertyKey, new DateTimeViewModel());
            SetValue(ScheduleRangeEndPropertyKey, new DateTimeViewModel());
            StatusOptions.SelectedItem = FromFilterOptions(_currentStatusOptions, out _);
            UpdatePageTitle(_currentStatusOptions);
        }

        private void UpdatePageTitle(FilterOptions options)
        {
            if (options.IsScheduled.HasValue)
            {
                if (options.IsScheduled.Value)
                    PageTitle = options.Status.HasValue ?
                        string.Format(FsInfoCat.Properties.Resources.FormatDisplayName_ScheduledCrawlConfigs_Status, options.Status.Value.GetDisplayName()) :
                        options.ShowAll ? FsInfoCat.Properties.Resources.DisplayName_ScheduledCrawlConfigs_All :
                        FsInfoCat.Properties.Resources.DisplayName_ScheduledCrawlConfigs_Failed;
                else
                    PageTitle = options.Status.HasValue ?
                        string.Format(FsInfoCat.Properties.Resources.FormatDisplayName_UnscheduledCrawlConfigs_Status, options.Status.Value.GetDisplayName()) :
                        options.ShowAll ? FsInfoCat.Properties.Resources.DisplayName_UnscheduledCrawlConfigs_All :
                        FsInfoCat.Properties.Resources.DisplayName_UnscheduledCrawlConfigs_Failed;
            }
            else
                PageTitle = options.Status.HasValue ?
                    string.Format(FsInfoCat.Properties.Resources.FormatDisplayName_CrawlConfigs_Status, options.Status.Value.GetDisplayName()) :
                    options.ShowAll ? FsInfoCat.Properties.Resources.DisplayName_CrawlConfigs_All : FsInfoCat.Properties.Resources.DisplayName_CrawlConfigs_Failed;
        }

        private FilterOptions ToFilterOptions(EnumChoiceItem<CrawlStatus> item, bool? isScheduled, DateTime? scheduleRangeStart, DateTime? scheduleRangeEnd)
        {
            if (ReferenceEquals(item, _allOption))
                return new(null, true, isScheduled, scheduleRangeStart, scheduleRangeEnd);
            return new(item?.Value, false, isScheduled, scheduleRangeStart, scheduleRangeEnd);
        }

        private EnumChoiceItem<CrawlStatus> FromFilterOptions(FilterOptions options, out bool? isScheduled)
        {
            isScheduled = options.IsScheduled;
            if (options.Status.HasValue)
                return StatusOptions.Choices.First(c => c.Value == options.Status);
            return options.ShowAll ? _allOption : _allFailedOption;
        }

        protected override IAsyncJob ReloadAsync(FilterOptions options)
        {
            UpdatePageTitle(options);
            return base.ReloadAsync(options);
        }

        protected override bool ConfirmItemDelete(ListItemViewModel item, object parameter) => MessageBox.Show(Application.Current.MainWindow,
            "This action cannot be undone!\n\nAre you sure you want to remove this crawl configuration from the database?",
            "Delete Crawl Configuration", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes;

        protected override ListItemViewModel CreateItemViewModel([DisallowNull] CrawlConfigListItem entity) => new(entity);

        protected override async Task<EntityEntry> DeleteEntityFromDbContextAsync([DisallowNull] CrawlConfigListItem entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            CrawlConfiguration target = await dbContext.CrawlConfigurations.FindAsync(new object[] { entity.Id }, statusListener.CancellationToken);
            if (target is null)
                return null;
            await CrawlConfiguration.DeleteAsync(target, dbContext, statusListener);
            return dbContext.Entry(target);
        }

        protected override bool EntityMatchesCurrentFilter([DisallowNull] CrawlConfigListItem entity) => _currentStatusOptions.Status.HasValue ?
            (entity.StatusValue == _currentStatusOptions.Status.Value && !_currentStatusOptions.IsScheduled.HasValue || (entity.NextScheduledStart is null) == _currentStatusOptions.IsScheduled.Value) :
            (_currentStatusOptions.ShowAll || entity.StatusValue switch
            {
                CrawlStatus.Completed or CrawlStatus.Disabled or CrawlStatus.InProgress or CrawlStatus.NotRunning => true,
                _ => false,
            });

        protected override IQueryable<CrawlConfigListItem> GetQueryableListing(FilterOptions options, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            statusListener.SetMessage("Reading crawl configurations from database");
            if (options.Status.HasValue)
            {
                CrawlStatus status = options.Status.Value;
                if (options.IsScheduled.HasValue)
                {
                    if (options.IsScheduled.Value)
                        return dbContext.CrawlConfigListing.Where(c => c.StatusValue == status && c.NextScheduledStart != null);
                    return dbContext.CrawlConfigListing.Where(c => c.StatusValue == status && c.NextScheduledStart == null);
                }
                return dbContext.CrawlConfigListing.Where(c => c.StatusValue == status);
            }
            if (options.ShowAll)
                return dbContext.CrawlConfigListing;
            return dbContext.CrawlConfigListing.Where(c => c.StatusValue != CrawlStatus.Completed && c.StatusValue != CrawlStatus.Disabled && c.StatusValue != CrawlStatus.InProgress &&
                c.StatusValue != CrawlStatus.NotRunning);
        }

        protected override void OnRefreshCommand(object parameter) => ReloadAsync(_currentStatusOptions);

        protected override void OnApplyFilterOptionsCommand(object parameter)
        {
            // TODO: Verify that this is okay
            FilterOptions newStatusOptions = ToFilterOptions(StatusOptions.SelectedItem, SchedulingOptions.Value, ScheduleRangeStart.ResultValue, ScheduleRangeEnd.ResultValue);
            if (newStatusOptions.IsScheduled != _currentStatusOptions.IsScheduled || newStatusOptions.ShowAll != _currentStatusOptions.ShowAll || newStatusOptions.Status != _currentStatusOptions.Status)
                _ = ReloadAsync(newStatusOptions);
        }

        protected override void OnCancelFilterOptionsCommand(object parameter)
        {
            UpdatePageTitle(_currentStatusOptions);
            StatusOptions.SelectedItem = FromFilterOptions(_currentStatusOptions, out bool? isScheduled);
            SchedulingOptions.Value = isScheduled;
            base.OnCancelFilterOptionsCommand(parameter);
        }

        void INavigatedToNotifiable.OnNavigatedTo() => ReloadAsync(_currentStatusOptions);

        protected override void OnReloadTaskCompleted(FilterOptions options) => _currentStatusOptions = options;

        protected override void OnReloadTaskFaulted([DisallowNull] Exception exception, FilterOptions options)
        {
            UpdatePageTitle(_currentStatusOptions);
            StatusOptions.SelectedItem = FromFilterOptions(_currentStatusOptions, out bool? isScheduled);
            SchedulingOptions.Value = isScheduled;
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is AsyncOperationFailureException aExc) ? aExc.UserMessage.NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<AsyncOperationFailureException>().Select(e => e.UserMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while loading items from the databse.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override void OnReloadTaskCanceled(FilterOptions options)
        {
            UpdatePageTitle(_currentStatusOptions);
            StatusOptions.SelectedItem = FromFilterOptions(_currentStatusOptions, out bool? isScheduled);
            SchedulingOptions.Value = isScheduled;
        }

        protected override PageFunction<ItemEditResult> GetEditPage(CrawlConfiguration args)
        {
            EditViewModel viewModel;
            if (args is null)
                viewModel = new(new CrawlConfiguration(), true);
            else
                viewModel = new EditViewModel(args, false);
            return new EditPage(viewModel);
        }

        protected override void OnEditTaskFaulted([DisallowNull] Exception exception, [DisallowNull] ListItemViewModel item)
        {
            UpdatePageTitle(_currentStatusOptions);
            StatusOptions.SelectedItem = FromFilterOptions(_currentStatusOptions, out bool? isScheduled);
            SchedulingOptions.Value = isScheduled;
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

        protected async override Task<CrawlConfiguration> LoadItemAsync([DisallowNull] CrawlConfigListItem item, [DisallowNull] IWindowsStatusListener statusListener)
        {
            using IServiceScope serviceScope = Services.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Guid id = item.Id;
            statusListener.SetMessage("Reading data");
            return await dbContext.CrawlConfigurations.Include(e => e.Root).FirstOrDefaultAsync(e => e.Id == id, statusListener.CancellationToken);
        }

        public record FilterOptions(CrawlStatus? Status, bool ShowAll, bool? IsScheduled, DateTime? ScheduleRangeStart, DateTime? ScheduleRangeEnd);
    }
}
