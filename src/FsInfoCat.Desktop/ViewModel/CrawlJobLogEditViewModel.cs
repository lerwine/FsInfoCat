using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Desktop.ViewModel
{
    public class CrawlJobLogEditViewModel<TEntity, TCrawlConfigEntity, TCrawlConfigViewModel>([DisallowNull] TEntity entity, object state = null) : CrawlJobRowViewModel<TEntity>(entity), IItemFunctionViewModel<TEntity>
        where TEntity : Model.DbEntity, Model.ICrawlJobLog
        where TCrawlConfigEntity : Model.DbEntity, Model.ICrawlConfigurationListItem
        where TCrawlConfigViewModel : CrawlConfigListItemViewModel<TCrawlConfigEntity>
    {
        #region Completed Event Members

        public event EventHandler<ItemFunctionResultEventArgs> Completed;

        internal object InvocationState { get; } = state;

        object IItemFunctionViewModel.InvocationState => InvocationState;

        protected virtual void OnItemFunctionResult(ItemFunctionResultEventArgs args) => Completed?.Invoke(this, args);

        protected void RaiseItemInsertedResult([DisallowNull] Model.DbEntity entity) => OnItemFunctionResult(new(ItemFunctionResult.Inserted, entity, InvocationState));

        protected void RaiseItemUpdatedResult() => OnItemFunctionResult(new(ItemFunctionResult.ChangesSaved, Entity, InvocationState));

        protected void RaiseItemDeletedResult() => OnItemFunctionResult(new(ItemFunctionResult.Deleted, Entity, InvocationState));

        protected void RaiseItemUnmodifiedResult() => OnItemFunctionResult(new(ItemFunctionResult.Unmodified, Entity, InvocationState));

        #endregion

        public TCrawlConfigEntity Configuration => throw new NotImplementedException("Configuration not implemented");

        public ulong? MaxTotalItems => throw new NotImplementedException("Configuration not implemented");

        public TimeSpanViewModel TTL => throw new NotImplementedException("Configuration not implemented");
    }
}
