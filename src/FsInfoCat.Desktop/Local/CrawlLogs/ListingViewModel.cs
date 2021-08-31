using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
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
            statusListener.BeginSetMessage("Get crawl log items");
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

        protected override void OnSaveFilterOptionsCommand(object parameter)
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

        protected override bool ConfirmItemDelete([DisallowNull] ListItemViewModel item, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override Task<int> DeleteEntityFromDbContextAsync([DisallowNull] CrawlJobLogListItem entity, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
        {
            throw new NotImplementedException();
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
