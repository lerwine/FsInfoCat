using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
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

        protected override void OnSaveFilterOptionsCommand(object parameter)
        {
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
            throw new NotImplementedException();
        }

        protected override bool ConfirmItemDelete(ListItemViewModel item, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override void OnAddNewItemCommand(object parameter)
        {
            throw new NotImplementedException();
        }

        protected override async Task<int> DeleteEntityFromDbContextAsync([DisallowNull] VolumeListItemWithFileSystem entity, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
        {
            Guid id = entity.Id;
            Volume volume = await dbContext.Volumes.FirstOrDefaultAsync(e => e.Id == id);
            if (volume is null)
                return 0;
            dbContext.Volumes.Remove(volume);
            return await dbContext.SaveChangesAsync(statusListener.CancellationToken);
        }
    }
    public class DetailsViewModel : DependencyObject
    {

    }
    public class EditViewModel : DependencyObject
    {

    }
}
