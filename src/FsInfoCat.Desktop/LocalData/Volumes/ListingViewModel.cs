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

namespace FsInfoCat.Desktop.LocalData.Volumes
{
    public class ListingViewModel : ListingViewModel<VolumeListItemWithFileSystem, ListItemViewModel, ListingViewModel.ListingOptions>, INavigatedToNotifiable
    {
        private readonly ILogger<ListingViewModel> _logger;

        #region StatusFilterOption Property Members

        private ListingOptions _currentOptions = new(null, true);
        private readonly EnumChoiceItem<VolumeStatus> _allOption;
        private readonly EnumChoiceItem<VolumeStatus> _inactiveOption;
        private readonly EnumChoiceItem<VolumeStatus> _activeOption;
        private static readonly DependencyPropertyKey StatusFilterOptionPropertyKey = DependencyProperty.RegisterReadOnly(nameof(StatusFilterOption), typeof(EnumValuePickerVM<VolumeStatus>), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="StatusFilterOption"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusFilterOptionProperty = StatusFilterOptionPropertyKey.DependencyProperty;

        public EnumValuePickerVM<VolumeStatus> StatusFilterOption => (EnumValuePickerVM<VolumeStatus>)GetValue(StatusFilterOptionProperty);

        #endregion

        public ListingViewModel()
        {
            _logger = App.GetLogger(this);
            EnumValuePickerVM<VolumeStatus> viewOptions = new(FsInfoCat.Properties.Resources.DisplayName_AllItems, FsInfoCat.Properties.Resources.DisplayName_ActiveItems,
                FsInfoCat.Properties.Resources.DisplayName_InactiveItems);
            _allOption = viewOptions.Choices.First(o => o.DisplayName == FsInfoCat.Properties.Resources.DisplayName_AllItems);
            _inactiveOption = viewOptions.Choices.First(o => o.DisplayName == FsInfoCat.Properties.Resources.DisplayName_InactiveItems);
            _activeOption = viewOptions.Choices.First(o => o.DisplayName == FsInfoCat.Properties.Resources.DisplayName_ActiveItems);
            viewOptions.SelectedItem = FromListingOptions(_currentOptions);
            SetValue(StatusFilterOptionPropertyKey, viewOptions);
            UpdatePageTitle(_currentOptions);
        }

        private void UpdatePageTitle(ListingOptions options)
        {
            if (options.Status.HasValue)
                PageTitle = string.Format(FsInfoCat.Properties.Resources.FormatDisplayName_Volumes_Status, options.Status.Value.GetDisplayName());
            else
                PageTitle = options.ShowActiveOnly.HasValue ?
                    (options.ShowActiveOnly.Value ? FsInfoCat.Properties.Resources.DisplayName_Volumes_ActiveOnly :
                    FsInfoCat.Properties.Resources.DisplayName_Volumes_InactiveOnly) :
                    FsInfoCat.Properties.Resources.DisplayName_Volumes_All;
        }

        private ListingOptions ToListingOptions(EnumChoiceItem<VolumeStatus> item)
        {
            if (item is not null)
            {
                VolumeStatus? status = item.Value;
                if (status.HasValue)
                    return new(status, null);
                if (ReferenceEquals(_allOption, item))
                    return new(null, null);
                if (ReferenceEquals(_inactiveOption, item))
                    return new(null, false);
            }
            return new(null, true);
        }

        private EnumChoiceItem<VolumeStatus> FromListingOptions(ListingOptions options) => options.Status.HasValue
                ? StatusFilterOption.Choices.FirstOrDefault(o => o.Value == options.Status)
                : options.ShowActiveOnly.HasValue ? options.ShowActiveOnly.Value ? _activeOption : _inactiveOption : _allOption;

        protected override IAsyncAction<IActivityEvent> RefreshAsync(ListingOptions options)
        {
            UpdatePageTitle(options);
            return base.RefreshAsync(options);
        }

        void INavigatedToNotifiable.OnNavigatedTo() => RefreshAsync(_currentOptions);

        protected override bool EntityMatchesCurrentFilter([DisallowNull] VolumeListItemWithFileSystem entity) => _currentOptions.Status.HasValue ? entity.Status == _currentOptions.Status.Value : (!_currentOptions.ShowActiveOnly.HasValue || entity.Status switch
        {
            VolumeStatus.Controlled or VolumeStatus.AccessError or VolumeStatus.Offline => _currentOptions.ShowActiveOnly.Value,
            _ => !_currentOptions.ShowActiveOnly.Value,
        });

        protected override IQueryable<VolumeListItemWithFileSystem> GetQueryableListing(ListingOptions options, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IActivityProgress progress)
        {
            progress.Report("Reading volume information records from database");
            if (options.Status.HasValue)
            {
                VolumeStatus s = options.Status.Value;
                return dbContext.VolumeListingWithFileSystem.Where(v => v.Status == s);
            }
            if (options.ShowActiveOnly.HasValue)
            {
                if (options.ShowActiveOnly.Value)
                    return dbContext.VolumeListingWithFileSystem.Where(v => v.Status == VolumeStatus.Controlled || v.Status == VolumeStatus.AccessError || v.Status == VolumeStatus.Offline);
                return dbContext.VolumeListingWithFileSystem.Where(v => v.Status != VolumeStatus.Controlled && v.Status != VolumeStatus.AccessError && v.Status != VolumeStatus.Offline);
            }
            return dbContext.VolumeListingWithFileSystem;
        }

        protected override ListItemViewModel CreateItemViewModel([DisallowNull] VolumeListItemWithFileSystem entity) => new(entity);

        protected override void OnApplyFilterOptionsCommand(object parameter)
        {
            ListingOptions newOptions = ToListingOptions(StatusFilterOption.SelectedItem);
            if (newOptions.Status.HasValue ? _currentOptions.Status != newOptions.Status : (_currentOptions.Status.HasValue || newOptions.ShowActiveOnly != _currentOptions.ShowActiveOnly))
                _ = RefreshAsync(newOptions);
        }

        protected override void OnCancelFilterOptionsCommand(object parameter)
        {
            UpdatePageTitle(_currentOptions);
            StatusFilterOption.SelectedItem = FromListingOptions(_currentOptions);
            base.OnCancelFilterOptionsCommand(parameter);
        }
        protected override void OnRefreshCommand(object parameter) => RefreshAsync(_currentOptions);

        protected override bool ConfirmItemDelete(ListItemViewModel item, object parameter) => MessageBox.Show(Application.Current.MainWindow,
            "This action cannot be undone!\n\nAre you sure you want to remove this volume record from the database?",
            "Delete Volume Record", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes;

        protected override async Task<EntityEntry> DeleteEntityFromDbContextAsync([DisallowNull] VolumeListItemWithFileSystem entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IActivityProgress progress)
        {
            Volume target = await dbContext.Volumes.FindAsync(new object[] { entity.Id }, progress.Token);
            EntityEntry<Volume> entry = dbContext.Entry(target);
            if (target is null)
                return null;
            await Volume.DeleteAsync(target, dbContext, progress, _logger);
            return entry;
        }

        protected override void OnReloadTaskCompleted(ListingOptions options) => _currentOptions = options;

        protected override void OnReloadTaskFaulted([DisallowNull] Exception exception, ListingOptions options)
        {
            UpdatePageTitle(_currentOptions);
            StatusFilterOption.SelectedItem = FromListingOptions(_currentOptions);
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is ActivityException aExc) ? aExc.ToString().NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<ActivityException>().Select(e => e.ToString())
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while loading items from the database.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override void OnReloadTaskCanceled(ListingOptions options)
        {
            UpdatePageTitle(_currentOptions);
            StatusFilterOption.SelectedItem = FromListingOptions(_currentOptions);
        }

        protected async override Task<PageFunction<ItemFunctionResultEventArgs>> GetDetailPageAsync([DisallowNull] ListItemViewModel item, [DisallowNull] IActivityProgress progress)
        {
            if (item is null)
                return await Dispatcher.InvokeAsync<PageFunction<ItemFunctionResultEventArgs>>(() => new DetailsPage(new(new(), null)));
            using IServiceScope serviceScope = Hosting.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Guid id = item.Entity.Id;
            Volume fs = await dbContext.Volumes.FirstOrDefaultAsync(f => f.Id == id, progress.Token);
            if (fs is null)
            {
                await Dispatcher.ShowMessageBoxAsync("Item not found in database. Click OK to refresh listing.", "Security Exception", MessageBoxButton.OK, MessageBoxImage.Error, progress.Token);
                RefreshAsync(_currentOptions);
                return null;
            }
            return await Dispatcher.InvokeAsync<PageFunction<ItemFunctionResultEventArgs>>(() => new DetailsPage(new(fs, item.Entity)));
        }

        protected override async Task<PageFunction<ItemFunctionResultEventArgs>> GetEditPageAsync(ListItemViewModel item, [DisallowNull] IActivityProgress progress)
        {
            if (item is null)
                return await Dispatcher.InvokeAsync<PageFunction<ItemFunctionResultEventArgs>>(() => new EditPage(new(new(), null)));
            using IServiceScope serviceScope = Hosting.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Guid id = item.Entity.Id;
            Volume fs = await dbContext.Volumes.FirstOrDefaultAsync(f => f.Id == id, progress.Token);
            if (fs is null)
            {
                await Dispatcher.ShowMessageBoxAsync("Item not found in database. Click OK to refresh listing.", "Security Exception", MessageBoxButton.OK, MessageBoxImage.Error, progress.Token);
                RefreshAsync(_currentOptions);
                return null;
            }
            return await Dispatcher.InvokeAsync<PageFunction<ItemFunctionResultEventArgs>>(() => new EditPage(new(fs, item.Entity)));
        }

        protected override void OnEditTaskFaulted([DisallowNull] Exception exception, ListItemViewModel item)
        {
            UpdatePageTitle(_currentOptions);
            StatusFilterOption.SelectedItem = FromListingOptions(_currentOptions);
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is ActivityException aExc) ? aExc.ToString().NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<ActivityException>().Select(e => e.ToString())
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while loading items from the database.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        public record ListingOptions(VolumeStatus? Status, bool? ShowActiveOnly);
    }
}
