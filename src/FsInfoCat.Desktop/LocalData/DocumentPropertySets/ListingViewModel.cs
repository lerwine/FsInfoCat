using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace FsInfoCat.Desktop.LocalData.DocumentPropertySets
{
    public class ListingViewModel : ListingViewModel<DocumentPropertiesListItem, ListItemViewModel, bool?, ItemEditResult>, INavigatedToNotifiable
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

        public ListingViewModel()
        {
            SetValue(FilterOptionsPropertyKey, new ThreeStateViewModel(true));
            UpdatePageTitle(_currentOptions);
        }

        private void UpdatePageTitle(bool? options) => PageTitle = options.HasValue ?
                    (options.Value ? FsInfoCat.Properties.Resources.DisplayName_DocumentPropertyGroups_HasFiles :
                    FsInfoCat.Properties.Resources.DisplayName_DocumentPropertyGroups_NoExistingFiles) :
                    FsInfoCat.Properties.Resources.DisplayName_DocumentPropertyGroups_All;

        protected override IAsyncJob ReloadAsync(bool? options)
        {
            UpdatePageTitle(options);
            return base.ReloadAsync(options);
        }

        void INavigatedToNotifiable.OnNavigatedTo() => ReloadAsync(_currentOptions);

        protected override IQueryable<DocumentPropertiesListItem> GetQueryableListing(bool? options, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            statusListener.SetMessage("Reading document property sets from database");
            if (options.HasValue)
            {
                if (options.Value)
                    return dbContext.DocumentPropertiesListing.Where(p => p.ExistingFileCount > 0L);
                return dbContext.DocumentPropertiesListing.Where(p => p.ExistingFileCount == 0L);
            }
            return dbContext.DocumentPropertiesListing;
        }

        protected override ListItemViewModel CreateItemViewModel([DisallowNull] DocumentPropertiesListItem entity) => new(entity);

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

        protected override bool ConfirmItemDelete(ListItemViewModel item, object parameter) => MessageBox.Show(Application.Current.MainWindow,
            "This action cannot be undone!\n\nAre you sure you want to remove this document property set from the database?",
            "Delete Document Property Set", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes;

        protected override async Task<EntityEntry> DeleteEntityFromDbContextAsync([DisallowNull] DocumentPropertiesListItem entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            DocumentPropertySet target = await dbContext.DocumentPropertySets.FindAsync(new object[] { entity.Id }, statusListener.CancellationToken);
            if (target is null)
                return null;
            EntityEntry entry = dbContext.DocumentPropertySets.Remove(target);
            await dbContext.SaveChangesAsync(statusListener.CancellationToken);
            return entry;
        }

        protected override void OnReloadTaskCompleted(bool? options) => _currentOptions = options;

        protected override void OnReloadTaskFaulted([DisallowNull] Exception exception, bool? options)
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

        protected override bool EntityMatchesCurrentFilter([DisallowNull] DocumentPropertiesListItem entity) => !_currentOptions.HasValue || (_currentOptions.Value ? entity.ExistingFileCount > 0L : entity.ExistingFileCount == 0L);

        protected async override Task<PageFunction<ItemEditResult>> GetDetailPageAsync([DisallowNull] ListItemViewModel item, [DisallowNull] IWindowsStatusListener statusListener)
        {
            DetailsViewModel viewModel;
            if (item is null)
                viewModel = new(new DocumentPropertySet(), null);
            else
            {
                using IServiceScope serviceScope = Services.CreateScope();
                using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
                Guid id = item.Entity.Id;
                DocumentPropertySet fs = await dbContext.DocumentPropertySets.FirstOrDefaultAsync(f => f.Id == id, statusListener.CancellationToken);
                if (fs is null)
                {
                    await Dispatcher.ShowMessageBoxAsync("Item not found in database. Click OK to refresh listing.", "Security Exception", MessageBoxButton.OK, MessageBoxImage.Error, statusListener.CancellationToken);
                    ReloadAsync(_currentOptions);
                    return null;
                }
                viewModel = new(fs, item.Entity);
            }
            return new DetailsPage(viewModel);
        }

        protected override async Task<PageFunction<ItemEditResult>> GetEditPageAsync(ListItemViewModel item, [DisallowNull] IWindowsStatusListener statusListener)
        {
            EditViewModel viewModel;
            if (item is null)
                viewModel = new(new DocumentPropertySet(), null);
            else
            {
                using IServiceScope serviceScope = Services.CreateScope();
                using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
                Guid id = item.Entity.Id;
                DocumentPropertySet fs = await dbContext.DocumentPropertySets.FirstOrDefaultAsync(f => f.Id == id, statusListener.CancellationToken);
                if (fs is null)
                {
                    await Dispatcher.ShowMessageBoxAsync("Item not found in database. Click OK to refresh listing.", "Security Exception", MessageBoxButton.OK, MessageBoxImage.Error, statusListener.CancellationToken);
                    ReloadAsync(_currentOptions);
                    return null;
                }
                viewModel = new EditViewModel(fs, item.Entity);
            }
            return new EditPage(viewModel);
        }

        //protected override async Task<DocumentPropertySet> LoadItemAsync([DisallowNull] DocumentPropertiesListItem item, [DisallowNull] IWindowsStatusListener statusListener)
        //{
        //    using IServiceScope serviceScope = Services.CreateScope();
        //    using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
        //    Guid id = item.Id;
        //    statusListener.SetMessage("Reading data");
        //    return await dbContext.DocumentPropertySets.Include(e => e.Files).FirstOrDefaultAsync(e => e.Id == id, statusListener.CancellationToken);
        //}

        protected override void OnEditTaskFaulted([DisallowNull] Exception exception, ListItemViewModel item)
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

        protected override void OnDeleteTaskFaulted([DisallowNull] Exception exception, [DisallowNull] ListItemViewModel item)
        {
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is AsyncOperationFailureException aExc) ? aExc.UserMessage.NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<AsyncOperationFailureException>().Select(e => e.UserMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while deleting the item from the databse.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
