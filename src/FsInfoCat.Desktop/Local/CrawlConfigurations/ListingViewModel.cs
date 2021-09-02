using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.Local.CrawlConfigurations
{
    public class ListingViewModel : ListingViewModel<CrawlConfigListItem, ListItemViewModel, (CrawlStatus? Status, bool ShowAll, bool? IsScheduled)>, INotifyNavigatedTo
    {
        private readonly EnumChoiceItem<CrawlStatus> _allOption;
        private readonly EnumChoiceItem<CrawlStatus> _allFailedOption;
        private (CrawlStatus? Status, bool ShowAll, bool? IsScheduled) _currentStatusOptions = new(null, true, false);

        #region StatusOptions Property Members

        private static readonly DependencyPropertyKey StatusOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(StatusOptions), typeof(EnumValuePickerVM<CrawlStatus>), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="StatusOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusOptionsProperty = StatusOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public EnumValuePickerVM<CrawlStatus> StatusOptions => (EnumValuePickerVM<CrawlStatus>)GetValue(StatusOptionsProperty);

        #endregion
        #region EditingStatusOptions Property Members

        private static readonly DependencyPropertyKey EditingStatusOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EditingStatusOptions), typeof(EnumValuePickerVM<CrawlStatus>), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="EditingStatusOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EditingStatusOptionsProperty = EditingStatusOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public EnumValuePickerVM<CrawlStatus> EditingStatusOptions => (EnumValuePickerVM<CrawlStatus>)GetValue(EditingStatusOptionsProperty);

        #endregion
        #region SchedulingOptions Property Members

        private static readonly DependencyPropertyKey SchedulingOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SchedulingOptions), typeof(ThreeStateViewModel), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="SchedulingOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SchedulingOptionsProperty = SchedulingOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public ThreeStateViewModel SchedulingOptions => (ThreeStateViewModel)GetValue(SchedulingOptionsProperty);

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
            if (_currentStatusOptions.Status.HasValue)
                StatusOptions.SelectedValue = _currentStatusOptions.Status.Value;
            else
                StatusOptions.SelectedItem = _currentStatusOptions.ShowAll ? _allOption : _allFailedOption;
        }

        protected override bool ConfirmItemDelete(ListItemViewModel item, object parameter) => MessageBox.Show(App.Current.MainWindow,
            "This action cannot be undone!\n\nAre you sure you want to remove this crawl configuration from the database?",
            "Delete Crawl Configuration", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes;

        protected override ListItemViewModel CreateItemViewModel([DisallowNull] CrawlConfigListItem entity) => new ListItemViewModel(entity);

        protected override async Task<int> DeleteEntityFromDbContextAsync([DisallowNull] CrawlConfigListItem entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            CrawlConfiguration target = await dbContext.CrawlConfigurations.FindAsync(new object[] { entity.Id }, statusListener.CancellationToken);
            return (target is null) ? 0 : await CrawlConfiguration.DeleteAsync(target, dbContext, statusListener);
        }

        protected override IQueryable<CrawlConfigListItem> GetQueryableListing((CrawlStatus? Status, bool ShowAll, bool? IsScheduled) options, [DisallowNull] LocalDbContext dbContext,
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
            (CrawlStatus? Status, bool ShowAll, bool? IsScheduled) newStatusOptions = (StatusOptions.SelectedValue, ReferenceEquals(StatusOptions.SelectedItem, _allOption), SchedulingOptions.Value);
            if (newStatusOptions.ShowAll == _currentStatusOptions.ShowAll)
            {
                if (newStatusOptions.Status.HasValue)
                {
                    if (_currentStatusOptions.Status.HasValue && _currentStatusOptions.Status.Value == newStatusOptions.Status.Value)
                    {
                        if (newStatusOptions.IsScheduled.HasValue ? (_currentStatusOptions.IsScheduled.HasValue && _currentStatusOptions.IsScheduled.Value == newStatusOptions.IsScheduled.Value) : !_currentStatusOptions.IsScheduled.HasValue)
                        {
                            if (_currentStatusOptions.IsScheduled.HasValue && _currentStatusOptions.IsScheduled.Value == newStatusOptions.IsScheduled.Value)
                                return;
                        }
                        else if (!_currentStatusOptions.IsScheduled.HasValue)
                            return;
                    }
                }
                else if (!_currentStatusOptions.Status.HasValue)
                    return;
            }
            _currentStatusOptions = newStatusOptions;
            ReloadAsync(_currentStatusOptions);
        }

        protected override void OnCancelFilterOptionsCommand(object parameter)
        {
            CrawlStatus? status = _currentStatusOptions.Status;
            if (status.HasValue)
                StatusOptions.SelectedValue = status.Value;
            else
                StatusOptions.SelectedItem = _currentStatusOptions.ShowAll ? _allOption : _allFailedOption;
            SchedulingOptions.Value = _currentStatusOptions.IsScheduled;
            base.OnCancelFilterOptionsCommand(parameter);
        }

        void INotifyNavigatedTo.OnNavigatedTo() => ReloadAsync(_currentStatusOptions);
    }
}
