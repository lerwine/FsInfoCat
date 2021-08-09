using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    /// <summary>
    /// View model which indicates the status of a background operation implemented through a <c><see cref="Func{TState, AsyncFuncOpViewModel{TState, TResult}.StatusListenerImpl, TResult}"/>. 
    /// <para>Extends <see cref="AsyncFuncOpViewModelBase{TState, TResult, AsyncFuncOpViewModel{TState, TResult}.StatusListenerImpl, AsyncFuncOpViewModel{TState, TResult}}" />.</para>
    /// </summary>
    /// <typeparam name="TState">The type of the state object associated with the background <see cref="Task{TResult}"/>.</typeparam>
    /// <typeparam name="TResult">The type of the result value produced by the background operation.</typeparam>
    /// <seealso cref="AsyncFuncOpViewModelBase{TState, TResult, AsyncFuncOpViewModel{TState, TResult}.StatusListenerImpl, AsyncFuncOpViewModel{TState, TResult}}" />
    public partial class AsyncFuncOpViewModel<TState, TResult> : AsyncFuncOpViewModelBase<TState, TResult, AsyncFuncOpViewModel<TState, TResult>.StatusListenerImpl, AsyncFuncOpViewModel<TState, TResult>>
    {
        private readonly StatusListenerImpl _statusListener;

        /// <summary>
        /// Gets the listener that can be used to monitor for cancellation and to update the current view model item.
        /// </summary>
        /// <returns>The <see cref="StatusListenerImpl"/> that can be used to monitor for cancellation and to update the current <see cref="AsyncFuncOpViewModel{TState, TResult}"/>.</returns>
        internal override StatusListenerImpl GetStatusListener() => _statusListener;

        private AsyncFuncOpViewModel(TState initialState, Func<StatusListenerImpl, Task<TResult>> createTask)
            : base(initialState, createTask)
        {
            _statusListener = new StatusListenerImpl(this);
        }

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="initialState">The initial value for the <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}.AsyncOpViewModel.State"/> property.</param>
        /// <param name="operationManager">The <see cref="AsyncOpResultManagerViewModel{TState, AsyncFuncOpViewModel{TState, TResult}, StatusListenerImpl, TResult}"/> that will track the background operation.</param>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <param name="creationOptions">The backround task creation options.</param>
        /// <param name="scheduler">The optional scheduler to use. The default is the <see cref="TaskScheduler.Default">instance provided by the .NET framework</see>.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TState, TResult}"/> object representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="func"/> was null.</exception>
        public static AsyncFuncOpViewModel<TState, TResult> StartNew(TState initialState, [DisallowNull] AsyncOpResultManagerViewModel<TState, AsyncFuncOpViewModel<TState, TResult>, StatusListenerImpl, TResult> operationManager,
            [DisallowNull] Func<TState, StatusListenerImpl, TResult> func, TaskCreationOptions creationOptions, TaskScheduler scheduler = null)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            AsyncFuncOpViewModel<TState, TResult> item = new(initialState, sl => Task.Factory.StartNew(s =>
            {
                sl.RaiseTaskStarted();
                return func((TState)s, sl);
            }, initialState, sl.CancellationToken, creationOptions, scheduler ?? TaskScheduler.Default));
            Add(operationManager, item);
            return item;
        }

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="initialState">The initial value for the <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}.AsyncOpViewModel.State"/> property.</param>
        /// <param name="operationManager">The <see cref="AsyncOpResultManagerViewModel{TState, AsyncFuncOpViewModel{TState, TResult}, StatusListenerImpl, TResult}"/> that will track the background operation.</param>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <returns>FsInfoCat.Desktop.ViewModel.AsyncOps.AsyncFuncOpViewModel&lt;TState, TResult&gt;.</returns>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TState, TResult}"/> object representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="func"/> was null.</exception>
        public static AsyncFuncOpViewModel<TState, TResult> StartNew(TState initialState, [DisallowNull] AsyncOpResultManagerViewModel<TState, AsyncFuncOpViewModel<TState, TResult>, StatusListenerImpl, TResult> operationManager,
            [DisallowNull] Func<TState, StatusListenerImpl, TResult> func)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            AsyncFuncOpViewModel<TState, TResult> item = new(initialState, sl => Task.Factory.StartNew(s =>
            {
                sl.RaiseTaskStarted();
                return func((TState)s, sl);
            }, initialState, sl.CancellationToken));
            Add(operationManager, item);
            return item;
        }

        /// <summary>
        /// Adds a new pending background operation.
        /// </summary>
        /// <param name="initialState">The initial value for the <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}.AsyncOpViewModel.State"/> property.</param>
        /// <param name="operationManager">The <see cref="AsyncOpResultManagerViewModel{TState, AsyncFuncOpViewModel{TState, TResult}, StatusListenerImpl, TResult}"/> that will track the background operation.</param>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <param name="creationOptions">The backround task creation options.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TState, TResult}"/> object representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="func"/> was null.</exception>
        public static AsyncFuncOpViewModel<TState, TResult> AddNew(TState initialState, [DisallowNull] AsyncOpResultManagerViewModel<TState, AsyncFuncOpViewModel<TState, TResult>, StatusListenerImpl, TResult> operationManager,
            [DisallowNull] Func<TState, StatusListenerImpl, TResult> func, TaskCreationOptions creationOptions)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            AsyncFuncOpViewModel<TState, TResult> item = new(initialState, sl => new Task<TResult>(s =>
            {
                sl.RaiseTaskStarted();
                return func((TState)s, sl);
            }, initialState, sl.CancellationToken, creationOptions));
            Add(operationManager, item);
            return item;
        }

        /// <summary>
        /// Adds a new pending background operation.
        /// </summary>
        /// <param name="initialState">The initial value for the <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}.AsyncOpViewModel.State"/> property.</param>
        /// <param name="operationManager">The <see cref="AsyncOpResultManagerViewModel{TState, AsyncFuncOpViewModel{TState, TResult}, StatusListenerImpl, TResult}"/> that will track the background operation.</param>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TState, TResult}"/> object representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="func"/> was null.</exception>
        public static AsyncFuncOpViewModel<TState, TResult> AddNew(TState initialState, [DisallowNull] AsyncOpResultManagerViewModel<TState, AsyncFuncOpViewModel<TState, TResult>, StatusListenerImpl, TResult> operationManager,
            [DisallowNull] Func<TState, StatusListenerImpl, TResult> func)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            AsyncFuncOpViewModel<TState, TResult> item = new(initialState, sl => new Task<TResult>(s =>
            {
                sl.RaiseTaskStarted();
                return func((TState)s, sl);
            }, initialState, sl.CancellationToken));
            Add(operationManager, item);
            return item;
        }
    }

    /// <summary>
    /// View model which indicates the status of a background operation implemented through a <see cref="Func{AsyncFuncOpViewModel{TResult}.StatusListenerImpl, TResult}"/>. 
    /// <para>Extends <see cref="AsyncFuncOpViewModelBase{object, TResult, AsyncFuncOpViewModel{TResult}.StatusListenerImpl, AsyncFuncOpViewModel{TResult}}" />.</para>
    /// </summary>
    /// <typeparam name="TResult">The type of the result value produced by the background operation.</typeparam>
    /// <seealso cref="AsyncFuncOpViewModelBase{object, TResult, AsyncFuncOpViewModel{TResult}.StatusListenerImpl, AsyncFuncOpViewModel{TResult}}" />
    public partial class AsyncFuncOpViewModel<TResult> : AsyncFuncOpViewModelBase<object, TResult, AsyncFuncOpViewModel<TResult>.StatusListenerImpl, AsyncFuncOpViewModel<TResult>>
    {
        private readonly StatusListenerImpl _statusListener;

        /// <summary>
        /// Gets the listener that can be used to monitor for cancellation and to update the current view model item.
        /// </summary>
        /// <returns>The <see cref="StatusListenerImpl"/> that can be used to monitor for cancellation and to update the current <see cref="AsyncFuncOpViewModel{TResult}"/>.</returns>
        internal override StatusListenerImpl GetStatusListener() => _statusListener;

        private AsyncFuncOpViewModel(Func<StatusListenerImpl, Task<TResult>> createTask)
            : base(null, createTask)
        {
            _statusListener = new StatusListenerImpl(this);
        }

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="operationManager">The <see cref="AsyncOpResultManagerViewModel{AsyncFuncOpViewModel{TResult}, StatusListenerImpl, TResult}"/> that will track the background operation.</param>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <param name="creationOptions">The backround task creation options.</param>
        /// <param name="scheduler">The optional scheduler to use. The default is the <see cref="TaskScheduler.Default">instance provided by the .NET framework</see>.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TResult}"/> object representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="func"/> was null.</exception>
        public static AsyncFuncOpViewModel<TResult> StartNew([DisallowNull] AsyncOpResultManagerViewModel<AsyncFuncOpViewModel<TResult>, StatusListenerImpl, TResult> operationManager,
            [DisallowNull] Func<StatusListenerImpl, TResult> func, TaskCreationOptions creationOptions, TaskScheduler scheduler = null)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            AsyncFuncOpViewModel<TResult> item = new(sl => Task.Factory.StartNew(() =>
            {
                sl.RaiseTaskStarted();
                return func(sl);
            }, sl.CancellationToken, creationOptions, scheduler ?? TaskScheduler.Default));
            Add(operationManager, item);
            return item;
        }

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="operationManager">The <see cref="AsyncOpResultManagerViewModel{AsyncFuncOpViewModel{TResult}, StatusListenerImpl, TResult}"/> that will track the background operation.</param>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TResult}"/> object representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="func"/> was null.</exception>
        public static AsyncFuncOpViewModel<TResult> StartNew([DisallowNull] AsyncOpResultManagerViewModel<AsyncFuncOpViewModel<TResult>, StatusListenerImpl, TResult> operationManager,
            [DisallowNull] Func<StatusListenerImpl, TResult> func)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            AsyncFuncOpViewModel<TResult> item = new(sl => Task.Factory.StartNew(() =>
            {
                sl.RaiseTaskStarted();
                return func(sl);
            }, sl.CancellationToken));
            Add(operationManager, item);
            return item;
        }

        /// <summary>
        /// Adds a new pending background operation.
        /// </summary>
        /// <param name="operationManager">The <see cref="AsyncOpResultManagerViewModel{AsyncFuncOpViewModel{TResult}, StatusListenerImpl, TResult}"/> that will track the background operation.</param>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <param name="creationOptions">The backround task creation options.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TResult}"/> object representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="func"/> was null.</exception>
        public static AsyncFuncOpViewModel<TResult> AddNew([DisallowNull] AsyncOpResultManagerViewModel<AsyncFuncOpViewModel<TResult>, StatusListenerImpl, TResult> operationManager,
            [DisallowNull] Func<StatusListenerImpl, TResult> func, TaskCreationOptions creationOptions)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            AsyncFuncOpViewModel<TResult> item = new(sl => new Task<TResult>(() =>
            {
                sl.RaiseTaskStarted();
                return func(sl);
            }, sl.CancellationToken, creationOptions));
            Add(operationManager, item);
            return item;
        }

        /// <summary>
        /// Adds a new pending background operation.
        /// </summary>
        /// <param name="operationManager">The <see cref="AsyncOpResultManagerViewModel{AsyncFuncOpViewModel{TResult}, StatusListenerImpl, TResult}"/> that will track the background operation.</param>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TResult}"/> object representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="func"/> was null.</exception>
        public static AsyncFuncOpViewModel<TResult> AddNew([DisallowNull] AsyncOpResultManagerViewModel<AsyncFuncOpViewModel<TResult>, StatusListenerImpl, TResult> operationManager,
            [DisallowNull] Func<StatusListenerImpl, TResult> func)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            AsyncFuncOpViewModel<TResult> item = new(sl => new Task<TResult>(() =>
            {
                sl.RaiseTaskStarted();
                return func(sl);
            }, sl.CancellationToken));
            Add(operationManager, item);
            return item;
        }
    }
}
