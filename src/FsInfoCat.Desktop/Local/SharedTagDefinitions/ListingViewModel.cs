using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.Local.SharedTagDefinitions
{
    public class ListingViewModel : ListingViewModel<SharedTagDefinitionListItem, ListItemViewModel, bool?>, INotifyNavigatedTo
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

        public ListingViewModel()
        {
            SetValue(ListingOptionsPropertyKey, new ThreeStateViewModel(_currentOptions));
        }

        void INotifyNavigatedTo.OnNavigatedTo() => ReloadAsync(_currentOptions);

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
            if (_currentOptions.HasValue ? (ListingOptions.Value.HasValue && _currentOptions.Value == ListingOptions.Value.Value) : !ListingOptions.Value.HasValue)
                return;
            _currentOptions = ListingOptions.Value;
            ReloadAsync(_currentOptions);
        }

        protected override void OnCancelFilterOptionsCommand(object parameter)
        {
            ListingOptions.Value = _currentOptions;
            base.OnCancelFilterOptionsCommand(parameter);
        }

        protected override void OnRefreshCommand(object parameter) => ReloadAsync(_currentOptions);

        protected override void OnItemEditCommand([DisallowNull] ListItemViewModel item, object parameter)
        {
            // TODO: Implement OnItemEditCommand(object);
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
            return MessageBox.Show(App.Current.MainWindow, "This action cannot be undone!\n\nAre you sure you want to remove this shared tag{message}from the database?",
                "Delete Shared Tag", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes;
        }

        protected override async Task<int> DeleteEntityFromDbContextAsync([DisallowNull] SharedTagDefinitionListItem entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            SharedTagDefinition target = await dbContext.SharedTagDefinitions.FindAsync(new object[] { entity.Id }, statusListener.CancellationToken);
            return (target is null) ? 0 : await SharedTagDefinition.DeleteAsync(target, dbContext, statusListener);
        }

        protected override void OnAddNewItemCommand(object parameter)
        {
            // TODO: Implement OnAddNewItemCommand(object);
        }
    }
}
