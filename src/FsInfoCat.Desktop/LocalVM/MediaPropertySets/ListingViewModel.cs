using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.LocalVM.MediaPropertySets
{
    public class ListingViewModel : ListingViewModel<MediaPropertiesListItem, ListItemViewModel, bool?>, INotifyNavigatedTo
    {
        private bool? _currentOptions;

        #region ListingOptions Property Members

        private static readonly DependencyPropertyKey ListingOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ListingOptions), typeof(ThreeStateViewModel), typeof(ListingViewModel),
                new PropertyMetadata(null));

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

        protected override IQueryable<MediaPropertiesListItem> GetQueryableListing(bool? options, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
        {
            statusListener.SetMessage("Reading media proeprty sets from database");
            if (options.HasValue)
            {
                if (options.Value)
                    return dbContext.MediaPropertiesListing.Where(p => p.ExistingFileCount > 0L);
                return dbContext.MediaPropertiesListing.Where(p => p.ExistingFileCount == 0L);
            }
            return dbContext.MediaPropertiesListing;
        }

        protected override ListItemViewModel CreateItemViewModel([DisallowNull] MediaPropertiesListItem entity) => new(entity);

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
            "This action cannot be undone!\n\nAre you sure you want to remove this media property set from the database?",
            "Delete Media Property Set", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes;

        protected override async Task<int> DeleteEntityFromDbContextAsync([DisallowNull] MediaPropertiesListItem entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            MediaPropertySet target = await dbContext.MediaPropertySets.FindAsync(new object[] { entity.Id }, statusListener.CancellationToken);
            if (target is null)
                return 0;
            _ = dbContext.MediaPropertySets.Remove(target);
            return await dbContext.SaveChangesAsync(statusListener.CancellationToken);
        }

        protected override void OnAddNewItemCommand(object parameter)
        {
            // TODO: Implement OnAddNewItemCommand(object);
        }
    }
}