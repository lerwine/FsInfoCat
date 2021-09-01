using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.Local.CrawlLogs
{
    public class ListingViewModel : ListingViewModel<CrawlJobLogListItem, ListItemViewModel, (CrawlStatus? Status, bool ShowAll)>, INotifyNavigatedTo
    {
        #region CurrentStatusOptions Property Members

        private static readonly DependencyPropertyKey CurrentStatusOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CurrentStatusOptions), typeof(EnumValuePickerVM<CrawlStatus>), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="CurrentStatusOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CurrentStatusOptionsProperty = CurrentStatusOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public EnumValuePickerVM<CrawlStatus> CurrentStatusOptions => (EnumValuePickerVM<CrawlStatus>)GetValue(CurrentStatusOptionsProperty);

        #endregion
        #region EditingStatusOptions Property Members

        private static readonly DependencyPropertyKey EditingStatusOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EditingStatusOptions), typeof(EnumValuePickerVM<CrawlStatus>), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="EditingStatusOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EditingStatusOptionsProperty = EditingStatusOptionsPropertyKey.DependencyProperty;
        private readonly EnumChoiceItem<CrawlStatus> _allOption;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public EnumValuePickerVM<CrawlStatus> EditingStatusOptions => (EnumValuePickerVM<CrawlStatus>)GetValue(EditingStatusOptionsProperty);

        #endregion

        public ListingViewModel()
        {
            string[] names = new[] { FsInfoCat.Properties.Resources.DisplayName_AllItems, FsInfoCat.Properties.Resources.DisplayName_AllFailedItems };
            EnumValuePickerVM<CrawlStatus> viewOptions = new(names);
            _allOption = viewOptions.Choices.First(o => o.DisplayName == FsInfoCat.Properties.Resources.DisplayName_AllItems); ;
            SetValue(CurrentStatusOptionsPropertyKey, viewOptions);
            SetValue(EditingStatusOptionsPropertyKey, new EnumValuePickerVM<CrawlStatus>(names) { SelectedIndex = viewOptions.SelectedIndex });
            ThreeStateViewModel isScheduledOption = new(null);
        }

        protected override IQueryable<CrawlJobLogListItem> GetQueryableListing((CrawlStatus? Status, bool ShowAll) options, [DisallowNull] LocalDbContext dbContext,
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

        protected override ListItemViewModel CreateItemViewModel([DisallowNull] CrawlJobLogListItem entity) => new ListItemViewModel(entity);

        protected override void OnApplyFilterOptionsCommand(object parameter)
        {
            CurrentStatusOptions.SelectedIndex = EditingStatusOptions.SelectedIndex;
            ReloadAsync(CurrentStatusOptions.SelectedItem);
        }

        protected override void OnCancelFilterOptionsCommand(object parameter)
        {
            EditingStatusOptions.SelectedIndex = CurrentStatusOptions.SelectedIndex;
            base.OnCancelFilterOptionsCommand(parameter);
        }

        protected override void OnRefreshCommand(object parameter) => ReloadAsync(CurrentStatusOptions.SelectedItem);

        protected override void OnItemEditCommand([DisallowNull] ListItemViewModel item, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override bool ConfirmItemDelete(ListItemViewModel item, object parameter) => MessageBox.Show(App.Current.MainWindow,
            "This action cannot be undone!\n\nAre you sure you want to remove this crawl result log entry from the database?",
            "Delete Crawl Result Log Entry", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes;

        protected override async Task<int> DeleteEntityFromDbContextAsync([DisallowNull] CrawlJobLogListItem entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            CrawlJobLog target = await dbContext.CrawlJobLogs.FindAsync(new object[] { entity.Id }, statusListener.CancellationToken);
            if (target is null)
                return 0;
            dbContext.CrawlJobLogs.Remove(target);
            return await dbContext.SaveChangesAsync(statusListener.CancellationToken);
        }

        protected override void OnAddNewItemCommand(object parameter)
        {
            throw new NotImplementedException();
        }

        void INotifyNavigatedTo.OnNavigatedTo() => ReloadAsync(CurrentStatusOptions.SelectedItem);

        private IAsyncJob ReloadAsync(EnumChoiceItem<CrawlStatus> selectedItem)
        {
            if (selectedItem.Value.HasValue)
                return ReloadAsync((selectedItem.Value, false));
            return ReloadAsync((null, ReferenceEquals(selectedItem, _allOption)));
        }
    }
}
