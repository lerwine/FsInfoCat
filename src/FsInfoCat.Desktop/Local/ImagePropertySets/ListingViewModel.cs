using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.Local.ImagePropertySets
{
    public class ListingViewModel : ListingViewModel<ImagePropertiesListItem, ListItemViewModel, bool?>, INotifyNavigatedTo
    {
        private bool? _currentOptions;

        #region ListingOptions Property Members

        private static readonly DependencyPropertyKey ListingOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ListingOptions), typeof(ThreeStateViewModel), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ListingOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ListingOptionsProperty = ListingOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public ThreeStateViewModel ListingOptions { get => (ThreeStateViewModel)GetValue(ListingOptionsProperty); private set => SetValue(ListingOptionsPropertyKey, value); }

        #endregion

        public ListingViewModel()
        {
            SetValue(ListingOptionsPropertyKey, new ThreeStateViewModel(true));
        }

        void INotifyNavigatedTo.OnNavigatedTo() => ReloadAsync(_currentOptions);

        protected override IQueryable<ImagePropertiesListItem> GetQueryableListing(bool? options, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
        {
            statusListener.SetMessage("Reading Image proeprty sets from database");
            if (options.HasValue)
            {
                if (options.Value)
                    return dbContext.ImagePropertiesListing.Where(p => p.ExistingFileCount > 0L);
                return dbContext.ImagePropertiesListing.Where(p => p.ExistingFileCount == 0L);
            }
            return dbContext.ImagePropertiesListing;
        }

        protected override ListItemViewModel CreateItemViewModel([DisallowNull] ImagePropertiesListItem entity) => new(entity);

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
            ReloadAsync(_currentOptions);
        }

        protected override void OnRefreshCommand(object parameter) => ReloadAsync(_currentOptions);

        protected override void OnItemEditCommand([DisallowNull] ListItemViewModel item, object parameter)
        {
            // TODO: Implement OnItemEditCommand(object);
        }

        protected override bool ConfirmItemDelete(ListItemViewModel item, object parameter) => MessageBox.Show(App.Current.MainWindow,
            "This action cannot be undone!\n\nAre you sure you want to remove this image property set from the database?",
            "Delete Image Property Set", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes;

        protected override async Task<int> DeleteEntityFromDbContextAsync([DisallowNull] ImagePropertiesListItem entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            ImagePropertySet target = await dbContext.ImagePropertySets.FindAsync(new object[] { entity.Id }, statusListener.CancellationToken);
            if (target is null)
                return 0;
            dbContext.ImagePropertySets.Remove(target);
            return await dbContext.SaveChangesAsync(statusListener.CancellationToken);
        }

        protected override void OnAddNewItemCommand(object parameter)
        {
            // TODO: Implement OnAddNewItemCommand(object);
        }
    }
}