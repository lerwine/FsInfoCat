using FsInfoCat.Desktop.ViewModel.AsyncOps;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class SymbolicNamesPageVM : DbEntityListingPageVM<SymbolicName, SymbolicNameItemVM>
    {
        #region ItemsLoadOp Property Members

        private static readonly DependencyPropertyKey ItemsLoadOpPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ItemsLoadOp), typeof(AsyncOps.AsyncOpResultManagerViewModel<bool?, int>), typeof(SymbolicNamesPageVM),
                new PropertyMetadata(new AsyncOps.AsyncOpResultManagerViewModel<bool?, int>()));

        /// <summary>
        /// Identifies the <see cref="ItemsLoadOp"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsLoadOpProperty = ItemsLoadOpPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public AsyncOps.AsyncOpResultManagerViewModel<bool?, int> ItemsLoadOp { get => (AsyncOps.AsyncOpResultManagerViewModel<bool?, int>)GetValue(ItemsLoadOpProperty); private set => SetValue(ItemsLoadOpPropertyKey, value); }

        #endregion

        internal Task<int> LoadAsync(bool? isInactive)
        {
            AsyncOps.AsyncFuncOpViewModel<bool?, int> bgOp = BgOps.FromAsync("Loading items", "Connecting to database...", isInactive, ItemsLoadOp, LoadItemsAsync);
            return bgOp.GetTask();
        }

        private async Task<int> LoadItemsAsync(bool? isInactive, AsyncOps.IStatusListener<bool?> statusListener)
        {
            statusListener.CancellationToken.ThrowIfCancellationRequested();
            IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            IQueryable<SymbolicName> items;
            if (isInactive.HasValue)
            {
                if (isInactive.Value)
                    items = from s in dbContext.SymbolicNames where s.IsInactive select s;
                else
                    items = from s in dbContext.SymbolicNames where !s.IsInactive select s;
            }
            else
                items = from s in dbContext.SymbolicNames select s;
            return await OnEntitiesLoaded(items, statusListener);
        }

        protected override SymbolicNameItemVM CreateItem(SymbolicName entity) => new(entity);

        protected override DbSet<SymbolicName> GetDbSet(LocalDbContext dbContext) => dbContext.SymbolicNames;

        protected override string GetDeleteProgressTitle(SymbolicNameItemVM item)
        {
            throw new NotImplementedException();
        }

        protected override string GetSaveExistingProgressTitle(SymbolicNameItemVM item)
        {
            throw new NotImplementedException();
        }

        protected override string GetSaveNewProgressTitle(SymbolicNameItemVM item)
        {
            throw new NotImplementedException();
        }

        protected override SymbolicName InitializeNewEntity()
        {
            throw new NotImplementedException();
        }

        protected override bool PromptItemDeleting(SymbolicNameItemVM item, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override bool ShowModalItemEditWindow(SymbolicNameItemVM item, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override Func<IStatusListener, Task<int>> GetItemsLoaderFactory()
        {
            throw new NotImplementedException();
        }
    }
}
