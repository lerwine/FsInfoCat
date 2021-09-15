using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Desktop.ViewModel
{
    public class CrawlJobLogEditViewModel<TEntity, TCrawlConfigEntity, TCrawlConfigViewModel> : CrawlJobRowViewModel<TEntity>, IItemFunctionViewModel<TEntity>
        where TEntity : DbEntity, ICrawlJobLog
        where TCrawlConfigEntity : DbEntity, ICrawlConfigurationListItem
        where TCrawlConfigViewModel : CrawlConfigListItemViewModel<TCrawlConfigEntity>
    {
        #region Completed Event Members

        public event EventHandler<ItemFunctionResultEventArgs> Completed;

        internal object InvocationState { get; }

        object IItemFunctionViewModel.InvocationState => InvocationState;

        protected virtual void OnItemFunctionResult(ItemFunctionResultEventArgs args) => Completed?.Invoke(this, args);

        protected void RaiseItemFunctionResult(ItemFunctionResult result) => OnItemFunctionResult(new(result, InvocationState));

        #endregion

        public CrawlJobLogEditViewModel([DisallowNull] TEntity entity, object state = null) : base(entity)
        {
            InvocationState = state;
            // TODO: Implement CrawlJobLogEditViewModel
        }

        public TCrawlConfigEntity Configuration => throw new NotImplementedException();

        public ushort MaxRecursionDepth => throw new NotImplementedException();

        public ulong? MaxTotalItems => throw new NotImplementedException();

        public TimeSpanViewModel TTL => throw new NotImplementedException();
    }
}
