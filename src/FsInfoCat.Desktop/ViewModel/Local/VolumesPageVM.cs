using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    /// <summary>
    /// View Model for <see cref="View.Local.VolumesPage"/>.
    /// </summary>
    public class VolumesPageVM : DbEntityListingPageVM<VolumeListItemWithFileSystem, VolumeItemWithFileSystemVM>
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
        protected void OnCurrentFileSystemPropertyChanged(FileSystem oldValue, FileSystem newValue)
        {
            // DEFERRED: Implement OnCurrentFileSystemPropertyChanged Logic
        }

        #endregion
        #region ColumnVisibilities Property Members

        private static readonly DependencyPropertyKey ColumnVisibilitiesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ColumnVisibilities), typeof(VolumesWithFileSystemColumnVisibilitiesViewModel), typeof(VolumesPageVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ColumnVisibilities"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColumnVisibilitiesProperty = ColumnVisibilitiesPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public VolumesWithFileSystemColumnVisibilitiesViewModel ColumnVisibilities => (VolumesWithFileSystemColumnVisibilitiesViewModel)GetValue(ColumnVisibilitiesProperty);

        #endregion
        public VolumesPageVM()
        {
            SetValue(StatusOptionsPropertyKey, new EnumValueSelectorVM<VolumeStatus>());
            SetValue(ColumnVisibilitiesPropertyKey, new VolumesWithFileSystemColumnVisibilitiesViewModel());
        }

        private async Task<int> LoadItemsAsync(ItemLoadParams state, IWindowsStatusListener statusListener)
        {
            statusListener.CancellationToken.ThrowIfCancellationRequested();
            IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            IQueryable<VolumeListItemWithFileSystem> items;
            if (state.FileSystemId.HasValue)
            {
                Guid id = state.FileSystemId.Value;

                if (state.StatusValues.Length == 1)
                {
                    VolumeStatus status = state.StatusValues[0];
                    items = from v in dbContext.VolumeListingWithFileSystem where v.FileSystemId == id && v.Status == status select v;
                }
                else if (state.StatusValues.Length > 1)
                {
                    VolumeStatus[] sv = state.StatusValues;
                    items = from v in dbContext.VolumeListingWithFileSystem where v.FileSystemId == id && sv.Contains(v.Status) select v;
                }
                else
                    items = from v in dbContext.VolumeListingWithFileSystem where v.FileSystemId == id select v;
            }
            else if (state.StatusValues.Length == 1)
            {
                VolumeStatus status = state.StatusValues[0];
                items = from v in dbContext.VolumeListingWithFileSystem where v.Status == status select v;
            }
            else if (state.StatusValues.Length > 1)
            {
                VolumeStatus[] sv = state.StatusValues;
                items = from v in dbContext.VolumeListingWithFileSystem where sv.Contains(v.Status) select v;
            }
            else
                items = from v in dbContext.VolumeListingWithFileSystem select v;
            return await OnEntitiesLoaded(items, statusListener, entity => new VolumeItemWithFileSystemVM(entity));
        }

        protected override DbSet<VolumeListItemWithFileSystem> GetDbSet(LocalDbContext dbContext) => dbContext.VolumeListingWithFileSystem;

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

        protected override bool ShowModalItemEditWindow(VolumeItemWithFileSystemVM item, object parameter, out string saveProgressTitle)
        {
            saveProgressTitle = string.Format(FsInfoCat.Properties.Resources.FormatMessage_SavingVolumeChanges, item.DisplayName);
            IWindowsAsyncJobFactoryService service = Services.ServiceProvider.GetRequiredService<IWindowsAsyncJobFactoryService>();
            return service.RunAsync("Loading Details", "Connecting to database", item.Model.Id, LoadItemAsync).ContinueWith(task => Dispatcher.Invoke(() =>
            {
                Volume entity = task.Result;
                if (entity is null)
                    return false;
                EditVolumeVM viewModel = new();
                return viewModel.ShowDialog(new View.Local.EditVolumeWindow(), entity, false) ?? false;
            }), TaskContinuationOptions.OnlyOnRanToCompletion).Result;
        }

        private async Task<Volume> LoadItemAsync(Guid id, IWindowsStatusListener arg)
        {
            using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            return await dbContext.Volumes.FindAsync(id);
        }

        protected override bool PromptItemDeleting(VolumeItemWithFileSystemVM item, object parameter, out string deleteProgressTitle)
        {
            using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            deleteProgressTitle = string.Format(FsInfoCat.Properties.Resources.FormatMessage_DeletingVolume, item.DisplayName);
            return MessageBox.Show(serviceScope.ServiceProvider.GetRequiredService<MainWindow>(),
                "This cannot be undone.\n\nAre you sure you want to delete this Volume?", "Delete Volume", MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.Yes;
        }

        public record ItemLoadParams(Guid? FileSystemId, VolumeStatus[] StatusValues);
    }
}
