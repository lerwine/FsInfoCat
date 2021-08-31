using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.Local.PersonalTagDefinitions
{
    public class ListingViewModel : DependencyObject, INotifyNavigatedTo
    {
        #region AddNewItemButtonClick Command Property Members

        private static readonly DependencyPropertyKey AddNewItemButtonClickPropertyKey = DependencyProperty.RegisterReadOnly(nameof(AddNewItemButtonClick),
            typeof(Commands.RelayCommand), typeof(ListingViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="AddNewItemButtonClick"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AddNewItemButtonClickProperty = AddNewItemButtonClickPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand AddNewItemButtonClick => (Commands.RelayCommand)GetValue(AddNewItemButtonClickProperty);

        private void OnAddNewItemButtonClick(object parameter)
        {
            // TODO: Implement OnAddNewItemButtonClick Logic
        }

        #endregion
        #region ShowFilterOptionsButtonClick Command Property Members

        private static readonly DependencyPropertyKey ShowFilterOptionsButtonClickPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ShowFilterOptionsButtonClick),
            typeof(Commands.RelayCommand), typeof(ListingViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ShowFilterOptionsButtonClick"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowFilterOptionsButtonClickProperty = ShowFilterOptionsButtonClickPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand ShowFilterOptionsButtonClick => (Commands.RelayCommand)GetValue(ShowFilterOptionsButtonClickProperty);

        private void OnShowFilterOptionsButtonClick(object parameter)
        {
            ViewOptionsVisible = true;
        }

        #endregion
        #region SaveFilterOptionsButtonClick Command Property Members

        private static readonly DependencyPropertyKey SaveFilterOptionsButtonClickPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SaveFilterOptionsButtonClick),
            typeof(Commands.RelayCommand), typeof(ListingViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="SaveFilterOptionsButtonClick"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SaveFilterOptionsButtonClickProperty = SaveFilterOptionsButtonClickPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand SaveFilterOptionsButtonClick => (Commands.RelayCommand)GetValue(SaveFilterOptionsButtonClickProperty);

        private void OnSaveFilterOptionsButtonClick(object parameter)
        {
            ViewOptions.Value = EditingOptions.Value;
            ViewOptionsVisible = false;
        }

        #endregion
        #region CancelFilterOptionsButtonClick Command Property Members

        private static readonly DependencyPropertyKey CancelFilterOptionsButtonClickPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CancelFilterOptionsButtonClick),
            typeof(Commands.RelayCommand), typeof(ListingViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="CancelFilterOptionsButtonClick"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CancelFilterOptionsButtonClickProperty = CancelFilterOptionsButtonClickPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand CancelFilterOptionsButtonClick => (Commands.RelayCommand)GetValue(CancelFilterOptionsButtonClickProperty);

        private void OnCancelFilterOptionsButtonClick(object parameter)
        {
            EditingOptions.Value = ViewOptions.Value;
            ViewOptionsVisible = false;
        }

        #endregion
        #region ViewOptionsVisible Property Members

        private static readonly DependencyPropertyKey ViewOptionsVisiblePropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewOptionsVisible), typeof(bool), typeof(ListingViewModel),
                new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="ViewOptionsVisible"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewOptionsVisibleProperty = ViewOptionsVisiblePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public bool ViewOptionsVisible { get => (bool)GetValue(ViewOptionsVisibleProperty); private set => SetValue(ViewOptionsVisiblePropertyKey, value); }

        #endregion
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
        #region Items Property Members

        private readonly ObservableCollection<ListItemViewModel> _backingItems = new();

        private static readonly DependencyPropertyKey ItemsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Items), typeof(ReadOnlyObservableCollection<ListItemViewModel>), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Items"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsProperty = ItemsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the items to be displayed in the page listing.
        /// </summary>
        /// <value>The items to be displayed in the page listing.</value>
        public ReadOnlyObservableCollection<ListItemViewModel> Items => (ReadOnlyObservableCollection<ListItemViewModel>)GetValue(ItemsProperty);

        #endregion

        public ListingViewModel()
        {
            SetValue(AddNewItemButtonClickPropertyKey, new Commands.RelayCommand(OnAddNewItemButtonClick));
            SetValue(ShowFilterOptionsButtonClickPropertyKey, new Commands.RelayCommand(OnShowFilterOptionsButtonClick));
            SetValue(SaveFilterOptionsButtonClickPropertyKey, new Commands.RelayCommand(OnSaveFilterOptionsButtonClick));
            SetValue(CancelFilterOptionsButtonClickPropertyKey, new Commands.RelayCommand(OnCancelFilterOptionsButtonClick));
            ThreeStateViewModel viewOptions = new(true);
            SetValue(ViewOptionsPropertyKey, viewOptions);
            SetValue(ItemsPropertyKey, new ReadOnlyObservableCollection<ListItemViewModel>(_backingItems));
            viewOptions.ValuePropertyChanged += (sender, e) => ReloadAsync(e.NewValue as bool?);
            SetValue(EditingOptionsPropertyKey, new ThreeStateViewModel(viewOptions.Value));
        }

        private IAsyncJob ReloadAsync(bool? showActiveOnly)
        {
            IWindowsAsyncJobFactoryService jobFactory = Services.GetRequiredService<IWindowsAsyncJobFactoryService>();
            return jobFactory.StartNew("Loading personal tags", "Opening database", showActiveOnly, LoadItemsAsync);
        }

        void INotifyNavigatedTo.OnNavigatedTo() => ReloadAsync(ViewOptions.Value);

        private async Task LoadItemsAsync(bool? showActiveOnly, IWindowsStatusListener statusListener)
        {
            using IServiceScope scope = Services.CreateScope();
            using LocalDbContext dbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
            IQueryable<PersonalTagDefinitionListItem> items = showActiveOnly.HasValue ?
                (showActiveOnly.Value ? dbContext.PersonalTagDefinitionListing.Where(f => !f.IsInactive) :
                dbContext.PersonalTagDefinitionListing.Where(f => f.IsInactive)) : dbContext.PersonalTagDefinitionListing;
            await items.ForEachAsync(async item => await AddItemAsync(item, statusListener), statusListener.CancellationToken);
        }

        private DispatcherOperation AddItemAsync(PersonalTagDefinitionListItem model, IWindowsStatusListener statusListener) => Dispatcher.InvokeAsync(() =>
        {
            ListItemViewModel item = new ListItemViewModel(model);
            _backingItems.Add(item);
        }, DispatcherPriority.Background, statusListener.CancellationToken);
    }
    public class DetailsViewModel : DependencyObject
    {
    }
    public class EditViewModel : DependencyObject
    {

    }
}
