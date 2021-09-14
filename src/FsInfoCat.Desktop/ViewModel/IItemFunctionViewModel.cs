using System;

namespace FsInfoCat.Desktop.ViewModel
{
    public interface IItemFunctionViewModel : IDbEntityRowViewModel
    {
        /// <summary>
        /// Gets the item invocation state object.
        /// </summary>
        /// <value>
        /// The state object supplied by the item function invocation source.
        /// </value>
        object InvocationState { get; }

        /// <summary>
        /// Occurs when item function is completed.
        /// </summary>
        event EventHandler<ItemFunctionResultEventArgs> Completed;
    }

    public interface IItemFunctionViewModel<TEntity> : IItemFunctionViewModel, IDbEntityRowViewModel<TEntity> where TEntity : DbEntity { }
}
