using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    /// <summary>
    /// Class OpItemEventArgs.
    /// <para>Extends <see cref="EventArgs" />.</para>
    /// </summary>
    /// <typeparam name="TState">The type of the state object associated with the background <see cref="Task"/>.</typeparam>
    /// <typeparam name="TTask">The type of <see cref="Task"/> executed as the background operation.</typeparam>
    /// <typeparam name="TItem">The type of <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}.AsyncOpViewModel">item</see> that contains the status and results of the background operation.</typeparam>
    /// <typeparam name="TListener">The type of the <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}.AsyncOpViewModel.StatusListener">listener</see> used within the background <typeparamref name="TTask">Task</typeparamref> to update the associated <typeparamref name="TItem">Item</typeparamref>.</typeparam>
    /// <seealso cref="EventArgs" />
    public class OpItemEventArgs<TState, TTask, TItem, TListener> : EventArgs
        where TTask : Task
        where TItem : AsyncOpManagerViewModel<TState, TTask, TItem, TListener>.AsyncOpViewModel
        where TListener : AsyncOpManagerViewModel<TState, TTask, TItem, TListener>.AsyncOpViewModel.StatusListener
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpItemEventArgs{TState, TTask, TItem, TListener}"/> class.
        /// </summary>
        /// <param name="item">The item that reprsents the background operation status and results.</param>
        public OpItemEventArgs([DisallowNull] TItem item) { Item = item; }

        /// <summary>
        /// Gets the item that reprsents the background operation status and results.
        /// </summary>
        /// <value>The item that reprsents the background operation status and results.</value>
        public TItem Item { get; }
    }
}
