using FsInfoCat.Desktop.ViewModel.AsyncOps;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class FileSystemsPageVM : DbEntityListingPageVM<FileSystemListItem, FileSystemItemVM, FileSystemItemDetailViewModel>
    {
        #region IsEditingViewOptions Property Members

        private static readonly DependencyPropertyKey IsEditingViewOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsEditingViewOptions), typeof(bool), typeof(FileSystemsPageVM),
                new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="IsEditingViewOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsEditingViewOptionsProperty = IsEditingViewOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public bool IsEditingViewOptions { get => (bool)GetValue(IsEditingViewOptionsProperty); private set => SetValue(IsEditingViewOptionsPropertyKey, value); }

        #endregion
        #region ViewOptionsOkClick Command Property Members

        private static readonly DependencyPropertyKey ViewOptionsOkClickPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewOptionsOkClick),
            typeof(Commands.RelayCommand), typeof(FileSystemsPageVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewOptionsOkClick"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewOptionsOkClickProperty = ViewOptionsOkClickPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand ViewOptionsOkClick => (Commands.RelayCommand)GetValue(ViewOptionsOkClickProperty);

        #endregion
        #region ViewOptionCancelClick Command Property Members

        private static readonly DependencyPropertyKey ViewOptionCancelClickPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewOptionCancelClick),
            typeof(Commands.RelayCommand), typeof(FileSystemsPageVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewOptionCancelClick"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewOptionCancelClickProperty = ViewOptionCancelClickPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand ViewOptionCancelClick => (Commands.RelayCommand)GetValue(ViewOptionCancelClickProperty);

        #endregion
        #region ShowViewOptions Command Property Members

        private static readonly DependencyPropertyKey ShowViewOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ShowViewOptions), typeof(Commands.RelayCommand), typeof(FileSystemsPageVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ShowViewOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowViewOptionsProperty = ShowViewOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand ShowViewOptions => (Commands.RelayCommand)GetValue(ShowViewOptionsProperty);

        #endregion
        #region ViewOptions Property Members

        private static readonly DependencyPropertyKey ViewOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewOptions), typeof(ThreeStateViewModel), typeof(FileSystemsPageVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewOptionsProperty = ViewOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public ThreeStateViewModel ViewOptions => (ThreeStateViewModel)GetValue(ViewOptionsProperty);

        #endregion
        #region EditingViewOptions Property Members

        private static readonly DependencyPropertyKey EditingViewOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EditingViewOptions), typeof(ThreeStateViewModel), typeof(FileSystemsPageVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="EditingViewOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EditingViewOptionsProperty = EditingViewOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public ThreeStateViewModel EditingViewOptions => (ThreeStateViewModel)GetValue(EditingViewOptionsProperty);

        #endregion

        public FileSystemsPageVM()
        {
            ThreeStateViewModel viewOptions = new(true);
            SetValue(ViewOptionsPropertyKey, viewOptions);
            SetValue(EditingViewOptionsPropertyKey, new ThreeStateViewModel(viewOptions.Value));
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
            viewOptions.ValuePropertyChanged += (s, e) => LoadItemsAsync();
            SetValue(ShowViewOptionsPropertyKey, new Commands.RelayCommand(() => IsEditingViewOptions = true));
            SetValue(ViewOptionsOkClickPropertyKey, new Commands.RelayCommand(() =>
            {
                IsEditingViewOptions = false;
                ViewOptions.Value = EditingViewOptions.Value;
            }));
            SetValue(ViewOptionCancelClickPropertyKey, new Commands.RelayCommand(() =>
            {
                EditingViewOptions.Value = ViewOptions.Value;
                IsEditingViewOptions = false;
            }));
        }

        protected override Func<IWindowsStatusListener, Task<int>> GetItemsLoaderFactory()
        {
            bool? viewOptions = ViewOptions.Value;
            return listener => Task.Run(async () => await LoadItemsAsync(viewOptions, listener));
        }

        private async Task<int> LoadItemsAsync(bool? showActive, IWindowsStatusListener statusListener)
        {
            statusListener.CancellationToken.ThrowIfCancellationRequested();
            IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            IQueryable<FileSystemListItem> items;
            if (showActive.HasValue)
            {
                if (showActive.Value)
                    items = from f in dbContext.FileSystemListing where !f.IsInactive select f;
                else
                    items = from f in dbContext.FileSystemListing where f.IsInactive select f;
            }
            else
                items = from f in dbContext.FileSystemListing select f;
            return await OnEntitiesLoaded(items, statusListener, r => new FileSystemItemVM(r));
        }

        protected override DbSet<FileSystemListItem> GetDbSet(LocalDbContext dbContext) => dbContext.FileSystemListing;

        protected override void OnAddNewItem(object parameter)
        {
            FileSystem entity = new();
            bool? isInactive = ViewOptions.Value;
            if (isInactive.HasValue)
                entity.IsInactive = isInactive.Value;
            throw new NotImplementedException();
        }

        protected override bool ShowModalItemEditWindow(FileSystemItemVM item, object parameter, out string saveProgressTitle)
        {
            saveProgressTitle = $"Saving File System Definition \"{item.DisplayName}\"";
            return BgOps.FromAsync("Loading Details", "Connecting to database", item.Model.Id, LoadItemAsync).ContinueWith(task => Dispatcher.Invoke(() =>
            {
                FileSystem entity = task.Result;
                if (entity is null)
                    return false;
                EditFileSystemVM viewModel = new();
                return viewModel.ShowDialog(new View.Local.EditFileSystemWindow(), entity, false) ?? false;
            }), TaskContinuationOptions.OnlyOnRanToCompletion).Result;
        }

        private async Task<FileSystem> LoadItemAsync(Guid id, IWindowsStatusListener arg)
        {
            using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            return await dbContext.FileSystems.FindAsync(id);
        }

        protected override bool PromptItemDeleting(FileSystemItemVM item, object parameter, out string deleteProgressTitle)
        {
            using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            deleteProgressTitle = $"Deleting File System Definition \"{item.DisplayName}\"";
            return MessageBox.Show(serviceScope.ServiceProvider.GetRequiredService<MainWindow>(),
                "This cannot be undone.\n\nAre you sure you want to delete this File System?", "Delete File System", MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.Yes;
        }

        //public record EntityAndCounts(FileSystemListItem Entity, int VolumeCount, int SymbolicNameCount);
    }
}
