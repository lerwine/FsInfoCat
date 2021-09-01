using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.Local.FileSystems
{
    public class ListingViewModel : ListingViewModel<FileSystemListItem, ListItemViewModel, bool?>, INotifyNavigatedTo
    {
        #region ViewOptions Property Members

        private static readonly DependencyPropertyKey ViewOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewOptions), typeof(ThreeStateViewModel), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewOptionsProperty = ViewOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the view model for the listing view options.
        /// </summary>
        /// <value>The view model that indicates what items to load into the <see cref="Items"/> collection.</value>
        public ThreeStateViewModel ViewOptions => (ThreeStateViewModel)GetValue(ViewOptionsProperty);

        #endregion
        #region EditingOptions Property Members

        private static readonly DependencyPropertyKey EditingOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EditingOptions), typeof(ThreeStateViewModel), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="EditingOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EditingOptionsProperty = EditingOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public ThreeStateViewModel EditingOptions => (ThreeStateViewModel)GetValue(EditingOptionsProperty);

        #endregion

        public ListingViewModel()
        {
            ThreeStateViewModel viewOptions = new(true);
            SetValue(ViewOptionsPropertyKey, viewOptions);
            viewOptions.ValuePropertyChanged += (sender, e) => ReloadAsync(e.NewValue as bool?);
            SetValue(EditingOptionsPropertyKey, new ThreeStateViewModel(viewOptions.Value));
        }

        void INotifyNavigatedTo.OnNavigatedTo() => ReloadAsync(ViewOptions.Value);

        protected override IQueryable<FileSystemListItem> GetQueryableListing(bool? options, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            statusListener.SetMessage("Reading file system definitions from database");
            return options.HasValue ? (options.Value ? dbContext.FileSystemListing.Where(f => !f.IsInactive) :
                dbContext.FileSystemListing.Where(f => f.IsInactive)) : dbContext.FileSystemListing;
        }

        protected override ListItemViewModel CreateItemViewModel([DisallowNull] FileSystemListItem entity) => new ListItemViewModel(entity);

        protected override void OnApplyFilterOptionsCommand(object parameter)
        {
            ViewOptions.Value = EditingOptions.Value;
        }

        protected override void OnCancelFilterOptionsCommand(object parameter)
        {
            EditingOptions.Value = ViewOptions.Value;
            base.OnCancelFilterOptionsCommand(parameter);
        }

        protected override void OnRefreshCommand(object parameter) => ReloadAsync(ViewOptions.Value);

        protected override void OnItemEditCommand([DisallowNull] ListItemViewModel item, object parameter)
        {
            throw new System.NotImplementedException();
        }

        protected override bool ConfirmItemDelete(ListItemViewModel item, object parameter) => MessageBox.Show(App.Current.MainWindow,
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
            throw new System.NotImplementedException();
        }
    }
}
