using FsInfoCat.Activities;
using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;
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

namespace FsInfoCat.Desktop.LocalData.SharedTagDefinitions
{
    public class ListingViewModel : ListingViewModel<SharedTagDefinitionListItem, ListItemViewModel, bool?>, INavigatedToNotifiable
    {
        private bool? _currentOptions = true;
        private readonly ILogger<ListingViewModel> _logger;

        #region ListingOptions Property Members

        private static readonly DependencyPropertyKey ListingOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ListingOptions), typeof(ThreeStateViewModel), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ListingOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ListingOptionsProperty = ListingOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the view model for the listing view options.
        /// </summary>
        /// <value>The view model that indicates what items to load into the <see cref="Items"/> collection.</value>
        public ThreeStateViewModel ListingOptions => (ThreeStateViewModel)GetValue(ListingOptionsProperty);

        #endregion

        private void UpdatePageTitle(bool? options) => PageTitle = options.HasValue ?
                    (options.Value ? FsInfoCat.Properties.Resources.DisplayName_SharedTagDefinition_ActiveOnly :
                    FsInfoCat.Properties.Resources.DisplayName_SharedTagDefinition_InactiveOnly) :
                    FsInfoCat.Properties.Resources.DisplayName_SharedTagDefinition_All;

        public ListingViewModel()
        {
            _logger = App.GetLogger(this);
            SetValue(ListingOptionsPropertyKey, new ThreeStateViewModel(_currentOptions));
            UpdatePageTitle(_currentOptions);
        }

        protected override IAsyncAction<IActivityEvent> RefreshAsync(bool? options)
        {
            UpdatePageTitle(options);
            return base.RefreshAsync(options);
        }

        void INavigatedToNotifiable.OnNavigatedTo() => RefreshAsync(_currentOptions);

        protected override IQueryable<SharedTagDefinitionListItem> GetQueryableListing(bool? options, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IActivityProgress progress)
        {
            progress.Report("Reading shared tags from database");
            return options.HasValue ? (options.Value ? dbContext.SharedTagDefinitionListing.Where(f => !f.IsInactive) :
                dbContext.SharedTagDefinitionListing.Where(f => f.IsInactive)) : dbContext.SharedTagDefinitionListing;
        }

        protected override bool EntityMatchesCurrentFilter([DisallowNull] SharedTagDefinitionListItem entity) => !_currentOptions.HasValue || _currentOptions.Value != entity.IsInactive;

        protected override ListItemViewModel CreateItemViewModel([DisallowNull] SharedTagDefinitionListItem entity) => new(entity);

        protected override void OnApplyFilterOptionsCommand(object parameter)
        {
            if (_currentOptions.HasValue ? (!ListingOptions.Value.HasValue || _currentOptions.Value != ListingOptions.Value.Value) : ListingOptions.Value.HasValue)
                _ = RefreshAsync(ListingOptions.Value);
        }

        protected override void OnCancelFilterOptionsCommand(object parameter)
        {
            UpdatePageTitle(_currentOptions);
            ListingOptions.Value = _currentOptions;
            base.OnCancelFilterOptionsCommand(parameter);
        }

        protected override void OnRefreshCommand(object parameter) => RefreshAsync(_currentOptions);

