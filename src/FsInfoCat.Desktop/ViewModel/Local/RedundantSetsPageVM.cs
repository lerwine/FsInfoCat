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
    public class RedundantSetsPageVM : DbEntityListingPageVM<RedundantSet, RedundantSetItemVM>
    {
        #region ItemsLoadOp Property Members

        private static readonly DependencyPropertyKey ItemsLoadOpPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ItemsLoadOp), typeof(AsyncOps.AsyncOpResultManagerViewModel<ItemLoadParams, int>), typeof(RedundantSetsPageVM),
                new PropertyMetadata(new AsyncOps.AsyncOpResultManagerViewModel<ItemLoadParams, int>()));

        /// <summary>
        /// Identifies the <see cref="ItemsLoadOp"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsLoadOpProperty = ItemsLoadOpPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public AsyncOps.AsyncOpResultManagerViewModel<ItemLoadParams, int> ItemsLoadOp { get => (AsyncOps.AsyncOpResultManagerViewModel<ItemLoadParams, int>)GetValue(ItemsLoadOpProperty); private set => SetValue(ItemsLoadOpPropertyKey, value); }

        #endregion

        internal Task<int> LoadAsync(Guid? binaryPropertiesId, string reference = null)
        {
            AsyncOps.AsyncFuncOpViewModel<ItemLoadParams, int> bgOp = BgOps.FromAsync("Loading items", "Connecting to database...",
                new(binaryPropertiesId, reference), ItemsLoadOp, LoadItemsAsync);
            return bgOp.GetTask();
        }

        private async Task<int> LoadItemsAsync(ItemLoadParams state, AsyncOps.IStatusListener<ItemLoadParams> statusListener)
        {
            statusListener.CancellationToken.ThrowIfCancellationRequested();
            IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            IIncludableQueryable<RedundantSet, BinaryPropertySet> redundantSets = dbContext.RedundantSets.Include(v => v.BinaryProperties);
            IQueryable<RedundantSet> items;
            string reference = state.Reference.AsWsNormalizedOrEmpty();
            if (state.BinaryPropertiesId.HasValue)
            {
                Guid id = state.BinaryPropertiesId.Value;
                if (reference.Length > 0)
                    items = from r in redundantSets where r.BinaryPropertiesId == id && r.Reference == reference select r;
                else
                    items = from r in redundantSets where r.BinaryPropertiesId == id select r;
            }
            else if (reference.Length > 0)
                items = from r in redundantSets where r.Reference == reference select r;
            else
                items = from r in redundantSets select r;
            return await OnEntitiesLoaded(items, statusListener);
        }

        protected override RedundantSetItemVM CreateItem(RedundantSet entity) => new(entity);

        protected override DbSet<RedundantSet> GetDbSet(LocalDbContext dbContext) => dbContext.RedundantSets;

        protected override string GetDeleteProgressTitle(RedundantSetItemVM item)
        {
            throw new NotImplementedException();
        }

        protected override string GetSaveExistingProgressTitle(RedundantSetItemVM item)
        {
            throw new NotImplementedException();
        }

        protected override string GetSaveNewProgressTitle(RedundantSetItemVM item)
        {
            throw new NotImplementedException();
        }

        protected override RedundantSet InitializeNewEntity()
        {
            throw new NotImplementedException();
        }

        protected override bool PromptItemDeleting(RedundantSetItemVM item, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override bool ShowModalItemEditWindow(RedundantSetItemVM item, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override Func<IStatusListener, Task<int>> GetItemsLoaderFactory()
        {
            throw new NotImplementedException();
        }

        public record ItemLoadParams(Guid? BinaryPropertiesId, string Reference);
    }
}