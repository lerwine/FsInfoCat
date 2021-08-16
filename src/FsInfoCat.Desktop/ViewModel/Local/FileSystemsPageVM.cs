using FsInfoCat.Desktop.ViewModel.AsyncOps;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class FileSystemsPageVM : DbEntityListingPageVM<FileSystem, FileSystemItemVM>
    {
        #region ShowActiveOnly Property Members

        /// <summary>
        /// Identifies the <see cref="ShowActiveOnly"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowActiveOnlyProperty = DependencyProperty.Register(nameof(ShowActiveOnly), typeof(bool), typeof(FileSystemsPageVM),
                new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as FileSystemsPageVM)?.OnShowActiveOnlyPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool ShowActiveOnly { get => (bool)GetValue(ShowActiveOnlyProperty); set => SetValue(ShowActiveOnlyProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ShowActiveOnly"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ShowActiveOnly"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ShowActiveOnly"/> property.</param>
        private void OnShowActiveOnlyPropertyChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                ShowInactiveOnly = ShowAllItems = false;
                LoadItemsAsync();
            }
            else if (!(ShowInactiveOnly || ShowAllItems))
                ShowInactiveOnly = true;
        }

        #endregion
        #region ShowInactiveOnly Property Members

        /// <summary>
        /// Identifies the <see cref="ShowInactiveOnly"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowInactiveOnlyProperty = DependencyProperty.Register(nameof(ShowInactiveOnly), typeof(bool), typeof(FileSystemsPageVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as FileSystemsPageVM)?.OnShowInactiveOnlyPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool ShowInactiveOnly { get => (bool)GetValue(ShowInactiveOnlyProperty); set => SetValue(ShowInactiveOnlyProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ShowInactiveOnly"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ShowInactiveOnly"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ShowInactiveOnly"/> property.</param>
        private void OnShowInactiveOnlyPropertyChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                ShowActiveOnly = ShowAllItems = false;
                LoadItemsAsync();
            }
            else if (!(ShowActiveOnly || ShowAllItems))
                ShowActiveOnly = true;
        }

        #endregion
        #region ShowAllItems Property Members

        /// <summary>
        /// Identifies the <see cref="ShowAllItems"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowAllItemsProperty = DependencyProperty.Register(nameof(ShowAllItems), typeof(bool), typeof(FileSystemsPageVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as FileSystemsPageVM)?.OnShowAllItemsPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool ShowAllItems { get => (bool)GetValue(ShowAllItemsProperty); set => SetValue(ShowAllItemsProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ShowAllItems"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ShowAllItems"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ShowAllItems"/> property.</param>
        private void OnShowAllItemsPropertyChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                ShowActiveOnly = ShowInactiveOnly = false;
                LoadItemsAsync();
            }
            else if (!(ShowActiveOnly || ShowInactiveOnly))
                ShowActiveOnly = true;
        }

        #endregion
        #region SymbolicNames Property Members

        private readonly ObservableCollection<SymbolicNameItemVM> _backingSymbolicNames = new();

        private static readonly DependencyPropertyKey SymbolicNamesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SymbolicNames), typeof(ReadOnlyObservableCollection<SymbolicNameItemVM>), typeof(FileSystemsPageVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="SymbolicNames"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SymbolicNamesProperty = SymbolicNamesPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public ReadOnlyObservableCollection<SymbolicNameItemVM> SymbolicNames => (ReadOnlyObservableCollection<SymbolicNameItemVM>)GetValue(SymbolicNamesProperty);

        #endregion
        #region ShowActiveSymbolicNames Property Members

        /// <summary>
        /// Identifies the <see cref="ShowActiveSymbolicNames"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowActiveSymbolicNamesProperty = DependencyProperty.Register(nameof(ShowActiveSymbolicNames), typeof(bool), typeof(FileSystemsPageVM),
                new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as FileSystemsPageVM)?.OnShowActiveSymbolicNamesPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool ShowActiveSymbolicNames { get => (bool)GetValue(ShowActiveSymbolicNamesProperty); set => SetValue(ShowActiveSymbolicNamesProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ShowActiveSymbolicNames"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ShowActiveSymbolicNames"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ShowActiveSymbolicNames"/> property.</param>
        private void OnShowActiveSymbolicNamesPropertyChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                ShowInactiveSymbolicNames = ShowAllSymbolicNames = false;
                LoadItemsAsync();
            }
            else if (!(ShowInactiveSymbolicNames || ShowAllSymbolicNames))
                ShowInactiveSymbolicNames = true;
        }

        #endregion
        #region ShowInactiveSymbolicNames Property Members

        /// <summary>
        /// Identifies the <see cref="ShowInactiveSymbolicNames"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowInactiveSymbolicNamesProperty = DependencyProperty.Register(nameof(ShowInactiveSymbolicNames), typeof(bool), typeof(FileSystemsPageVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as FileSystemsPageVM)?.OnShowInactiveSymbolicNamesPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool ShowInactiveSymbolicNames { get => (bool)GetValue(ShowInactiveSymbolicNamesProperty); set => SetValue(ShowInactiveSymbolicNamesProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ShowInactiveSymbolicNames"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ShowInactiveSymbolicNames"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ShowInactiveSymbolicNames"/> property.</param>
        private void OnShowInactiveSymbolicNamesPropertyChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                ShowActiveSymbolicNames = ShowAllSymbolicNames = false;
                LoadItemsAsync();
            }
            else if (!(ShowActiveSymbolicNames || ShowAllSymbolicNames))
                ShowActiveSymbolicNames = true;
        }

        #endregion
        #region ShowAllSymbolicNames Property Members

        /// <summary>
        /// Identifies the <see cref="ShowAllSymbolicNames"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowAllSymbolicNamesProperty = DependencyProperty.Register(nameof(ShowAllSymbolicNames), typeof(bool), typeof(FileSystemsPageVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as FileSystemsPageVM)?.OnShowAllSymbolicNamesPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool ShowAllSymbolicNames { get => (bool)GetValue(ShowAllSymbolicNamesProperty); set => SetValue(ShowAllSymbolicNamesProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ShowAllSymbolicNames"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ShowAllSymbolicNames"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ShowAllSymbolicNames"/> property.</param>
        private void OnShowAllSymbolicNamesPropertyChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                ShowActiveSymbolicNames = ShowInactiveSymbolicNames = false;
                LoadItemsAsync();
            }
            else if (!(ShowActiveSymbolicNames || ShowInactiveSymbolicNames))
                ShowActiveSymbolicNames = true;
        }

        #endregion
        public FileSystemsPageVM()
        {
            SetValue(SymbolicNamesPropertyKey, new ReadOnlyObservableCollection<SymbolicNameItemVM>(_backingSymbolicNames));
        }

        protected override Func<IStatusListener, Task<int>> GetItemsLoaderFactory()
        {
            bool? showActive = ShowAllItems ? null : ShowActiveOnly;
            return listener => Task.Run(async () => await LoadItemsAsync(showActive, listener));
        }

        private async Task<int> LoadItemsAsync(bool? showActive, IStatusListener statusListener)
        {
            statusListener.CancellationToken.ThrowIfCancellationRequested();
            IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            IQueryable<EntityAndCounts> items;
            if (showActive.HasValue)
            {
                if (showActive.Value)
                    items = from f in dbContext.FileSystems where !f.IsInactive select new EntityAndCounts(f, f.Volumes.Count(), f.SymbolicNames.Count());
                else
                    items = from f in dbContext.FileSystems where f.IsInactive select new EntityAndCounts(f, f.Volumes.Count(), f.SymbolicNames.Count());
            }
            else
                items = from f in dbContext.FileSystems select new EntityAndCounts(f, f.Volumes.Count(), f.SymbolicNames.Count());
            return await OnEntitiesLoaded(items, statusListener, r => new FileSystemItemVM(r));
        }

        protected override FileSystemItemVM CreateItem(FileSystem entity) => new(entity);

        protected override DbSet<FileSystem> GetDbSet(LocalDbContext dbContext) => dbContext.FileSystems;

        protected override string GetDeleteProgressTitle(FileSystemItemVM item)
        {
            throw new NotImplementedException();
        }

        protected override string GetSaveExistingProgressTitle(FileSystemItemVM item)
        {
            throw new NotImplementedException();
        }

        protected override string GetSaveNewProgressTitle(FileSystemItemVM item)
        {
            throw new NotImplementedException();
        }

        protected override FileSystem InitializeNewEntity()
        {
            throw new NotImplementedException();
        }

        protected override bool PromptItemDeleting(FileSystemItemVM item, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override bool ShowModalItemEditWindow(FileSystemItemVM item, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override void OnSelectedItemPropertyChanged(FileSystemItemVM oldValue, FileSystemItemVM newValue)
        {
            _backingSymbolicNames.Clear();
            StartSymbolicNamesLoad(newValue?.Model.Id, ShowAllSymbolicNames ? null : ShowActiveSymbolicNames);
            base.OnSelectedItemPropertyChanged(oldValue, newValue);
        }

        private void StartSymbolicNamesLoad(Guid? fileSystemId, bool? showActive)
        {
            _backingSymbolicNames.Clear();
            if (fileSystemId.HasValue)
                _ = BgOps.FromAsync("", "", fileSystemId.Value, RelatedItemLoader, new Func<Guid, IStatusListener<Guid>, Task<int>>((id, listener) =>
                Task.Run(async () => await LoadSymbolicNamesAsync(id, showActive, listener))));
        }

        private async Task<int> LoadSymbolicNamesAsync(Guid fileSystemId, bool? showActive, IStatusListener statusListener)
        {
            statusListener.CancellationToken.ThrowIfCancellationRequested();
            IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            SymbolicName[] items;
            if (showActive.HasValue)
            {
                if (showActive.Value)
                    items = await (from s in dbContext.SymbolicNames where !s.IsInactive && s.FileSystemId == fileSystemId select s).ToArrayAsync(statusListener.CancellationToken);
                else
                    items = await (from s in dbContext.SymbolicNames where s.IsInactive && s.FileSystemId == fileSystemId select s).ToArrayAsync(statusListener.CancellationToken);
            }
            else
                items = await (from s in dbContext.SymbolicNames where s.FileSystemId == fileSystemId select s).ToArrayAsync(statusListener.CancellationToken);

            return await Dispatcher.InvokeAsync(() =>
            {
                _backingSymbolicNames.Clear();
                if (SelectedItem?.Model.Id == fileSystemId)
                {
                    SelectedItem.SymbolicNameCount = items.Length;
                    foreach (SymbolicName item in items)
                        _backingSymbolicNames.Add(new SymbolicNameItemVM(item));
                    return items.Length;
                }
                return 0;
            }, DispatcherPriority.Background, statusListener.CancellationToken);
        }

        public record EntityAndCounts(FileSystem Entity, int VolumeCount, int SymbolicNameCount);
    }
}
