using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.LocalData.CrawlConfigurations
{
    /* BUG: Binding errors
Severity	Count	Data Context	Binding Path	Target	Target Type	Description	File	Line	Project
Error	1	ListItemViewModel	RootPath	TextBox.Text, Name='pathTextBox'	String	RootPath property not found on object of type ListItemViewModel.	\localdata\crawlconfigurations\listitemcontrol.xaml	45	FsInfoCat.Desktop
Error	1	ListItemViewModel	FileSystemDetailText	TextBox.Text, Name='fileSystemTextBox'	String	FileSystemDetailText property not found on object of type ListItemViewModel.	\localdata\crawlconfigurations\listitemcontrol.xaml	63	FsInfoCat.Desktop

     */
    public class ListingViewModel : ListingViewModel<CrawlConfigListItem, ListItemViewModel, ListingViewModel.FilterOptions, ItemEditResult>, INavigatedToNotifiable
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

        private DispatcherOperation<PageFunction<ItemEditResult>> CreateEditPageAsync(CrawlConfiguration crawlConfiguration, SubdirectoryListItemWithAncestorNames selectedRoot, CrawlConfigListItem listitem,
            [DisallowNull] IWindowsStatusListener statusListener) => Dispatcher.InvokeAsync<PageFunction<ItemEditResult>>(() =>
            {
                if (crawlConfiguration is null || selectedRoot is null)
                {
                    _ = MessageBox.Show(Application.Current.MainWindow, "Item not found in database. Click OK to refresh listing.", "Security Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                    ReloadAsync(_currentStatusOptions);
                    return null;
                }
                return new EditPage(new(crawlConfiguration, listitem) { Root = new(selectedRoot) });
            }, DispatcherPriority.Normal, statusListener.CancellationToken);

        protected async override Task<PageFunction<ItemEditResult>> GetDetailPageAsync([DisallowNull] ListItemViewModel item, [DisallowNull] IWindowsStatusListener statusListener)
        {
            if (item is null)
                return await Dispatcher.InvokeAsync<PageFunction<ItemEditResult>>(() => new DetailsPage(new(new CrawlConfiguration(), null)));
            using IServiceScope serviceScope = Services.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Guid id = item.Entity.Id;
            CrawlConfiguration fs = await dbContext.CrawlConfigurations.FirstOrDefaultAsync(f => f.Id == id, statusListener.CancellationToken);
            if (fs is null)
            {
                await Dispatcher.ShowMessageBoxAsync("Item not found in database. Click OK to refresh listing.", "Security Exception", MessageBoxButton.OK, MessageBoxImage.Error, statusListener.CancellationToken);
                ReloadAsync(_currentStatusOptions);
                return null;
            }
            // BUG: The calling thread must be STA, because many UI components require this.
            return await Dispatcher.InvokeAsync<PageFunction<ItemEditResult>>(() => new DetailsPage(new(fs, item.Entity)));
        }

        protected override async Task<PageFunction<ItemEditResult>> GetEditPageAsync(ListItemViewModel listItem, [DisallowNull] IWindowsStatusListener statusListener)
        {
            CrawlConfiguration crawlConfiguration;
            SubdirectoryListItemWithAncestorNames selectedRoot;
            CrawlConfigListItem itemEntity;
            if (listItem is not null)
            {
                using IServiceScope serviceScope = Services.CreateScope();
                using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
                Guid id = listItem.Entity.Id;
                crawlConfiguration = await dbContext.CrawlConfigurations.FirstOrDefaultAsync(c => c.Id == id, statusListener.CancellationToken);
                if (crawlConfiguration is null)
                    selectedRoot = null;
                else
                {
                    id = crawlConfiguration.RootId;
                    selectedRoot = await dbContext.SubdirectoryListingWithAncestorNames.FirstOrDefaultAsync(d => d.Id == id);
                }
                return await CreateEditPageAsync(crawlConfiguration, selectedRoot, listItem.Entity, statusListener);
            }

            itemEntity = null;
            selectedRoot = null;
            crawlConfiguration = null;
            while (selectedRoot is null)
            {
                string path = await Dispatcher.InvokeAsync(() =>
                {
                    using System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog()
                    {
                        Description = "Select root folder",
                        ShowNewFolderButton = false,
                        SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
                    };
                    return (dialog.ShowDialog(new WindowOwner()) == System.Windows.Forms.DialogResult.OK) ? dialog.SelectedPath : null;
                }, DispatcherPriority.Background, statusListener.CancellationToken);
                if (string.IsNullOrEmpty(path))
                    return null;
                DirectoryInfo directoryInfo;
                try { directoryInfo = new(path); }
                catch (SecurityException exc)
                {
                    statusListener.Logger.LogError(exc, "Permission denied getting directory information for {Path}.", path);
                    switch (await Dispatcher.ShowMessageBoxAsync($"Permission denied while attempting to import subdirectory.", "Security Exception", MessageBoxButton.OKCancel, MessageBoxImage.Error,
                        statusListener.CancellationToken))
                    {
                        case MessageBoxResult.OK:
                            selectedRoot = null;
                            break;
                        default:
                            return null;
                    }
                    directoryInfo = null;
                }
                catch (PathTooLongException exc)
                {
                    statusListener.Logger.LogError(exc, "Error getting directory information for ({Path} is too long).", path);
                    switch (await Dispatcher.ShowMessageBoxAsync($"Path is too long. Cannnot import subdirectory as crawl root.", "Path Too Long", MessageBoxButton.OKCancel, MessageBoxImage.Error, statusListener.CancellationToken))
                    {
                        case MessageBoxResult.OK:
                            selectedRoot = null;
                            break;
                        default:
                            return null;
                    }
                    directoryInfo = null;
                }
                catch (Exception exc)
                {
                    statusListener.Logger.LogError(exc, "Error getting directory information for {Path}.", path);
                    switch (await Dispatcher.ShowMessageBoxAsync($"Unable to import subdirectory. See system logs for details.", "File System Error", MessageBoxButton.OKCancel, MessageBoxImage.Error,
                        statusListener.CancellationToken))
                    {
                        case MessageBoxResult.OK:
                            selectedRoot = null;
                            break;
                        default:
                            return null;
                    }
                    directoryInfo = null;
                }
                if (directoryInfo is null)
                {
                    selectedRoot = null;
                    crawlConfiguration = null;
                }
                else
                {
                    using IServiceScope serviceScope = Services.CreateScope();
                    using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
                    Subdirectory root = await Subdirectory.FindByFullNameAsync(path, dbContext, statusListener.CancellationToken);
                    if (root is null)
                    {
                        crawlConfiguration = null;
                        root = (await Subdirectory.ImportBranchAsync(directoryInfo, dbContext, statusListener.CancellationToken))?.Entity;
                    }
                    else
                        crawlConfiguration = await dbContext.Entry(root).GetRelatedReferenceAsync(d => d.CrawlConfiguration, statusListener.CancellationToken);
                    Guid id = root.Id;
                    selectedRoot = await dbContext.SubdirectoryListingWithAncestorNames.FirstOrDefaultAsync(d => d.Id == id, statusListener.CancellationToken);
                }
                if (crawlConfiguration is not null)
                {
                    switch (await Dispatcher.ShowMessageBoxAsync($"There is already a configuration defined for that path. Would you like to edit that configuration, instead?", "Configuration exists", MessageBoxButton.YesNoCancel,
                        MessageBoxImage.Warning, statusListener.CancellationToken))
                    {
                        case MessageBoxResult.Yes:
                            Guid id = crawlConfiguration.Id;
                            using (IServiceScope serviceScope = Services.CreateScope())
                            {
                                using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
                                itemEntity = await dbContext.CrawlConfigListing.FirstOrDefaultAsync(c => c.Id == id, statusListener.CancellationToken);
                                if (selectedRoot is null)
                                {
                                    id = crawlConfiguration.RootId;
                                    selectedRoot = await dbContext.SubdirectoryListingWithAncestorNames.FirstOrDefaultAsync(d => d.Id == id, statusListener.CancellationToken);
                                }
                            }
                            break;
                        case MessageBoxResult.No:
                            selectedRoot = null;
                            crawlConfiguration = null;
                            break;
                        default:
                            return null;
                    }
                }
            }
            return await CreateEditPageAsync(crawlConfiguration ?? new(), selectedRoot, itemEntity, statusListener);
        }

        protected override void OnEditTaskFaulted([DisallowNull] Exception exception, ListItemViewModel item)
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

        public record FilterOptions(CrawlStatus? Status, bool ShowAll, bool? IsScheduled, DateTime? ScheduleRangeStart, DateTime? ScheduleRangeEnd);
    }
}
