using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.Local.Volumes
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
            ViewOptions.SelectedIndex = EditingOptions.SelectedIndex;
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
            EditingOptions.SelectedIndex = ViewOptions.SelectedIndex;
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

        private readonly EnumChoiceItem<VolumeStatus> _allOption;
        private readonly EnumChoiceItem<VolumeStatus> _inactiveOption;

        private static readonly DependencyPropertyKey ViewOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewOptions), typeof(EnumValuePickerVM<VolumeStatus>), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewOptionsProperty = ViewOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public EnumValuePickerVM<VolumeStatus> ViewOptions => (EnumValuePickerVM<VolumeStatus>)GetValue(ViewOptionsProperty);

        #endregion
        #region EditingOptions Property Members

        private static readonly DependencyPropertyKey EditingOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EditingOptions), typeof(EnumValuePickerVM<VolumeStatus>), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="EditingOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EditingOptionsProperty = EditingOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public EnumValuePickerVM<VolumeStatus> EditingOptions => (EnumValuePickerVM<VolumeStatus>)GetValue(EditingOptionsProperty);

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
            SetValue(ItemsPropertyKey, new ReadOnlyObservableCollection<ListItemViewModel>(_backingItems));
            string[] names = new[] {FsInfoCat.Properties.Resources.DisplayName_AllItems,
                FsInfoCat.Properties.Resources.DisplayName_ActiveItems, FsInfoCat.Properties.Resources.DisplayName_InctiveItems};
            EnumValuePickerVM<VolumeStatus> viewOptions = new(names);
            _allOption = viewOptions.Choices.First(o => o.DisplayName == FsInfoCat.Properties.Resources.DisplayName_AllItems);
            _inactiveOption = viewOptions.Choices.First(o => o.DisplayName == FsInfoCat.Properties.Resources.DisplayName_InctiveItems);
            SetValue(ViewOptionsPropertyKey, viewOptions);
            viewOptions.SelectedItemPropertyChanged += (object sender, DependencyPropertyChangedEventArgs e) => ReloadAsync(e.NewValue as EnumChoiceItem<VolumeStatus>);
            SetValue(EditingOptionsPropertyKey, new EnumValuePickerVM<VolumeStatus>(names) { SelectedIndex = viewOptions.SelectedIndex });
        }

        private IAsyncJob ReloadAsync(EnumChoiceItem<VolumeStatus> selectedItem)
        {
            bool? showActiveOnly;
            VolumeStatus? status;
            if (selectedItem is not null)
            {
                if ((status = selectedItem.Value).HasValue || ReferenceEquals(selectedItem, _allOption))
                    showActiveOnly = null;
                else
                    showActiveOnly = !ReferenceEquals(selectedItem, _inactiveOption);
            }
            else
            {
                status = null;
                showActiveOnly = true;
            }
            IWindowsAsyncJobFactoryService jobFactory = Services.GetRequiredService<IWindowsAsyncJobFactoryService>();
            return jobFactory.StartNew("Loading volumes", "Opening database", status, showActiveOnly, LoadItemsAsync);
        }

        void INotifyNavigatedTo.OnNavigatedTo() => ReloadAsync(ViewOptions.SelectedItem);

        private async Task LoadItemsAsync(VolumeStatus? status, bool? showActiveOnly, IWindowsStatusListener statusListener)
        {
            using IServiceScope scope = Services.CreateScope();
            using LocalDbContext dbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
            IQueryable<VolumeListItemWithFileSystem> items;
            if (status.HasValue)
            {
                VolumeStatus s = status.Value;
                items = dbContext.VolumeListingWithFileSystem.Where(v => v.Status == s);
            }
            else if (showActiveOnly.HasValue)
            {
                if (showActiveOnly.Value)
                    items = dbContext.VolumeListingWithFileSystem.Where(v => v.Status == VolumeStatus.Controlled || v.Status == VolumeStatus.AccessError ||
                    v.Status == VolumeStatus.Offline);
                else
                    items = dbContext.VolumeListingWithFileSystem.Where(v => v.Status != VolumeStatus.Controlled && v.Status != VolumeStatus.AccessError &&
                    v.Status != VolumeStatus.Offline);
            }
            else
                items = dbContext.VolumeListingWithFileSystem;
            await items.ForEachAsync(async item => await AddItemAsync(item, statusListener), statusListener.CancellationToken);
        }

        private DispatcherOperation AddItemAsync(VolumeListItemWithFileSystem model, IWindowsStatusListener statusListener) => Dispatcher.InvokeAsync(() =>
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
