using FsInfoCat.Desktop.ViewModel.AsyncOps;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
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

        public record EntityAndCounts(FileSystem Entity, int VolumeCount, int SymbolicNameCount);
    }
}
