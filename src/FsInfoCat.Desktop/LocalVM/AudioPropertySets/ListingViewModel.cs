using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.LocalVM.AudioPropertySets
{
    public class ListingViewModel : ListingViewModel<AudioPropertiesListItem, ListItemViewModel, bool?>, INotifyNavigatedTo
    {
        private bool? _currentOptions;

        #region ListingOptions Property Members

        private static readonly DependencyPropertyKey ListingOptionsPropertyKey = DependencyPropertyBuilder<ListingViewModel, ThreeStateViewModel>
            .Register(nameof(ListingOptions))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ListingOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ListingOptionsProperty = ListingOptionsPropertyKey.DependencyProperty;

        public ThreeStateViewModel ListingOptions { get => (ThreeStateViewModel)GetValue(ListingOptionsProperty); private set => SetValue(ListingOptionsPropertyKey, value); }

        #endregion

        public ListingViewModel()
        {
            SetValue(ListingOptionsPropertyKey, new ThreeStateViewModel(true));
        }

        void INotifyNavigatedTo.OnNavigatedTo() => ReloadAsync(_currentOptions);

        protected override IQueryable<AudioPropertiesListItem> GetQueryableListing(bool? options, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            statusListener.SetMessage("Reading audio property sets from database");
            if (options.HasValue)
            {
                if (options.Value)
                    return dbContext.AudioPropertiesListing.Where(p => p.ExistingFileCount > 0L);
                return dbContext.AudioPropertiesListing.Where(p => p.ExistingFileCount == 0L);
            }
            return dbContext.AudioPropertiesListing;
        }

        protected override ListItemViewModel CreateItemViewModel([DisallowNull] AudioPropertiesListItem entity)
        {
            ListItemViewModel listItemViewModel = new(entity);
            CommonAttached.SetListItemTitle(listItemViewModel, listItemViewModel.CalculateSummaryText());
            return listItemViewModel;
        }

        protected override void OnCancelFilterOptionsCommand(object parameter)
        {
            ListingOptions.Value = _currentOptions;
            base.OnCancelFilterOptionsCommand(parameter);
        }

        protected override void OnApplyFilterOptionsCommand(object parameter)
        {
            if (_currentOptions.HasValue ? (ListingOptions.Value.HasValue && _currentOptions.Value == ListingOptions.Value.Value) : !ListingOptions.Value.HasValue)
                return;
            _currentOptions = ListingOptions.Value;
            _ = ReloadAsync(_currentOptions);
        }

        protected override void OnRefreshCommand(object parameter) => ReloadAsync(_currentOptions);

        protected override void OnItemEditCommand([DisallowNull] ListItemViewModel item, object parameter)
        {
            // TODO: Implement OnItemEditCommand(object);
        }

        protected override bool ConfirmItemDelete(ListItemViewModel item, object parameter) => MessageBox.Show(Application.Current.MainWindow,
            "This action cannot be undone!\n\nAre you sure you want to remove this audio property set from the database?",
            "Delete Audio Property Set", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes;

        protected override async Task<int> DeleteEntityFromDbContextAsync([DisallowNull] AudioPropertiesListItem entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            AudioPropertySet target = await dbContext.AudioPropertySets.FindAsync(new object[] { entity.Id }, statusListener.CancellationToken);
            if (target is null)
                return 0;
            _ = dbContext.AudioPropertySets.Remove(target);
            return await dbContext.SaveChangesAsync(statusListener.CancellationToken);
        }

        protected override void OnAddNewItemCommand(object parameter)
        {
            // TODO: Implement OnAddNewItemCommand(object);
        }
    }
}