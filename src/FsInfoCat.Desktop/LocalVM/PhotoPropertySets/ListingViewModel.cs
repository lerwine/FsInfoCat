using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.LocalVM.PhotoPropertySets
{
    public class ListingViewModel : ListingViewModel<PhotoPropertiesListItem, ListItemViewModel, bool?>, INotifyNavigatedTo
    {
        private bool? _currentOptions;

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
                    (options.Value ? FsInfoCat.Properties.Resources.DisplayName_PhotoPropertyGroups_HasFiles :
                    FsInfoCat.Properties.Resources.DisplayName_PhotoPropertyGroups_NoExistingFiles) :
                    FsInfoCat.Properties.Resources.DisplayName_PhotoPropertyGroups_All;

        #endregion

        public ListingViewModel()
        {
            SetValue(FilterOptionsPropertyKey, new ThreeStateViewModel(true));
            UpdatePageTitle(_currentOptions);
        }

        protected override IAsyncJob ReloadAsync(bool? options)
        {
            UpdatePageTitle(options);
            return base.ReloadAsync(options);
        }

        void INotifyNavigatedTo.OnNavigatedTo() => ReloadAsync(_currentOptions);

        protected override IQueryable<PhotoPropertiesListItem> GetQueryableListing(bool? options, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            statusListener.SetMessage("Reading photo property sets from database");
            if (options.HasValue)
            {
                if (options.Value)
                    return dbContext.PhotoPropertiesListing.Where(p => p.ExistingFileCount > 0L);
                return dbContext.PhotoPropertiesListing.Where(p => p.ExistingFileCount == 0L);
            }
            return dbContext.PhotoPropertiesListing;
        }

        protected override ListItemViewModel CreateItemViewModel([DisallowNull] PhotoPropertiesListItem entity) => new(entity);

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
            "This action cannot be undone!\n\nAre you sure you want to remove this photo property set from the database?",
            "Delete Photo Property Set", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes;

        protected override async Task<int> DeleteEntityFromDbContextAsync([DisallowNull] PhotoPropertiesListItem entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            PhotoPropertySet target = await dbContext.PhotoPropertySets.FindAsync(new object[] { entity.Id }, statusListener.CancellationToken);
            if (target is null)
                return 0;
            _ = dbContext.PhotoPropertySets.Remove(target);
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
