using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.LocalVM.FileSystems
{
    public class ListingViewModel : ListingViewModel<FileSystemListItem, ListItemViewModel, bool?>, INotifyNavigatedTo
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

        public ListingViewModel()
        {
            SetValue(ListingOptionPropertyKey, new ThreeStateViewModel(_currentListingOption));
        }

        void INotifyNavigatedTo.OnNavigatedTo()
        {
            IAsyncJob asyncJob = ReloadAsync(_currentListingOption);
            _ = asyncJob.Task.ContinueWith(task => OnReloadComplete(task, asyncJob));
        }

        private void OnReloadComplete(Task task, IAsyncJob asyncJob)
        {
            if (task.IsCanceled || !task.IsFaulted)
                return;
            string userMessage = task.Exception.InnerExceptions.OfType<AsyncOperationFailureException>().Select(e => e.UserMessage)
                .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault();
            if (userMessage is null)
                userMessage = "An unexpected error has occurred. See logs for details.";
            _ = MessageBox.Show(Application.Current.MainWindow, userMessage, asyncJob.Title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

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
            if (_currentListingOption.HasValue)
            {
                if (ListingOption.Value.HasValue && _currentListingOption.Value == ListingOption.Value)
                    return;
            }
            else if (!ListingOption.Value.HasValue)
                return;
            _currentListingOption = ListingOption.Value;
            IAsyncJob asyncJob = ReloadAsync(_currentListingOption);
            _ = asyncJob.Task.ContinueWith(task => OnReloadComplete(task, asyncJob));
        }

        protected override void OnCancelFilterOptionsCommand(object parameter)
        {
            ListingOption.Value = _currentListingOption;
            base.OnCancelFilterOptionsCommand(parameter);
        }

        protected override void OnRefreshCommand(object parameter) => ReloadAsync(_currentListingOption);

        protected override void OnItemEditCommand([DisallowNull] ListItemViewModel item, object parameter)
        {
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
    }
}