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
        internal Task<int> LoadAsync(bool? isInactive)
        {
            return BgOps.FromAsync("Loading items", "Connecting to database...", isInactive, LoadItemsAsync);
        }

        private async Task<int> LoadItemsAsync(bool? isInactive, AsyncOps.IStatusListener statusListener)
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
