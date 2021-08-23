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
    public class RedundantSetsPageVM : DbEntityListingPageVM<RedundantSetListItem, RedundantSetItemVM>
    {
        internal Task<int> LoadAsync(Guid? binaryPropertiesId, string reference = null)
        {
            return BgOps.FromAsync("Loading items", "Connecting to database...", new ItemLoadParams(binaryPropertiesId, reference), LoadItemsAsync);
        }

        private async Task<int> LoadItemsAsync(ItemLoadParams state, IWindowsStatusListener statusListener)
        {
            statusListener.CancellationToken.ThrowIfCancellationRequested();
            IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            IQueryable<RedundantSetListItem> items;
            string reference = state.Reference.AsWsNormalizedOrEmpty();
            if (state.BinaryPropertiesId.HasValue)
            {
                Guid id = state.BinaryPropertiesId.Value;
                if (reference.Length > 0)
                    items = from r in dbContext.RedundantSetListing where r.BinaryPropertiesId == id && r.Reference == reference select r;
                else
                    items = from r in dbContext.RedundantSetListing where r.BinaryPropertiesId == id select r;
            }
            else if (reference.Length > 0)
                items = from r in dbContext.RedundantSetListing where r.Reference == reference select r;
            else
                items = from r in dbContext.RedundantSetListing select r;
            return await OnEntitiesLoaded(items, statusListener, entity => new RedundantSetItemVM(entity));
        }

        protected override DbSet<RedundantSetListItem> GetDbSet(LocalDbContext dbContext) => dbContext.RedundantSetListing;

        protected override Func<IWindowsStatusListener, Task<int>> GetItemsLoaderFactory()
        {
            throw new NotImplementedException();
        }

        protected override void OnAddNewItem(object parameter)
        {
            throw new NotImplementedException();
        }

        protected override bool ShowModalItemEditWindow(RedundantSetItemVM item, object parameter, out string saveProgressTitle)
        {
            throw new NotImplementedException();
        }

        protected override bool PromptItemDeleting(RedundantSetItemVM item, object parameter, out string deleteProgressTitle)
        {
            throw new NotImplementedException();
        }

        public record ItemLoadParams(Guid? BinaryPropertiesId, string Reference);
    }
}
