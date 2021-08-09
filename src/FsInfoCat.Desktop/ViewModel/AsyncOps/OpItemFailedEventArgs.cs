using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    /// <summary>
    /// Eevnt arguments for the <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}.OperationFailed">OperationFailed</see> event.
    /// <para>Extends <see cref="OpItemEventArgs{TState, TTask, TItem, TListener}" />.</para>
    /// </summary>
    /// <typeparam name="TState">The type of the state object associated with the background <see cref="Task"/>.</typeparam>
    /// <typeparam name="TTask">The type of <see cref="Task"/> executed as the background operation.</typeparam>
    /// <typeparam name="TItem">The type of <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}.AsyncOpViewModel">item</see> that contains the status and results of the background operation.</typeparam>
    /// <typeparam name="TListener">The type of the <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}.AsyncOpViewModel.StatusListener">listener</see> used within the background <typeparamref name="TTask">Task</typeparamref> to update the associated <typeparamref name="TItem">Item</typeparamref>.</typeparam>
    /// <seealso cref="OpItemEventArgs{TState, TTask, TItem, TListener}" />
    public class OpItemFailedEventArgs<TState, TTask, TItem, TListener> : OpItemEventArgs<TState, TTask, TItem, TListener>
        where TTask : Task
        where TItem : AsyncOpManagerViewModel<TState, TTask, TItem, TListener>.AsyncOpViewModel
        where TListener : AsyncOpManagerViewModel<TState, TTask, TItem, TListener>.AsyncOpViewModel.StatusListener
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpItemFailedEventArgs{TState, TTask, TItem, TListener}"/> class.
        /// </summary>
        /// <param name="item">The item representing the state of the background operation.</param>
        /// <param name="exception">The unhandled exception that was thrown during execution of the background operation.</param>
        public OpItemFailedEventArgs([DisallowNull] TItem item, [DisallowNull] AggregateException exception) : base(item)
        {
            Exception e = exception;
            while (exception.InnerExceptions.Count == 1)
            {
                if ((e = exception.InnerException) is not AggregateException a)
                    break;
                exception = a;
            }
            Exception = e;
        }

        /// <summary>
        /// Gets the exception that was thrown during execution of the background operation.
        /// </summary>
        /// <value>The exception that was thrown during execution of the background operation.</value>
        public Exception Exception { get; }
    }
}
