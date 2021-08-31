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

        protected override IQueryable<SharedTagDefinitionListItem> GetQueryableListing(bool? options, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener) =>
            options.HasValue ? (options.Value ? dbContext.SharedTagDefinitionListing.Where(f => !f.IsInactive) : dbContext.SharedTagDefinitionListing.Where(f => f.IsInactive)) : dbContext.SharedTagDefinitionListing;

        protected override ListItemViewModel CreateItemViewModel([DisallowNull] SharedTagDefinitionListItem entity) => new ListItemViewModel(entity);

        protected override void OnSaveFilterOptionsCommand(object parameter)
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

        protected override bool ConfirmItemDelete([DisallowNull] ListItemViewModel item, object parameter)
        {
            throw new System.NotImplementedException();
        }

        protected override async Task<int> DeleteEntityFromDbContextAsync([DisallowNull] SharedTagDefinitionListItem entity, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnAddNewItemCommand(object parameter)
        {
            throw new System.NotImplementedException();
        }
    }
    public class DetailsViewModel : DependencyObject
    {
    }
    public class EditViewModel : DependencyObject
    {

    }
}
