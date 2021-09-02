using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.Local.Volumes
{
    public class ListingViewModel : ListingViewModel<VolumeListItemWithFileSystem, ListItemViewModel, (VolumeStatus? Status, bool? ShowActiveOnly)>, INotifyNavigatedTo
    {
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

        public ListingViewModel()
        {
            string[] names = new[] { FsInfoCat.Properties.Resources.DisplayName_AllItems, FsInfoCat.Properties.Resources.DisplayName_ActiveItems, FsInfoCat.Properties.Resources.DisplayName_InactiveItems };
            EnumValuePickerVM<VolumeStatus> viewOptions = new(names);
            _allOption = viewOptions.Choices.First(o => o.DisplayName == FsInfoCat.Properties.Resources.DisplayName_AllItems);
            _inactiveOption = viewOptions.Choices.First(o => o.DisplayName == FsInfoCat.Properties.Resources.DisplayName_InactiveItems);
            SetValue(ViewOptionsPropertyKey, viewOptions);
            viewOptions.SelectedItemPropertyChanged += (object sender, DependencyPropertyChangedEventArgs e) => ReloadAsync(e.NewValue as EnumChoiceItem<VolumeStatus>);
            SetValue(EditingOptionsPropertyKey, new EnumValuePickerVM<VolumeStatus>(names) { SelectedIndex = viewOptions.SelectedIndex });
        }

        void INotifyNavigatedTo.OnNavigatedTo() => ReloadAsync(ViewOptions.SelectedItem);

        private IAsyncJob ReloadAsync(EnumChoiceItem<VolumeStatus> selectedItem)
        {
            VolumeStatus? status;
            if (selectedItem is not null)
            {
                if ((status = selectedItem.Value).HasValue || ReferenceEquals(selectedItem, _allOption))
                    return ReloadAsync((status, null));
                return ReloadAsync((status, !ReferenceEquals(selectedItem, _inactiveOption)));
            }
            return ReloadAsync((null, true));
        }

        protected override IQueryable<VolumeListItemWithFileSystem> GetQueryableListing((VolumeStatus? Status, bool? ShowActiveOnly) options, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            statusListener.SetMessage("Reading volume information records from database");
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
            // BUG: Need to fix logic
            ViewOptions.SelectedIndex = EditingOptions.SelectedIndex;
        }

        protected override void OnCancelFilterOptionsCommand(object parameter)
        {
            EditingOptions.SelectedIndex = ViewOptions.SelectedIndex;
            base.OnCancelFilterOptionsCommand(parameter);
        }
        protected override void OnRefreshCommand(object parameter) => ReloadAsync(ViewOptions.SelectedItem);

        protected override void OnItemEditCommand(ListItemViewModel item, object parameter)
        {
            // TODO: Implement OnItemEditCommand(object);
        }

        protected override bool ConfirmItemDelete(ListItemViewModel item, object parameter) => MessageBox.Show(App.Current.MainWindow,
            "This action cannot be undone!\n\nAre you sure you want to remove this volume record from the database?",
            "Delete Volume Record", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes;

        protected override void OnAddNewItemCommand(object parameter)
        {
            // TODO: Implement OnAddNewItemCommand(object);
        }

        protected override async Task<int> DeleteEntityFromDbContextAsync([DisallowNull] VolumeListItemWithFileSystem entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            Volume target = await dbContext.Volumes.FindAsync(new object[] { entity.Id }, statusListener.CancellationToken);
            return (target is null) ? 0 : await Volume.DeleteAsync(target, dbContext, statusListener);
        }
    }
}