        protected override bool ConfirmItemDelete(ListItemViewModel item, object parameter)
        {
            string message = item.FileTagCount switch
            {
                0 => item.SubdirectoryTagCount switch
                {
                    0 => item.VolumeTagCount switch
                    {
                        0 => $" ",
                        1 => $", including 1 volume tag, ",
                        _ => $", including all {item.VolumeTagCount} volume tags, ",
                    },
                    1 => item.VolumeTagCount switch
                    {
                        0 => $", including 1 sub-directory tag, ",
                        1 => $", including 1 sub-directory tag and 1 volume tag, ",
                        _ => $", including 1 sub-directory tag and all {item.VolumeTagCount} volume tags, ",
                    },
                    _ => item.VolumeTagCount switch
                    {
                        0 => $", including all {item.SubdirectoryTagCount} sub-directory tags, ",
                        1 => $", including all {item.SubdirectoryTagCount} sub-directory tags and 1 volume tag, ",
                        _ => $", including all {item.SubdirectoryTagCount} sub-directory and {item.VolumeTagCount} volume tags, ",
                    },
                },
                1 => item.SubdirectoryTagCount switch
                {
                    0 => item.VolumeTagCount switch
                    {
                        0 => $", including 1 file tag, ",
                        1 => $", including 1 file tag and 1 volume tag, ",
                        _ => $", including 1 file tag and all {item.VolumeTagCount} volume tags, ",
                    },
                    1 => item.VolumeTagCount switch
                    {
                        0 => $", including 1 file tag and 1 sub-directory tag, ",
                        1 => $", including 1 file tag, 1 sub-directory tag and 1 volume tag, ",
                        _ => $", including 1 file tag, 1 sub-directory tag and all {item.VolumeTagCount} volume tags, ",
                    },
                    _ => item.VolumeTagCount switch
                    {
                        0 => $", including 1 file tag and all {item.SubdirectoryTagCount} sub-directory tags, ",
                        1 => $", including 1 file tag and all {item.SubdirectoryTagCount} sub-directory tags and 1 volume tag, ",
                        _ => $", including 1 file tag and all {item.SubdirectoryTagCount} sub-directory and {item.VolumeTagCount} volume tags, ",
                    },
                },
                _ => item.SubdirectoryTagCount switch
                {
                    0 => item.VolumeTagCount switch
                    {
                        0 => $", including all {item.FileTagCount} file tags, ",
                        1 => $", including all {item.FileTagCount} file tags and 1 volume tag, ",
                        _ => $", including all {item.FileTagCount} file tags and {item.VolumeTagCount} volume tags, ",
                    },
                    1 => item.VolumeTagCount switch
                    {
                        0 => $", including all {item.FileTagCount} file tags and 1 sub-directory tag, ",
                        1 => $", including all {item.FileTagCount} file tags, 1 sub-directory tag and 1 volume tag, ",
                        _ => $", including all {item.FileTagCount} file tags, 1 sub-directory tag and {item.VolumeTagCount} volume tags, ",
                    },
                    _ => item.VolumeTagCount switch
                    {
                        0 => $", including all {item.FileTagCount} file and {item.SubdirectoryTagCount} sub-directory tags, ",
                        1 => $", including all {item.FileTagCount} file tags, {item.SubdirectoryTagCount} sub-directory tags and 1 volume tag, ",
                        _ => $", including all {item.FileTagCount} file tags, {item.SubdirectoryTagCount} sub-directory tags and {item.VolumeTagCount} volume tags, ",
                    },
                },
            };
            return MessageBox.Show(Application.Current.MainWindow, $"This action cannot be undone!\n\nAre you sure you want to remove this shared tag{message}from the database?",
                "Delete Shared Tag", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes;
        }

        protected override async Task<EntityEntry> DeleteEntityFromDbContextAsync([DisallowNull] SharedTagDefinitionListItem entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IActivityProgress progress)
        {
            SharedTagDefinition target = await dbContext.SharedTagDefinitions.FindAsync(new object[] { entity.Id }, progress.Token);
            if (target is null)
                return null;
            return await SharedTagDefinition.DeleteAsync(target, dbContext, progress, _logger);
        }

        protected override void OnReloadTaskCompleted(bool? options) => _currentOptions = options;

        protected override void OnReloadTaskFaulted([DisallowNull] Exception exception, bool? options)
        {
            UpdatePageTitle(_currentOptions);
            ListingOptions.Value = _currentOptions;
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is ActivityException aExc) ? aExc.ToString().NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<ActivityException>().Select(e => e.ToString())
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while loading items from the database.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override void OnReloadTaskCanceled(bool? options)
        {
            UpdatePageTitle(_currentOptions);
            ListingOptions.Value = _currentOptions;
        }

        protected async override Task<PageFunction<ItemFunctionResultEventArgs>> GetDetailPageAsync([DisallowNull] ListItemViewModel item, [DisallowNull] IActivityProgress progress)
        {
            if (item is null)
                return await Dispatcher.InvokeAsync<PageFunction<ItemFunctionResultEventArgs>>(() => new DetailsPage(new(new(), null)));
            using IServiceScope serviceScope = Hosting.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Guid id = item.Entity.Id;
            SharedTagDefinition fs = await dbContext.SharedTagDefinitions.FirstOrDefaultAsync(f => f.Id == id, progress.Token);
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
            SharedTagDefinition fs = await dbContext.SharedTagDefinitions.FirstOrDefaultAsync(f => f.Id == id, progress.Token);
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
            ListingOptions.Value = _currentOptions;
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
    }
}
