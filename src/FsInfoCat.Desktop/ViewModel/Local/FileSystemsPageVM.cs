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
    public class FileSystemsPageVM : DbEntityListingPageVM<FileSystem, FileSystemItemVM, FileSystemItemDetailViewModel>
    {
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

        public FileSystemsPageVM()
        {
            ThreeStateViewModel viewOptions = new(true);
            SetValue(ViewOptionsPropertyKey, viewOptions);
            viewOptions.ValuePropertyChanged += (s, e) => LoadItemsAsync();
        }

        protected override Func<IStatusListener, Task<int>> GetItemsLoaderFactory()
        {
            bool? viewOptions = ViewOptions.Value;
            return listener => Task.Run(async () => await LoadItemsAsync(viewOptions, listener));
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
