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

namespace FsInfoCat.Desktop.LocalData.AudioPropertySets
{
    public class ListingViewModel : ListingViewModel<AudioPropertiesListItem, ListItemViewModel, bool?, ItemEditResult>, INavigatedToNotifiable
    {
        private bool? _currentOptions = true;

        #region FilterOptions Property Members

        private static readonly DependencyPropertyKey FilterOptionsPropertyKey = DependencyPropertyBuilder<ListingViewModel, ThreeStateViewModel>
            .Register(nameof(FilterOptions))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="FilterOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FilterOptionsProperty = FilterOptionsPropertyKey.DependencyProperty;

        public ThreeStateViewModel FilterOptions { get => (ThreeStateViewModel)GetValue(FilterOptionsProperty); private set => SetValue(FilterOptionsPropertyKey, value); }

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

        private void UpdatePageTitle(bool? options) => PageTitle = options.HasValue ?
                    (options.Value ? FsInfoCat.Properties.Resources.DisplayName_AudioPropertyGroups_HasFiles :
                    FsInfoCat.Properties.Resources.DisplayName_AudioPropertyGroups_NoExistingFiles) :
                    FsInfoCat.Properties.Resources.DisplayName_AudioPropertyGroups_All;

        void INavigatedToNotifiable.OnNavigatedTo() => ReloadAsync(_currentOptions);

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

        protected override bool EntityMatchesCurrentFilter([DisallowNull] AudioPropertiesListItem entity) => !_currentOptions.HasValue || (_currentOptions.Value ? entity.ExistingFileCount > 0L : entity.ExistingFileCount == 0L);

        protected override ListItemViewModel CreateItemViewModel([DisallowNull] AudioPropertiesListItem entity)
        {
            ListItemViewModel listItemViewModel = new(entity);
            CommonAttached.SetListItemTitle(listItemViewModel, listItemViewModel.CalculateSummaryText());
            return listItemViewModel;
        }

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
            "This action cannot be undone!\n\nAre you sure you want to remove this audio property set from the database?",
            "Delete Audio Property Set", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes;

        protected override async Task<EntityEntry> DeleteEntityFromDbContextAsync([DisallowNull] AudioPropertiesListItem entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            AudioPropertySet target = await dbContext.AudioPropertySets.FindAsync(new object[] { entity.Id }, statusListener.CancellationToken);
            if (target is null)
                return null;
            EntityEntry entry = dbContext.AudioPropertySets.Remove(target);
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

        protected async override Task<PageFunction<ItemEditResult>> GetDetailPageAsync([DisallowNull] ListItemViewModel item, [DisallowNull] IWindowsStatusListener statusListener)
        {
            if (item is null)
                return await Dispatcher.InvokeAsync<PageFunction<ItemEditResult>>(() => new DetailsPage(new(new AudioPropertySet(), null)));
            using IServiceScope serviceScope = Services.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Guid id = item.Entity.Id;
            AudioPropertySet fs = await dbContext.AudioPropertySets.FirstOrDefaultAsync(f => f.Id == id, statusListener.CancellationToken);
            if (fs is null)
            {
                await Dispatcher.ShowMessageBoxAsync("Item not found in database. Click OK to refresh listing.", "Security Exception", MessageBoxButton.OK, MessageBoxImage.Error, statusListener.CancellationToken);
                ReloadAsync(_currentOptions);
                return null;
            }
            return await Dispatcher.InvokeAsync<PageFunction<ItemEditResult>>(() => new DetailsPage(new(fs, item.Entity)));
        }

        protected override async Task<PageFunction<ItemEditResult>> GetEditPageAsync(ListItemViewModel item, [DisallowNull] IWindowsStatusListener statusListener)
        {
            if (item is null)
                return await Dispatcher.InvokeAsync<PageFunction<ItemEditResult>>(() => new EditPage(new(new AudioPropertySet(), null)));
            using IServiceScope serviceScope = Services.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Guid id = item.Entity.Id;
            AudioPropertySet fs = await dbContext.AudioPropertySets.FirstOrDefaultAsync(f => f.Id == id, statusListener.CancellationToken);
            if (fs is null)
            {
                await Dispatcher.ShowMessageBoxAsync("Item not found in database. Click OK to refresh listing.", "Security Exception", MessageBoxButton.OK, MessageBoxImage.Error, statusListener.CancellationToken);
                ReloadAsync(_currentOptions);
                return null;
            }
            return await Dispatcher.InvokeAsync<PageFunction<ItemEditResult>>(() => new EditPage(new(fs, item.Entity)));
        }

        //protected async override Task<AudioPropertySet> LoadItemAsync([DisallowNull] AudioPropertiesListItem item, [DisallowNull] IWindowsStatusListener statusListener)
        //{
        //    using IServiceScope serviceScope = Services.CreateScope();
        //    using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
        //    Guid id = item.Id;
        //    statusListener.SetMessage("Reading data");
        //    return await dbContext.AudioPropertySets.Include(e => e.Files).FirstOrDefaultAsync(e => e.Id == id, statusListener.CancellationToken);
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