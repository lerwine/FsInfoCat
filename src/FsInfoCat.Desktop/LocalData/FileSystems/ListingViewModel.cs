using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.LocalData.FileSystems
{
    public class ListingViewModel : ListingViewModel<FileSystemListItem, ListItemViewModel, bool?>, INavigatedToNotifiable
    {
        bool? _currentListingOption = true;

        #region ListingOption Property Members

        private static readonly DependencyPropertyKey ListingOptionPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ListingOption), typeof(ThreeStateViewModel), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ListingOption"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ListingOptionProperty = ListingOptionPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the view model for the listing view options.
        /// </summary>
        /// <value>The view model that indicates what items to load into the <see cref="Items"/> collection.</value>
        public ThreeStateViewModel ListingOption => (ThreeStateViewModel)GetValue(ListingOptionProperty);

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
                    (options.Value ? FsInfoCat.Properties.Resources.DisplayName_FileSystemDefinitions_IsActive :
                    FsInfoCat.Properties.Resources.DisplayName_FileSystemDefinitions_IsInactive) :
                    FsInfoCat.Properties.Resources.DisplayName_FileSystemDefinitions_All;

        #endregion

        public ListingViewModel()
        {
            SetValue(ListingOptionPropertyKey, new ThreeStateViewModel(_currentListingOption));
            UpdatePageTitle(_currentListingOption);
        }

        protected override IAsyncJob ReloadAsync(bool? options)
        {
            UpdatePageTitle(options);
            return base.ReloadAsync(options);
        }

        void INavigatedToNotifiable.OnNavigatedTo() => ReloadAsync(_currentListingOption);

        protected override IQueryable<FileSystemListItem> GetQueryableListing(bool? options, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            statusListener.SetMessage("Reading file system definitions from database");
            return options.HasValue ? (options.Value ? dbContext.FileSystemListing.Where(f => !f.IsInactive) :
                dbContext.FileSystemListing.Where(f => f.IsInactive)) : dbContext.FileSystemListing;
        }

        protected override ListItemViewModel CreateItemViewModel([DisallowNull] FileSystemListItem entity) => new(entity);

        protected override void OnApplyFilterOptionsCommand(object parameter)
        {
            if (_currentListingOption.HasValue ? (!ListingOption.Value.HasValue || _currentListingOption.Value != ListingOption.Value.Value) : ListingOption.Value.HasValue)
                _ = ReloadAsync(ListingOption.Value);
        }

        protected override void OnCancelFilterOptionsCommand(object parameter)
        {
            UpdatePageTitle(_currentListingOption);
            ListingOption.Value = _currentListingOption;
            base.OnCancelFilterOptionsCommand(parameter);
        }

        protected override void OnRefreshCommand(object parameter) => ReloadAsync(_currentListingOption);

        protected override void OnItemEditCommand([DisallowNull] ListItemViewModel item, object parameter)
        {
            //EditViewModel.ShowEditWindow(item.Entity);
            // TODO: Implement OnItemEditCommand(object);
        }

        protected override bool ConfirmItemDelete(ListItemViewModel item, object parameter) => MessageBox.Show(Application.Current.MainWindow,
            "This action cannot be undone!\n\nAre you sure you want to remove this file system definition from the database?",
            "Delete File System Definition", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes;

        protected override async Task<int> DeleteEntityFromDbContextAsync([DisallowNull] FileSystemListItem entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            FileSystem target = await dbContext.FileSystems.FindAsync(new object[] { entity.Id }, statusListener.CancellationToken);
            return (target is null) ? 0 : await FileSystem.DeleteAsync(target, dbContext, statusListener);
        }

        protected override void OnAddNewItemCommand(object parameter)
        {
            // TODO: Implement OnAddNewItemCommand(object);
        }

        protected override void OnReloadTaskCompleted(bool? options) => _currentListingOption = options;

        protected override void OnReloadTaskFaulted(Exception exception, bool? options)
        {
            UpdatePageTitle(_currentListingOption);
            ListingOption.Value = _currentListingOption;
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is AsyncOperationFailureException aExc) ? aExc.UserMessage.NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<AsyncOperationFailureException>().Select(e => e.UserMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while loading items from the databse.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override void OnReloadTaskCanceled(bool? options)
        {
            UpdatePageTitle(_currentListingOption);
            ListingOption.Value = _currentListingOption;
        }
    }
}
