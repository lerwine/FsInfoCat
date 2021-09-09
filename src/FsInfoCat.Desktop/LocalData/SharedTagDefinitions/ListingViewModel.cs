using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace FsInfoCat.Desktop.LocalData.SharedTagDefinitions
{
    public class ListingViewModel : ListingViewModel<SharedTagDefinitionListItem, ListItemViewModel, bool?, SharedTagDefinition, ItemEditResult>, INavigatedToNotifiable
    {
        private bool? _currentOptions = true;

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
                    FsInfoCat.Properties.Resources.DisplayName_SharedTagDefinition_InactiveOnlly) :
                    FsInfoCat.Properties.Resources.DisplayName_SharedTagDefinition_All;

        public ListingViewModel()
        {
            SetValue(ListingOptionsPropertyKey, new ThreeStateViewModel(_currentOptions));
            UpdatePageTitle(_currentOptions);
        }

        protected override IAsyncJob ReloadAsync(bool? options)
        {
            UpdatePageTitle(options);
            return base.ReloadAsync(options);
        }

        void INavigatedToNotifiable.OnNavigatedTo() => ReloadAsync(_currentOptions);

        protected override IQueryable<SharedTagDefinitionListItem> GetQueryableListing(bool? options, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            statusListener.SetMessage("Reading shared tags from database");
            return options.HasValue ? (options.Value ? dbContext.SharedTagDefinitionListing.Where(f => !f.IsInactive) :
                dbContext.SharedTagDefinitionListing.Where(f => f.IsInactive)) : dbContext.SharedTagDefinitionListing;
        }

        protected override ListItemViewModel CreateItemViewModel([DisallowNull] SharedTagDefinitionListItem entity) => new(entity);

        protected override void OnApplyFilterOptionsCommand(object parameter)
        {
            if (_currentOptions.HasValue ? (!ListingOptions.Value.HasValue || _currentOptions.Value != ListingOptions.Value.Value) : ListingOptions.Value.HasValue)
                _ = ReloadAsync(ListingOptions.Value);
        }

        protected override void OnCancelFilterOptionsCommand(object parameter)
        {
            UpdatePageTitle(_currentOptions);
            ListingOptions.Value = _currentOptions;
            base.OnCancelFilterOptionsCommand(parameter);
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

        private void OnEditItemComplete(object sender, ReturnEventArgs<ItemEditResult> e)
        {
            switch (e.Result.Result)
            {
                case Microsoft.EntityFrameworkCore.EntityState.Added:
                    // TODO: Add to current list if it matches filter.
                    break;
                case Microsoft.EntityFrameworkCore.EntityState.Deleted:
                    RemoveItem(Items.FirstOrDefault(i => ReferenceEquals(i.Entity, e.Result.ItemEntity)));
                    break;
            }
        }

        private void OnItemAdded(object sender, ReturnEventArgs<FileSystem> e)
        {
            if (e.Result is not null)
                throw new NotImplementedException();
            // TODO: Add to current list if it matches filter.
        }

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
            return MessageBox.Show(Application.Current.MainWindow, "This action cannot be undone!\n\nAre you sure you want to remove this shared tag{message}from the database?",
                "Delete Shared Tag", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes;
        }

        protected override async Task<int> DeleteEntityFromDbContextAsync([DisallowNull] SharedTagDefinitionListItem entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            SharedTagDefinition target = await dbContext.SharedTagDefinitions.FindAsync(new object[] { entity.Id }, statusListener.CancellationToken);
            return (target is null) ? 0 : await SharedTagDefinition.DeleteAsync(target, dbContext, statusListener);
        }

        protected override void OnReloadTaskCompleted(bool? options) => _currentOptions = options;

        protected override void OnReloadTaskFaulted(Exception exception, bool? options)
        {
            UpdatePageTitle(_currentOptions);
            ListingOptions.Value = _currentOptions;
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
            ListingOptions.Value = _currentOptions;
        }
    }
}
