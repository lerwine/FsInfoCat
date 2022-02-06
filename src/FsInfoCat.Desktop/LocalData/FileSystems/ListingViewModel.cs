using FsInfoCat.Activities;
using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace FsInfoCat.Desktop.LocalData.FileSystems
{

    public class ListingViewModel : ListingViewModel<FileSystemListItem, ListItemViewModel, bool?>, INavigatedToNotifiable
    {
        bool? _currentListingOption = true;
        private readonly ILogger<ListingViewModel> _logger;

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
            _logger = App.GetLogger(this);
            SetValue(ListingOptionPropertyKey, new ThreeStateViewModel(_currentListingOption));
            UpdatePageTitle(_currentListingOption);
        }

        private void UpdatePageTitle(bool? options) => PageTitle = options.HasValue ?
                    (options.Value ? FsInfoCat.Properties.Resources.DisplayName_FileSystemDefinitions_IsActive :
                    FsInfoCat.Properties.Resources.DisplayName_FileSystemDefinitions_IsInactive) :
                    FsInfoCat.Properties.Resources.DisplayName_FileSystemDefinitions_All;

        protected override IAsyncAction<IActivityEvent> RefreshAsync(bool? options)
        {
            UpdatePageTitle(options);
            return base.RefreshAsync(options);
        }

        void INavigatedToNotifiable.OnNavigatedTo() => RefreshAsync(_currentListingOption);

        protected override IQueryable<FileSystemListItem> GetQueryableListing(bool? options, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IActivityProgress progress)
        {
            progress.Report("Reading file system definitions from database");
            return options.HasValue ? (options.Value ? dbContext.FileSystemListing.Where(f => !f.IsInactive) :
                dbContext.FileSystemListing.Where(f => f.IsInactive)) : dbContext.FileSystemListing;
        }

        protected override ListItemViewModel CreateItemViewModel([DisallowNull] FileSystemListItem entity) => new(entity);

        protected override void OnApplyFilterOptionsCommand(object parameter)
        {
            if (_currentListingOption.HasValue ? (!ListingOption.Value.HasValue || _currentListingOption.Value != ListingOption.Value.Value) : ListingOption.Value.HasValue)
                _ = RefreshAsync(ListingOption.Value);
        }

        protected override void OnCancelFilterOptionsCommand(object parameter)
        {
            UpdatePageTitle(_currentListingOption);
            ListingOption.Value = _currentListingOption;
            base.OnCancelFilterOptionsCommand(parameter);
        }

        protected override void OnRefreshCommand(object parameter) => RefreshAsync(_currentListingOption);

        protected override bool ConfirmItemDelete(ListItemViewModel item, object parameter) => MessageBox.Show(Application.Current.MainWindow,
            "This action cannot be undone!\n\nAre you sure you want to remove this file system definition from the database?",
            "Delete File System Definition", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes;

        protected override async Task<EntityEntry> DeleteEntityFromDbContextAsync([DisallowNull] FileSystemListItem entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IActivityProgress progress)
        {
            FileSystem target = await dbContext.FileSystems.FindAsync(new object[] { entity.Id }, progress.Token);
            if (target is null)
                return null;
            await FileSystem.DeleteAsync(target, dbContext, progress, _logger);
            return dbContext.Entry(target);
        }

        protected override void OnReloadTaskCompleted(bool? options) => _currentListingOption = options;

        protected override void OnReloadTaskFaulted([DisallowNull] Exception exception, bool? options)
        {
            UpdatePageTitle(_currentListingOption);
            ListingOption.Value = _currentListingOption;
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is ActivityException aExc) ? aExc.ToString().NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<ActivityException>().Select(e => e.ToString())
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while loading items from the database.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override void OnReloadTaskCanceled(bool? options)
        {
            UpdatePageTitle(_currentListingOption);
            ListingOption.Value = _currentListingOption;
        }

        protected override bool EntityMatchesCurrentFilter([DisallowNull] FileSystemListItem entity) => !_currentListingOption.HasValue || _currentListingOption.Value != entity.IsInactive;

        protected async override Task<PageFunction<ItemFunctionResultEventArgs>> GetDetailPageAsync([DisallowNull] ListItemViewModel item, [DisallowNull] IActivityProgress progress)
        {
            if (item is null)
                return await Dispatcher.InvokeAsync<PageFunction<ItemFunctionResultEventArgs>>(() => new DetailsPage(new(new(), null)));
            using IServiceScope serviceScope = Hosting.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Guid id = item.Entity.Id;
            FileSystem fs = await dbContext.FileSystems.FirstOrDefaultAsync(f => f.Id == id, progress.Token);
            if (fs is null)
            {
                await Dispatcher.ShowMessageBoxAsync("Item not found in database. Click OK to refresh listing.", "Security Exception", MessageBoxButton.OK, MessageBoxImage.Error, progress.Token);
                RefreshAsync(_currentListingOption);
                return null;
            }
            return await Dispatcher.InvokeAsync<PageFunction<ItemFunctionResultEventArgs>>(() => new DetailsPage(new(fs, item.Entity)));
        }

        protected async override Task<PageFunction<ItemFunctionResultEventArgs>> GetEditPageAsync(ListItemViewModel item, [DisallowNull] IActivityProgress progress)
        {
            if (item is null)
                return await Dispatcher.InvokeAsync<PageFunction<ItemFunctionResultEventArgs>>(() => new EditPage(new(new(), null)));
            using IServiceScope serviceScope = Hosting.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Guid id = item.Entity.Id;
            FileSystem fs = await dbContext.FileSystems.FirstOrDefaultAsync(f => f.Id == id, progress.Token);
            if (fs is null)
            {
                await Dispatcher.ShowMessageBoxAsync("Item not found in database. Click OK to refresh listing.", "Security Exception", MessageBoxButton.OK, MessageBoxImage.Error, progress.Token);
                RefreshAsync(_currentListingOption);
                return null;
            }
            return await Dispatcher.InvokeAsync<PageFunction<ItemFunctionResultEventArgs>>(() => new EditPage(new(fs, item.Entity)));
        }

        protected override void OnDeleteTaskFaulted([DisallowNull] Exception exception, [DisallowNull] ListItemViewModel item)
        {
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is ActivityException aExc) ? aExc.ToString().NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<ActivityException>().Select(e => e.ToString())
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while deleting the item from the database.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override void OnEditTaskFaulted([DisallowNull] Exception exception, ListItemViewModel item)
        {
            UpdatePageTitle(_currentListingOption);
            ListingOption.Value = _currentListingOption;
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is ActivityException aExc) ? aExc.ToString().NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<ActivityException>().Select(e => e.ToString())
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while loading items from the database.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
