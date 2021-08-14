using System;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    /// <summary>
    /// Tracks <see cref="Func{TState, TListener, TResult}"/> delegates asynchronously executed as background operations.
    /// <para>Extends <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}" />.</para>
    /// </summary>
    /// <typeparam name="TState">The type of the state object associated with the background <see cref="Task{TResult}"/>.</typeparam>
    /// <typeparam name="TItem">The type of item that inherits from <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}.AsyncOpViewModel"/>,
    /// containing the status and results of the background operation.</typeparam>
    /// <typeparam name="TListener">The type of listener that inherits from <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}.AsyncOpViewModel.StatusListener" />
    /// and is used within the background <see cref="Task{TResult}"/> to update the associated <typeparamref name="TItem">Item</typeparamref>.</typeparam>
    /// <typeparam name="TResult">The type of the result value produced by the <see cref="Func{TListener, TResult}"/> delegate.</typeparam>
    /// <seealso cref="AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}" />
    public class AsyncOpResultManagerViewModel<TState, TItem, TListener, TResult> : AsyncOpManagerViewModel<TState, Task<TResult>, TItem, TListener>
        where TItem : AsyncOpResultManagerViewModel<TState, TItem, TListener, TResult>.AsyncOpViewModel
        where TListener : AsyncOpResultManagerViewModel<TState, TItem, TListener, TResult>.AsyncOpViewModel.StatusListener
    {
        /// <summary>
        /// Occurs when the value of the <see cref="LastResult"/> property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler LastResultPropertyChanged;

        private static readonly DependencyPropertyKey LastResultPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastResult), typeof(TResult), typeof(AsyncOpResultManagerViewModel<TState, TItem, TListener, TResult>),
                new PropertyMetadata(default, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as AsyncOpResultManagerViewModel<TState, TItem, TListener, TResult>)?.LastResultPropertyChanged?.Invoke(d, e)));

        public static readonly DependencyProperty LastResultProperty = LastResultPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the last result value returned from a background operation.
        /// </summary>
        /// <value>The last result value returned from the background operation of a tracked <see cref="TItem"/>.</value>
        public TResult LastResult
        {
            get => (TResult)GetValue(LastResultProperty);
            private set => SetValue(LastResultPropertyKey, value);

        }
    }

    /// <summary>
    /// Tracks <see cref="Func{TListener, TResult}"/> delegates asynchronously executed as background operations.
    /// <para>Extends <see cref="AsyncOpResultManagerViewModel{object, TItem, TListener, TResult}" />.</para>
    /// </summary>
    /// <typeparam name="TItem">The type of item that inherits from <see cref="AsyncOpManagerViewModel{object, Task{TResult}, TItem, TListener}.AsyncOpViewModel"/>,
    /// containing the status and results of the background operation.</typeparam>
    /// <typeparam name="TListener">The type of listener that inherits from <see cref="AsyncOpManagerViewModel{object, Task{TResult}, TItem, TListener}.AsyncOpViewModel.StatusListener" />
    /// and is used within the background <see cref="Task{TResult}"/> to update the associated <typeparamref name="TItem">Item</typeparamref>.</typeparam>
    /// <typeparam name="TResult">The type of the result value produced by the <see cref="Func{TListener, TResult}"/> delegate.</typeparam>
    /// <seealso cref="AsyncOpResultManagerViewModel{object, TItem, TListener, TResult}" />
    public class AsyncOpResultManagerViewModel<TItem, TListener, TResult> : AsyncOpResultManagerViewModel<object, TItem, TListener, TResult>
        where TItem : AsyncOpResultManagerViewModel<TItem, TListener, TResult>.AsyncOpViewModel
        where TListener : AsyncOpResultManagerViewModel<TItem, TListener, TResult>.AsyncOpViewModel.StatusListener
    {
    }

    /// <summary>
    /// Tracks <see cref="Func{AsyncFuncOpViewModel{TState, TResult}.StatusListenerImpl, TResult}"/> delegates asynchronously executed as background operations.
    /// <para>Extends <see cref="AsyncOpResultManagerViewModel{TState, AsyncFuncOpViewModel{TState, TResult}, AsyncFuncOpViewModel{TState, TResult}.StatusListenerImpl, TResult}" />.</para>
    /// </summary>
    /// <typeparam name="TState">The type of the state object associated with the background <see cref="Task{TResult}"/>.</typeparam>
    /// <typeparam name="TResult">The type of the result value produced by the <see cref="Func{AsyncFuncOpViewModel{TState, TResult}.StatusListenerImpl, TResult}"/> delegate.</typeparam>
    /// <seealso cref="AsyncOpResultManagerViewModel{TState, AsyncFuncOpViewModel{TState, TResult}, AsyncFuncOpViewModel{TState, TResult}.StatusListenerImpl, TResult}" />
    public class AsyncOpResultManagerViewModel<TState, TResult> : AsyncOpResultManagerViewModel<TState, AsyncFuncOpViewModel<TState, TResult>, AsyncFuncOpViewModel<TState, TResult>.StatusListenerImpl, TResult>
    {
        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="initialState">The initial value of the <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, AsyncFuncOpViewModel{TState, TResult}, AsyncFuncOpViewModel{TState, TResult}.StatusListenerImpl}.AsyncOpViewModel.State">AsyncOpResultManagerViewModel&lt;TState, TResult&gt;.AsyncOpViewModel.State</see> property.</param>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <param name="creationOptions">The backround task creation options.</param>
        /// <param name="scheduler">The optional scheduler to use. The default is the <see cref="TaskScheduler.Default">instance provided by the .NET framework</see>.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TState, TResult}"/> object representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> was null.</exception>
        public AsyncFuncOpViewModel<TState, TResult> StartNewBgOperation(TState initialState, string initialMessage, Func<TState, AsyncFuncOpViewModel<TState, TResult>.StatusListenerImpl, TResult> func, TaskCreationOptions creationOptions, Guid? concurrencyId = null, TaskScheduler scheduler = null) =>
            AsyncFuncOpViewModel<TState, TResult>.StartNew(initialState, initialMessage, this, func, creationOptions, concurrencyId, scheduler);

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="initialState">The initial value of the <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, AsyncFuncOpViewModel{TState, TResult}, AsyncFuncOpViewModel{TState, TResult}.StatusListenerImpl}.AsyncOpViewModel.State">AsyncOpResultManagerViewModel&lt;TState, TResult&gt;.AsyncOpViewModel.State</see> property.</param>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TState, TResult}"/> object representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> was null.</exception>
        public AsyncFuncOpViewModel<TState, TResult> StartNewBgOperation(TState initialState, string initialMessage, Func<TState, AsyncFuncOpViewModel<TState, TResult>.StatusListenerImpl, TResult> func, Guid? concurrencyId = null) =>
            AsyncFuncOpViewModel<TState, TResult>.StartNew(initialState, initialMessage, this, func, concurrencyId);

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="initialState">The initial value of the <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, AsyncFuncOpViewModel{TState, TResult}, AsyncFuncOpViewModel{TState, TResult}.StatusListenerImpl}.AsyncOpViewModel.State">AsyncOpResultManagerViewModel&lt;TState, TResult&gt;.AsyncOpViewModel.State</see> property.</param>
        /// <param name="func">The asynchronous delegate that will implement the background operation and produce the result value.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TState, TResult}"/> object representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> was null.</exception>
        public AsyncFuncOpViewModel<TState, TResult> StartBgOperationFromAsync(TState initialState, string initialMessage, Func<TState, AsyncFuncOpViewModel<TState, TResult>.StatusListenerImpl, Task<TResult>> func, Guid? concurrencyId = null) =>
            AsyncFuncOpViewModel<TState, TResult>.FromAsync(initialState, initialMessage, this, func, concurrencyId);

        /// <summary>
        /// Adds a pending background operation.
        /// </summary>
        /// <param name="initialState">The initial value of the <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, AsyncFuncOpViewModel{TState, TResult}, AsyncFuncOpViewModel{TState, TResult}.StatusListenerImpl}.AsyncOpViewModel.State">AsyncOpResultManagerViewModel&lt;TState, TResult&gt;.AsyncOpViewModel.State</see> property.</param>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <param name="creationOptions">The backround task creation options.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TState, TResult}"/> object representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> was null.</exception>
        public AsyncFuncOpViewModel<TState, TResult> AddPendingBgOperation(TState initialState, string initialMessage, Func<TState, AsyncFuncOpViewModel<TState, TResult>.StatusListenerImpl, TResult> func, TaskCreationOptions creationOptions, Guid? concurrencyId = null) =>
            AsyncFuncOpViewModel<TState, TResult>.AddPending(initialState, initialMessage, this, func, creationOptions, concurrencyId);

        /// <summary>
        /// Adds a pending background operation.
        /// </summary>
        /// <param name="initialState">The initial value of the <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, AsyncFuncOpViewModel{TState, TResult}, AsyncFuncOpViewModel{TState, TResult}.StatusListenerImpl}.AsyncOpViewModel.State">AsyncOpResultManagerViewModel&lt;TState, TResult&gt;.AsyncOpViewModel.State</see> property.</param>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TState, TResult}"/> object representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> was null.</exception>
        public AsyncFuncOpViewModel<TState, TResult> AddPendingBgOperation(TState initialState, string initialMessage, Func<TState, AsyncFuncOpViewModel<TState, TResult>.StatusListenerImpl, TResult> func, Guid? concurrencyId = null) =>
            AsyncFuncOpViewModel<TState, TResult>.AddPending(initialState, initialMessage, this, func, concurrencyId);
    }

    /// <summary>
    /// Tracks <see cref="Func{AsyncFuncOpViewModel{TResult}.StatusListenerImpl, TResult}"/> delegates asynchronously executed as background operations.
    /// Class AsyncOpResultManagerViewModel.
    /// Implements the <see cref="AsyncOpResultManagerViewModel{AsyncFuncOpViewModel{TResult}, AsyncFuncOpViewModel{TResult}.StatusListenerImpl, TResult}" />
    /// </summary>
    /// <typeparam name="TResult">The type of the result value produced by the <see cref="Func{AsyncFuncOpViewModel{TResult}.StatusListenerImpl, TResult}"/> delegate.</typeparam>
    /// <seealso cref="AsyncOpResultManagerViewModel{AsyncFuncOpViewModel{TResult}, AsyncFuncOpViewModel{TResult}.StatusListenerImpl, TResult}" />
    public class AsyncOpResultManagerViewModel<TResult> : AsyncOpResultManagerViewModel<AsyncFuncOpViewModel<TResult>, AsyncFuncOpViewModel<TResult>.StatusListenerImpl, TResult>
    {
        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <param name="creationOptions">The backround task creation options.</param>
        /// <param name="scheduler">The optional scheduler to use. The default is the <see cref="TaskScheduler.Default">instance provided by the .NET framework</see>.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TResult}"/> object representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> was null.</exception>
        public AsyncFuncOpViewModel<TResult> StartNewBgOperation(string initialMessage, Func<AsyncFuncOpViewModel<TResult>.StatusListenerImpl, TResult> func, TaskCreationOptions creationOptions, Guid? concurrencyId = null, TaskScheduler scheduler = null) =>
            AsyncFuncOpViewModel<TResult>.StartNew(initialMessage, this, func, creationOptions, concurrencyId, scheduler);

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TResult}"/> object representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> was null.</exception>
        public AsyncFuncOpViewModel<TResult> StartNewBgOperation(string initialMessage, Func<AsyncFuncOpViewModel<TResult>.StatusListenerImpl, TResult> func, Guid? concurrencyId = null) =>
            AsyncFuncOpViewModel<TResult>.StartNew(initialMessage, this, func, concurrencyId);

        /// <summary>
        /// Starts the bg operation from asynchronous.
        /// </summary>
        /// <param name="func">The asynchronous delegate that will implement the background operation and produce the result value.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TResult}"/> object representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> was null.</exception>
        public AsyncFuncOpViewModel<TResult> StartBgOperationFromAsync(string initialMessage, Func<AsyncFuncOpViewModel<TResult>.StatusListenerImpl, Task<TResult>> func, Guid? concurrencyId = null) =>
            AsyncFuncOpViewModel<TResult>.FromAsync(initialMessage, this, func, concurrencyId);

        /// <summary>
        /// Adds a pending background operation.
        /// </summary>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <param name="creationOptions">The backround task creation options.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TResult}"/> object representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> was null.</exception>
        public AsyncFuncOpViewModel<TResult> AddPendingBgOperation(string initialMessage, Func<AsyncFuncOpViewModel<TResult>.StatusListenerImpl, TResult> func, TaskCreationOptions creationOptions, Guid? concurrencyId = null) =>
            AsyncFuncOpViewModel<TResult>.AddPending(initialMessage, this, func, creationOptions, concurrencyId);

        /// <summary>
        /// Adds a pending background operation.
        /// </summary>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TResult}"/> object representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="func"/> was null.</exception>
        public AsyncFuncOpViewModel<TResult> AddPendingBgOperation(string initialMessage, Func<AsyncFuncOpViewModel<TResult>.StatusListenerImpl, TResult> func, Guid? concurrencyId = null) =>
            AsyncFuncOpViewModel<TResult>.AddPending(initialMessage, this, func, concurrencyId);
    }
}
