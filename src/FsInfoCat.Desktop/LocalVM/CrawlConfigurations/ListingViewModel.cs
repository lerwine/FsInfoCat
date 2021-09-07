using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.LocalVM.CrawlConfigurations
{
    public class ListingViewModel : ListingViewModel<CrawlConfigListItem, ListItemViewModel, ListingViewModel.FilterOptions>, INotifyNavigatedTo
    {
        private readonly EnumChoiceItem<CrawlStatus> _allOption;
        private readonly EnumChoiceItem<CrawlStatus> _allFailedOption;
        private FilterOptions _currentStatusOptions = new(null, true, false);

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
        #region PageTitle Property Members

        private static readonly DependencyPropertyKey PageTitlePropertyKey = DependencyPropertyBuilder<ListingViewModel, string>
            .Register(nameof(PageTitle))
            .DefaultValue("")
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="PageTitle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PageTitleProperty = PageTitlePropertyKey.DependencyProperty;

        public string PageTitle { get => GetValue(PageTitleProperty) as string; private set => SetValue(PageTitlePropertyKey, value); }

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
            StatusOptions.SelectedItem = FromFilterOptions(_currentStatusOptions, out _);
            UpdatePageTitle(_currentStatusOptions);
        }

        private FilterOptions ToFilterOptions(EnumChoiceItem<CrawlStatus> item, bool? isScheduled)
        {
            if (ReferenceEquals(item, _allOption))
                return new(null, true, isScheduled);
            return new(item?.Value, false, isScheduled);
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

        protected override async Task<int> DeleteEntityFromDbContextAsync([DisallowNull] CrawlConfigListItem entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            CrawlConfiguration target = await dbContext.CrawlConfigurations.FindAsync(new object[] { entity.Id }, statusListener.CancellationToken);
            return (target is null) ? 0 : await CrawlConfiguration.DeleteAsync(target, dbContext, statusListener);
        }

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

        protected override void OnAddNewItemCommand(object parameter)
        {
            // TODO: Implement OnAddNewItemCommand(object);
        }

        protected override void OnItemEditCommand([DisallowNull] ListItemViewModel item, object parameter)
        {
            // TODO: Implement OnItemEditCommand(object);
        }

        protected override void OnRefreshCommand(object parameter) => ReloadAsync(_currentStatusOptions);

        protected override void OnApplyFilterOptionsCommand(object parameter)
        {
            FilterOptions newStatusOptions = ToFilterOptions(StatusOptions.SelectedItem, SchedulingOptions.Value);
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

        void INotifyNavigatedTo.OnNavigatedTo() => ReloadAsync(_currentStatusOptions);

        protected override void OnReloadTaskCompleted(FilterOptions options) => _currentStatusOptions = options;

        protected override void OnReloadTaskFaulted(Exception exception, FilterOptions options)
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

        public record FilterOptions(CrawlStatus? Status, bool ShowAll, bool? IsScheduled);
    }
}
