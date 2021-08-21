using FsInfoCat.Desktop.ViewModel.AsyncOps;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class VolumesPageVM : DbEntityListingPageVM<Volume, VolumeItemVM>
    {
        #region StatusOptions Property Members

        private static readonly DependencyPropertyKey StatusOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(StatusOptions), typeof(EnumValueSelectorVM<VolumeStatus>), typeof(VolumesPageVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="StatusOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusOptionsProperty = StatusOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public EnumValueSelectorVM<VolumeStatus> StatusOptions => (EnumValueSelectorVM<VolumeStatus>)GetValue(StatusOptionsProperty);

        #endregion
        #region CurrentFileSystem Property Members

        /// <summary>
        /// Identifies the <see cref="CurrentFileSystem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CurrentFileSystemProperty = DependencyProperty.Register(nameof(CurrentFileSystem), typeof(FileSystem), typeof(VolumesPageVM),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as VolumesPageVM)?.OnCurrentFileSystemPropertyChanged((FileSystem)e.OldValue, (FileSystem)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public FileSystem CurrentFileSystem { get => (FileSystem)GetValue(CurrentFileSystemProperty); set => SetValue(CurrentFileSystemProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="CurrentFileSystem"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="CurrentFileSystem"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="CurrentFileSystem"/> property.</param>
        private void OnCurrentFileSystemPropertyChanged(FileSystem oldValue, FileSystem newValue)
        {
            // TODO: Implement OnCurrentFileSystemPropertyChanged Logic
        }

        #endregion

        public VolumesPageVM()
        {
            SetValue(StatusOptionsPropertyKey, new EnumValueSelectorVM<VolumeStatus>());
        }

        private async Task<int> LoadItemsAsync(ItemLoadParams state, IWindowsStatusListener statusListener)
        {
            statusListener.CancellationToken.ThrowIfCancellationRequested();
            IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            IIncludableQueryable<Volume, FileSystem> volumes = dbContext.Volumes.Include(v => v.FileSystem);
            IQueryable<Volume> items;
            if (state.FileSystemId.HasValue)
            {
                Guid id = state.FileSystemId.Value;

                if (state.StatusValues.Length == 1)
                {
                    VolumeStatus status = state.StatusValues[0];
                    items = from v in volumes where v.FileSystemId == id && v.Status == status select v;
                }
                else if (state.StatusValues.Length > 1)
                {
                    VolumeStatus[] sv = state.StatusValues;
                    items = from v in volumes where v.FileSystemId == id && sv.Contains(v.Status) select v;
                }
                else
                    items = from v in volumes where v.FileSystemId == id select v;
            }
            else if (state.StatusValues.Length == 1)
            {
                VolumeStatus status = state.StatusValues[0];
                items = from v in volumes where v.Status == status select v;
            }
            else if (state.StatusValues.Length > 1)
            {
                VolumeStatus[] sv = state.StatusValues;
                items = from v in volumes where sv.Contains(v.Status) select v;
            }
            else
                items = from v in volumes select v;
            return await OnEntitiesLoaded(items, statusListener, entity => new VolumeItemVM(entity));
        }

        protected override DbSet<Volume> GetDbSet(LocalDbContext dbContext) => dbContext.Volumes;

        protected override Func<IWindowsStatusListener, Task<int>> GetItemsLoaderFactory()
        {
            ItemLoadParams loadParams = new(CurrentFileSystem?.Id, StatusOptions.SelectedItems.Select(e => e.Value).ToArray());
            return listener => Task.Run(async () => await LoadItemsAsync(loadParams, listener));
        }

        protected override void OnAddNewItem(object parameter)
        {
            Volume volume = new() { FileSystem = CurrentFileSystem };
            VolumeStatus[] value = StatusOptions.SelectedItems.Select(i => i.Value).ToArray();
            if (value.Length == 1)
                volume.Status = value[0];
            throw new NotImplementedException();
            //string progressTitle = string.Format(FsInfoCat.Properties.Resources.FormatMessage_AddingNewVolume, item.DisplayName);
            throw new NotImplementedException();
        }

        protected override bool ShowModalItemEditWindow(VolumeItemVM item, object parameter, out string saveProgressTitle)
        {
            saveProgressTitle = string.Format(FsInfoCat.Properties.Resources.FormatMessage_SavingVolumeChanges, item.DisplayName);
            throw new NotImplementedException();
        }

        protected override bool PromptItemDeleting(VolumeItemVM item, object parameter, out string deleteProgressTitle)
        {
            deleteProgressTitle = string.Format(FsInfoCat.Properties.Resources.FormatMessage_DeletingVolume, item.DisplayName);
            throw new NotImplementedException();
        }

        public record ItemLoadParams(Guid? FileSystemId, VolumeStatus[] StatusValues);
    }
}
