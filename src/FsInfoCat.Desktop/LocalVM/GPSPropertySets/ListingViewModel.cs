using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.LocalVM.GPSPropertySets
{
    public class ListingViewModel : ListingViewModel<GPSPropertiesListItem, ListItemViewModel, bool?>, INotifyNavigatedTo
    {
        private bool? _currentOptions = true;

        #region FilterOptions Property Members

        private static readonly DependencyPropertyKey FilterOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(FilterOptions), typeof(ThreeStateViewModel), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="FilterOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FilterOptionsProperty = FilterOptionsPropertyKey.DependencyProperty;

        public ThreeStateViewModel FilterOptions { get => (ThreeStateViewModel)GetValue(FilterOptionsProperty); private set => SetValue(FilterOptionsPropertyKey, value); }

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

        private void UpdatePageTitle(bool? options) => PageTitle = options.HasValue ?
                    (options.Value ? FsInfoCat.Properties.Resources.DisplayName_GPSPropertyGroups_HasFiles :
                    FsInfoCat.Properties.Resources.DisplayName_GPSPropertyGroups_NoExistingFiles) :
                    FsInfoCat.Properties.Resources.DisplayName_GPSPropertyGroups_All;

        #endregion

        public ListingViewModel()
        {
            SetValue(FilterOptionsPropertyKey, new ThreeStateViewModel(_currentOptions));
            UpdatePageTitle(_currentOptions);
        }

        protected override IAsyncJob ReloadAsync(bool? options)
        {
            UpdatePageTitle(options);
            return base.ReloadAsync(options);
        }

        void INotifyNavigatedTo.OnNavigatedTo() => ReloadAsync(_currentOptions);

        protected override IQueryable<GPSPropertiesListItem> GetQueryableListing(bool? options, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            statusListener.SetMessage("Reading GPS proeprty sets from database");
            if (options.HasValue)
            {
                if (options.Value)
                    return dbContext.GPSPropertiesListing.Where(p => p.ExistingFileCount > 0L);
                return dbContext.GPSPropertiesListing.Where(p => p.ExistingFileCount == 0L);
            }
            return dbContext.GPSPropertiesListing;
        }

        protected override ListItemViewModel CreateItemViewModel([DisallowNull] GPSPropertiesListItem entity) => new(entity);

        protected override void OnCancelFilterOptionsCommand(object parameter)
        {
            UpdatePageTitle(_currentOptions);
            FilterOptions.Value = _currentOptions;
            base.OnCancelFilterOptionsCommand(parameter);
        }

        protected override void OnApplyFilterOptionsCommand(object parameter)
        {
            if (_currentOptions.HasValue ? (!FilterOptions.Value.HasValue || _currentOptions.Value != FilterOptions.Value.Value) : FilterOptions.Value.HasValue)
                _ = ReloadAsync(FilterOptions.Value);
        }

        protected override void OnRefreshCommand(object parameter) => ReloadAsync(_currentOptions);

        protected override void OnItemEditCommand([DisallowNull] ListItemViewModel item, object parameter)
        {
            // TODO: Implement OnItemEditCommand(object);
        }

        protected override bool ConfirmItemDelete(ListItemViewModel item, object parameter) => MessageBox.Show(Application.Current.MainWindow,
            "This action cannot be undone!\n\nAre you sure you want to remove this GPS property set from the database?",
            "Delete GPS Property Set", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes;
        
        protected override async Task<int> DeleteEntityFromDbContextAsync([DisallowNull] GPSPropertiesListItem entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            GPSPropertySet target = await dbContext.GPSPropertySets.FindAsync(new object[] { entity.Id }, statusListener.CancellationToken);
            if (target is null)
                return 0;
            _ = dbContext.GPSPropertySets.Remove(target);
            return await dbContext.SaveChangesAsync(statusListener.CancellationToken);
        }

        protected override void OnAddNewItemCommand(object parameter)
        {
            // TODO: Implement OnAddNewItemCommand(object);
        }

        protected override void OnReloadTaskCompleted(bool? options) => _currentOptions = options;

        protected override void OnReloadTaskFaulted(Exception exception, bool? options)
        {
            UpdatePageTitle(_currentOptions);
            FilterOptions.Value = _currentOptions;
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is AsyncOperationFailureException aExc) ? aExc.UserMessage.NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<AsyncOperationFailureException>().Select(e => e.UserMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while loading items from the databse.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override void OnReloadTaskCanceled(bool? options)
        {
            UpdatePageTitle(_currentOptions);
            FilterOptions.Value = _currentOptions;
        }
    }
}
