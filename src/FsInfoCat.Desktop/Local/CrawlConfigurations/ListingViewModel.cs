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

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public EnumValuePickerVM<CrawlStatus> EditingStatusOptions => (EnumValuePickerVM<CrawlStatus>)GetValue(EditingStatusOptionsProperty);

        #endregion
        #region CurrentIsScheduledOption Property Members

        private static readonly DependencyPropertyKey CurrentIsScheduledOptionPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CurrentIsScheduledOption), typeof(ThreeStateViewModel), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="CurrentIsScheduledOption"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CurrentIsScheduledOptionProperty = CurrentIsScheduledOptionPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public ThreeStateViewModel CurrentIsScheduledOption => (ThreeStateViewModel)GetValue(CurrentIsScheduledOptionProperty);

        #endregion
        #region EditingIsScheduledOption Property Members

        private static readonly DependencyPropertyKey EditingIsScheduledOptionPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EditingIsScheduledOption), typeof(ThreeStateViewModel), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="EditingIsScheduledOption"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EditingIsScheduledOptionProperty = EditingIsScheduledOptionPropertyKey.DependencyProperty;
        private readonly EnumChoiceItem<CrawlStatus> _allOption;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public ThreeStateViewModel EditingIsScheduledOption => (ThreeStateViewModel)GetValue(EditingIsScheduledOptionProperty);

        #endregion

        public ListingViewModel()
        {
            string[] names = new[] { FsInfoCat.Properties.Resources.DisplayName_AllItems, FsInfoCat.Properties.Resources.DisplayName_AllFailedItems };
            EnumValuePickerVM<CrawlStatus> viewOptions = new(names);
            _allOption = viewOptions.Choices.First(o => o.DisplayName == FsInfoCat.Properties.Resources.DisplayName_AllItems); ;
            SetValue(CurrentStatusOptionsPropertyKey, viewOptions);
            SetValue(EditingStatusOptionsPropertyKey, new EnumValuePickerVM<CrawlStatus>(names) { SelectedIndex = viewOptions.SelectedIndex });
            ThreeStateViewModel isScheduledOption = new(null);
            SetValue(CurrentIsScheduledOptionPropertyKey, isScheduledOption);
            SetValue(EditingIsScheduledOptionPropertyKey, new ThreeStateViewModel(isScheduledOption.Value));
        }

        protected override bool ConfirmItemDelete([DisallowNull] ListItemViewModel item, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override ListItemViewModel CreateItemViewModel([DisallowNull] CrawlConfigListItem entity) => new ListItemViewModel(entity);

        protected override async Task<int> DeleteEntityFromDbContextAsync([DisallowNull] CrawlConfigListItem entity, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
        {
            throw new NotImplementedException();
        }

        protected override IQueryable<CrawlConfigListItem> GetQueryableListing((CrawlStatus? Status, bool ShowAll, bool? IsScheduled) options, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            statusListener.BeginSetMessage("Get crawl listing items");
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
            throw new NotImplementedException();
        }

        protected override void OnItemEditCommand([DisallowNull] ListItemViewModel item, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override void OnRefreshCommand(object parameter) => ReloadAsync(CurrentStatusOptions.SelectedItem, CurrentIsScheduledOption.Value);

        protected override void OnApplyFilterOptionsCommand(object parameter)
        {
            CurrentIsScheduledOption.Value = EditingIsScheduledOption.Value;
            CurrentStatusOptions.SelectedIndex = EditingStatusOptions.SelectedIndex;
            ReloadAsync(CurrentStatusOptions.SelectedItem, CurrentIsScheduledOption.Value);
        }

        protected override void OnCancelFilterOptionsCommand(object parameter)
        {
            EditingIsScheduledOption.Value = CurrentIsScheduledOption.Value;
            EditingStatusOptions.SelectedIndex = CurrentStatusOptions.SelectedIndex;
            base.OnCancelFilterOptionsCommand(parameter);
        }

        void INotifyNavigatedTo.OnNavigatedTo() => ReloadAsync(CurrentStatusOptions.SelectedItem, CurrentIsScheduledOption.Value);

        private IAsyncJob ReloadAsync(EnumChoiceItem<CrawlStatus> selectedItem, bool? isScheduledOption)
        {
            if (selectedItem.Value.HasValue)
                return ReloadAsync((selectedItem.Value, false, isScheduledOption));
            return ReloadAsync((null, ReferenceEquals(selectedItem, _allOption), isScheduledOption));
        }
    }
}
