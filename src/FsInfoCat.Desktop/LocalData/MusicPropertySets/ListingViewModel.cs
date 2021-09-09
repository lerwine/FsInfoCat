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

namespace FsInfoCat.Desktop.LocalData.MusicPropertySets
{
    public class ListingViewModel : ListingViewModel<MusicPropertiesListItem, ListItemViewModel, bool?, MusicPropertySet, ItemEditResult>, INavigatedToNotifiable
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
                    (options.Value ? FsInfoCat.Properties.Resources.DisplayName_MusicPropertyGroups_HasFiles :
                    FsInfoCat.Properties.Resources.DisplayName_MusicPropertyGroups_NoExistingFiles) :
                    FsInfoCat.Properties.Resources.DisplayName_MusicPropertyGroups_All;

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

        void INavigatedToNotifiable.OnNavigatedTo() => ReloadAsync(_currentOptions);

        protected override IQueryable<MusicPropertiesListItem> GetQueryableListing(bool? options, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
        {
            statusListener.SetMessage("Reading music proeprty sets from database");
            if (options.HasValue)
            {
                if (options.Value)
                    return dbContext.MusicPropertiesListing.Where(p => p.ExistingFileCount > 0L);
                return dbContext.MusicPropertiesListing.Where(p => p.ExistingFileCount == 0L);
            }
            return dbContext.MusicPropertiesListing;
        }

        protected override ListItemViewModel CreateItemViewModel([DisallowNull] MusicPropertiesListItem entity) => new(entity);

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

        private static async Task<FileSystem> LoadItemAsync([DisallowNull] FileSystemListItem item, [DisallowNull] IWindowsStatusListener statusListener)
        {
            using IServiceScope serviceScope = Services.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Guid id = item.Id;
            statusListener.SetMessage("Reading data");
            return await dbContext.FileSystems.Include(e => e.SymbolicNames).FirstOrDefaultAsync(e => e.Id == id, statusListener.CancellationToken);
        }

        protected override bool ConfirmItemDelete(ListItemViewModel item, object parameter) => MessageBox.Show(Application.Current.MainWindow,
            "This action cannot be undone!\n\nAre you sure you want to remove this music property set from the database?",
            "Delete Music Property Set", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes;

        protected override async Task<EntityEntry> DeleteEntityFromDbContextAsync([DisallowNull] MusicPropertiesListItem entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            MusicPropertySet target = await dbContext.MusicPropertySets.FindAsync(new object[] { entity.Id }, statusListener.CancellationToken);
            if (target is null)
                return null;
            EntityEntry entry = dbContext.MusicPropertySets.Remove(target);
            await dbContext.SaveChangesAsync(statusListener.CancellationToken);
            return entry;
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

        protected override bool EntityMatchesCurrentFilter(MusicPropertiesListItem entity)
        {
            throw new NotImplementedException();
        }

        protected override PageFunction<ItemEditResult> GetEditPage(MusicPropertySet args)
        {
            throw new NotImplementedException();
        }

        protected override Task<MusicPropertySet> LoadItemAsync([DisallowNull] MusicPropertiesListItem item, [DisallowNull] IWindowsStatusListener statusListener)
        {
            throw new NotImplementedException();
        }

        protected override void OnEditTaskFaulted(Exception exception, ListItemViewModel item)
        {
            throw new NotImplementedException();
        }

        protected override void OnDeleteTaskFaulted(Exception exception, ListItemViewModel item)
        {
            throw new NotImplementedException();
        }
    }
}
