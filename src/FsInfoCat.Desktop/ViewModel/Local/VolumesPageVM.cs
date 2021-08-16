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
        #region ItemsLoadOp Property Members

        private static readonly DependencyPropertyKey ItemsLoadOpPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ItemsLoadOp), typeof(AsyncOps.AsyncOpResultManagerViewModel<ItemLoadParams, int>), typeof(VolumesPageVM),
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

        internal Task<int> LoadAsync(Guid? fileSystemId, VolumeStatus? statusValue, bool statusNotEquals = false)
        {
            AsyncOps.AsyncFuncOpViewModel<ItemLoadParams, int> bgOp = BgOps.FromAsync("Loading items", "Connecting to database...",
                new(fileSystemId, statusValue, statusNotEquals), ItemsLoadOp, LoadItemsAsync);
            return bgOp.GetTask();
        }

        private async Task<int> LoadItemsAsync(ItemLoadParams state, AsyncOps.IStatusListener<ItemLoadParams> statusListener)
        {
            statusListener.CancellationToken.ThrowIfCancellationRequested();
            IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            IIncludableQueryable<Volume, FileSystem> volumes = dbContext.Volumes.Include(v => v.FileSystem);
            IQueryable<Volume> items;
            if (state.FileSystemId.HasValue)
            {
                Guid id = state.FileSystemId.Value;

                if (state.StatusValue.HasValue)
                {
                    VolumeStatus status = state.StatusValue.Value;
                    if (state.StatusNotEquals)
                        items = from v in volumes where v.FileSystemId == id && v.Status != status select v;
                    else
                        items = from v in volumes where v.FileSystemId == id && v.Status == status select v;
                }
                else
                    items = from v in volumes where v.FileSystemId == id select v;
            }
            else if (state.StatusValue.HasValue)
            {
                VolumeStatus status = state.StatusValue.Value;
                if (state.StatusNotEquals)
                    items = from v in volumes where v.Status != status select v;
                else
                    items = from v in volumes where v.Status == status select v;
            }
            else
                items = from v in volumes select v;
            return await OnEntitiesLoaded(items, statusListener);
        }

        protected override VolumeItemVM CreateItem(Volume entity) => new(entity);

        protected override DbSet<Volume> GetDbSet(LocalDbContext dbContext) => dbContext.Volumes;

        protected override string GetSaveNewProgressTitle(VolumeItemVM item) =>
            string.Format(FsInfoCat.Properties.Resources.FormatMessage_AddingNewVolume, item.DisplayName);

        protected override string GetSaveExistingProgressTitle(VolumeItemVM item) =>
            string.Format(FsInfoCat.Properties.Resources.FormatMessage_SavingVolumeChanges, item.DisplayName);

        protected override string GetDeleteProgressTitle(VolumeItemVM item) =>
            string.Format(FsInfoCat.Properties.Resources.FormatMessage_DeletingVolume, item.DisplayName);

        protected override Volume InitializeNewEntity() => new()
        {
            Id = Guid.NewGuid(),
            CreatedOn = DateTime.Now
        };

        protected override bool PromptItemDeleting(VolumeItemVM item, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override bool ShowModalItemEditWindow(VolumeItemVM item, object parameter)
        {
            throw new NotImplementedException();
        }

        public record ItemLoadParams(Guid? FileSystemId, VolumeStatus? StatusValue, bool StatusNotEquals);
    }
}
