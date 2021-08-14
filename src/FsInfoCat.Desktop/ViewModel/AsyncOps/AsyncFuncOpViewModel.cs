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
        protected AsyncFuncOpViewModel(Guid concurrencyId, TState initialState, string initialMessage, [DisallowNull] Func<StatusListenerImpl, Task<TResult>> createTask, [AllowNull] Action<StatusListenerImpl> onListenerCreated)
            : base(concurrencyId, initialMessage, new Builder(initialState, createTask), onListenerCreated) { }

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="initialState">The initial value for the <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}.AsyncOpViewModel.State"/> property.</param>
        /// <param name="operationManager">The <see cref="AsyncOpResultManagerViewModel{TState, AsyncFuncOpViewModel{TState, TResult}, StatusListenerImpl, TResult}"/>
        /// that will track the background operation.</param>
        /// <param name="onListenerCreated">The delegate that will be invoked on the UI thread after the <see cref="StatusListenerImpl">status listener</see> is created and
        /// before the <see cref="Task{TResult}"/> is created.</param>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <param name="creationOptions">The backround task creation options.</param>
        /// <param name="scheduler">The optional scheduler to use. The default is the <see cref="TaskScheduler.Default">instance provided by the .NET framework</see>.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TState, TResult}"/> object representing the status of the background operation.</returns>
        /// <remarks>Use <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}.AsyncOpViewModel.Cancel(bool)">AsyncFuncOpViewModel{TState, TResult}.Cancel(bool)</see>
        /// or <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}.AsyncOpViewModel.Cancel()">AsyncFuncOpViewModel{TState, TResult}.Cancel()</see> to cancel
        /// background operations.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="func"/> was null.</exception>
        public static AsyncFuncOpViewModel<TState, TResult> StartNew(TState initialState, string initialMessage,
            [DisallowNull] AsyncOpResultManagerViewModel<TState, AsyncFuncOpViewModel<TState, TResult>, StatusListenerImpl, TResult> operationManager,
            [AllowNull] Action<StatusListenerImpl> onListenerCreated, [DisallowNull] Func<TState, StatusListenerImpl, TResult> func, TaskCreationOptions creationOptions, Guid? concurrencyId = null, TaskScheduler scheduler = null)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            AsyncFuncOpViewModel<TState, TResult> item = new(concurrencyId ?? Guid.NewGuid(), initialState, initialMessage, sl => Task.Factory.StartNew(s =>
            {
                sl.RaiseTaskStarted();
                return func((TState)s, sl);
            }, initialState, sl.CancellationToken, creationOptions, scheduler ?? TaskScheduler.Default), onListenerCreated);
            Add(operationManager, item);
            return item;
        }

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="initialState">The initial value for the <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}.AsyncOpViewModel.State"/> property.</param>
        /// <param name="operationManager">The <see cref="AsyncOpResultManagerViewModel{TState, AsyncFuncOpViewModel{TState, TResult}, StatusListenerImpl, TResult}"/>
        /// that will track the background operation.</param>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <param name="creationOptions">The backround task creation options.</param>
        /// <param name="scheduler">The optional scheduler to use. The default is the <see cref="TaskScheduler.Default">instance provided by the .NET framework</see>.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TState, TResult}"/> object representing the status of the background operation.</returns>
        /// <remarks>Use <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}.AsyncOpViewModel.Cancel(bool)">AsyncFuncOpViewModel{TState, TResult}.Cancel(bool)</see>
        /// or <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}.AsyncOpViewModel.Cancel()">AsyncFuncOpViewModel{TState, TResult}.Cancel()</see> to cancel
        /// background operations.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="func"/> was null.</exception>
        public static AsyncFuncOpViewModel<TState, TResult> StartNew(TState initialState, string initialMessage,
            [DisallowNull] AsyncOpResultManagerViewModel<TState, AsyncFuncOpViewModel<TState, TResult>, StatusListenerImpl, TResult> operationManager,
            [DisallowNull] Func<TState, StatusListenerImpl, TResult> func, TaskCreationOptions creationOptions, Guid? concurrencyId = null, TaskScheduler scheduler = null) =>
            StartNew(initialState, initialMessage, operationManager, null, func, creationOptions, concurrencyId, scheduler);

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="initialState">The initial value for the <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}.AsyncOpViewModel.State"/> property.</param>
        /// <param name="operationManager">The <see cref="AsyncOpResultManagerViewModel{TState, AsyncFuncOpViewModel{TState, TResult}, StatusListenerImpl, TResult}"/>
        /// that will track the background operation.</param>
        /// <param name="onListenerCreated">The delegate that will be invoked on the UI thread after the <see cref="StatusListenerImpl">status listener</see> is created and
        /// before the <see cref="Task{TResult}"/> is created.</param>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TState, TResult}"/> object representing the status of the background operation.</returns>
        /// <remarks>Use <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}.AsyncOpViewModel.Cancel(bool)">AsyncFuncOpViewModel{TState, TResult}.Cancel(bool)</see>
        /// or <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}.AsyncOpViewModel.Cancel()">AsyncFuncOpViewModel{TState, TResult}.Cancel()</see> to cancel
        /// background operations.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="func"/> was null.</exception>
        public static AsyncFuncOpViewModel<TState, TResult> StartNew(TState initialState, string initialMessage,
            [DisallowNull] AsyncOpResultManagerViewModel<TState, AsyncFuncOpViewModel<TState, TResult>, StatusListenerImpl, TResult> operationManager,
            [AllowNull] Action<StatusListenerImpl> onListenerCreated, [DisallowNull] Func<TState, StatusListenerImpl, TResult> func, Guid? concurrencyId = null)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            AsyncFuncOpViewModel<TState, TResult> item = new(concurrencyId ?? Guid.NewGuid(), initialState, initialMessage, sl => Task.Factory.StartNew(s =>
            {
                sl.RaiseTaskStarted();
                return func((TState)s, sl);
            }, initialState, sl.CancellationToken), onListenerCreated);
            Add(operationManager, item);
            return item;
        }

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="initialState">The initial value for the <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}.AsyncOpViewModel.State"/> property.</param>
        /// <param name="operationManager">The <see cref="AsyncOpResultManagerViewModel{TState, AsyncFuncOpViewModel{TState, TResult}, StatusListenerImpl, TResult}"/>
        /// that will track the background operation.</param>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TState, TResult}"/> object representing the status of the background operation.</returns>
        /// <remarks>Use <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}.AsyncOpViewModel.Cancel(bool)">AsyncFuncOpViewModel{TState, TResult}.Cancel(bool)</see>
        /// or <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}.AsyncOpViewModel.Cancel()">AsyncFuncOpViewModel{TState, TResult}.Cancel()</see> to cancel
        /// background operations.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="func"/> was null.</exception>
        public static AsyncFuncOpViewModel<TState, TResult> StartNew(TState initialState, string initialMessage,
            [DisallowNull] AsyncOpResultManagerViewModel<TState, AsyncFuncOpViewModel<TState, TResult>, StatusListenerImpl, TResult> operationManager,
            [DisallowNull] Func<TState, StatusListenerImpl, TResult> func, Guid? concurrencyId = null) => StartNew(initialState, initialMessage, operationManager, null, func, concurrencyId);

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="initialState">The initial value for the <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}.AsyncOpViewModel.State" /> property.</param>
        /// <param name="operationManager">The <see cref="AsyncOpResultManagerViewModel{TState, AsyncFuncOpViewModel{TState, TResult}, StatusListenerImpl, TResult}" />
        /// that will track the background operation.</param>
        /// <param name="onListenerCreated">The delegate that will be invoked on the UI thread after the <see cref="StatusListenerImpl">status listener</see> is created and
        /// before the <see cref="Task{TResult}"/> is created.</param>
        /// <param name="func">The asynchronous delegate that will implement the background operation and produce the result value.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TState, TResult}" /> object representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException">nameof(operationManager)</exception>
        /// <remarks>Use <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}.AsyncOpViewModel.Cancel(bool)">AsyncFuncOpViewModel{TState, TResult}.Cancel(bool)</see>
        /// or <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}.AsyncOpViewModel.Cancel()">AsyncFuncOpViewModel{TState, TResult}.Cancel()</see> to cancel
        /// background operations.</remarks>
        public static AsyncFuncOpViewModel<TState, TResult> FromAsync(TState initialState, string initialMessage,
            [DisallowNull] AsyncOpResultManagerViewModel<TState, AsyncFuncOpViewModel<TState, TResult>, StatusListenerImpl, TResult> operationManager,
            [AllowNull] Action<StatusListenerImpl> onListenerCreated, [DisallowNull] Func<TState, StatusListenerImpl, Task<TResult>> func, Guid? concurrencyId = null)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            TaskProxy tp = new(initialState, func);
            AsyncFuncOpViewModel<TState, TResult> item = new(concurrencyId ?? Guid.NewGuid(), initialState, initialMessage, tp.Handler, onListenerCreated);
            Add(operationManager, item);
            return item;
        }

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="initialState">The initial value for the <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}.AsyncOpViewModel.State" /> property.</param>
        /// <param name="operationManager">The <see cref="AsyncOpResultManagerViewModel{TState, AsyncFuncOpViewModel{TState, TResult}, StatusListenerImpl, TResult}" />
        /// that will track the background operation.</param>
        /// <param name="func">The asynchronous delegate that will implement the background operation and produce the result value.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TState, TResult}" /> object representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException">nameof(operationManager)</exception>
        /// <remarks>Use <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}.AsyncOpViewModel.Cancel(bool)">AsyncFuncOpViewModel{TState, TResult}.Cancel(bool)</see>
        /// or <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}.AsyncOpViewModel.Cancel()">AsyncFuncOpViewModel{TState, TResult}.Cancel()</see> to cancel
        /// background operations.</remarks>
        public static AsyncFuncOpViewModel<TState, TResult> FromAsync(TState initialState, string initialMessage,
            [DisallowNull] AsyncOpResultManagerViewModel<TState, AsyncFuncOpViewModel<TState, TResult>, StatusListenerImpl, TResult> operationManager,
            [DisallowNull] Func<TState, StatusListenerImpl, Task<TResult>> func, Guid? concurrencyId = null) => FromAsync(initialState, initialMessage, operationManager, null, func, concurrencyId);


        /// <summary>
        /// Adds a new pending background operation.
        /// </summary>
        /// <param name="initialState">The initial value for the <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}.AsyncOpViewModel.State"/> property.</param>
        /// <param name="operationManager">The <see cref="AsyncOpResultManagerViewModel{TState, AsyncFuncOpViewModel{TState, TResult}, StatusListenerImpl, TResult}"/>
        /// that will track the background operation.</param>
        /// <param name="onListenerCreated">The delegate that will be invoked on the UI thread after the <see cref="StatusListenerImpl">status listener</see> is created and
        /// before the <see cref="Task{TResult}"/> is created.</param>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <param name="creationOptions">The backround task creation options.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TState, TResult}"/> object representing the status of the background operation.</returns>
        /// <remarks>Use <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}.StartPendingOperation(TItem, TaskScheduler)">AsyncOpManagerViewModel&lt;TState, TResult&gt;.StartPendingOperation(AsyncFuncOpViewModel&lt;TState, TResult&gt;, TaskScheduler?)</see>
        /// to start pending background operations.
        /// <para>Use <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}.AsyncOpViewModel.Cancel(bool)">AsyncFuncOpViewModel{TState, TResult}.Cancel(bool)</see>
        /// or <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}.AsyncOpViewModel.Cancel()">AsyncFuncOpViewModel{TState, TResult}.Cancel()</see> to cancel
        /// background operations.</para></remarks>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="func"/> was null.</exception>
        public static AsyncFuncOpViewModel<TState, TResult> AddPending(TState initialState, string initialMessage,
            [DisallowNull] AsyncOpResultManagerViewModel<TState, AsyncFuncOpViewModel<TState, TResult>, StatusListenerImpl, TResult> operationManager,
            [AllowNull] Action<StatusListenerImpl> onListenerCreated, [DisallowNull] Func<TState, StatusListenerImpl, TResult> func, TaskCreationOptions creationOptions, Guid? concurrencyId = null)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            AsyncFuncOpViewModel<TState, TResult> item = new(concurrencyId ?? Guid.NewGuid(), initialState, initialMessage, sl => new Task<TResult>(s =>
            {
                sl.RaiseTaskStarted();
                return func((TState)s, sl);
            }, initialState, sl.CancellationToken, creationOptions), onListenerCreated);
            Add(operationManager, item);
            return item;
        }

        /// <summary>
        /// Adds a new pending background operation.
        /// </summary>
        /// <param name="initialState">The initial value for the <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}.AsyncOpViewModel.State"/> property.</param>
        /// <param name="operationManager">The <see cref="AsyncOpResultManagerViewModel{TState, AsyncFuncOpViewModel{TState, TResult}, StatusListenerImpl, TResult}"/>
        /// that will track the background operation.</param>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <param name="creationOptions">The backround task creation options.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TState, TResult}"/> object representing the status of the background operation.</returns>
        /// <remarks>Use <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}.StartPendingOperation(TItem, TaskScheduler)">AsyncOpManagerViewModel&lt;TState, TResult&gt;.StartPendingOperation(AsyncFuncOpViewModel&lt;TState, TResult&gt;, TaskScheduler?)</see>
        /// to start pending background operations.
        /// <para>Use <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}.AsyncOpViewModel.Cancel(bool)">AsyncFuncOpViewModel{TState, TResult}.Cancel(bool)</see>
        /// or <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}.AsyncOpViewModel.Cancel()">AsyncFuncOpViewModel{TState, TResult}.Cancel()</see> to cancel
        /// background operations.</para></remarks>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="func"/> was null.</exception>
        public static AsyncFuncOpViewModel<TState, TResult> AddPending(TState initialState, string initialMessage,
            [DisallowNull] AsyncOpResultManagerViewModel<TState, AsyncFuncOpViewModel<TState, TResult>, StatusListenerImpl, TResult> operationManager,
            [DisallowNull] Func<TState, StatusListenerImpl, TResult> func, TaskCreationOptions creationOptions, Guid? concurrencyId = null) =>
            AddPending(initialState, initialMessage, operationManager, null, func, creationOptions, concurrencyId);

        /// <summary>
        /// Adds a new pending background operation.
        /// </summary>
        /// <param name="initialState">The initial value for the <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}.AsyncOpViewModel.State"/> property.</param>
        /// <param name="operationManager">The <see cref="AsyncOpResultManagerViewModel{TState, AsyncFuncOpViewModel{TState, TResult}, StatusListenerImpl, TResult}"/>
        /// that will track the background operation.</param>
        /// <param name="onListenerCreated">The delegate that will be invoked on the UI thread after the <see cref="StatusListenerImpl">status listener</see> is created and
        /// before the <see cref="Task{TResult}"/> is created.</param>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TState, TResult}"/> object representing the status of the background operation.</returns>
        /// <remarks>Use <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}.StartPendingOperation(TItem, TaskScheduler)">AsyncOpManagerViewModel&lt;TState, TResult&gt;.StartPendingOperation(AsyncFuncOpViewModel&lt;TState, TResult&gt;, TaskScheduler?)</see> to start pending background operations.
        /// <para>Use <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}.AsyncOpViewModel.Cancel(bool)">AsyncFuncOpViewModel{TState, TResult}.Cancel(bool)</see>
        /// or <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}.AsyncOpViewModel.Cancel()">AsyncFuncOpViewModel{TState, TResult}.Cancel()</see> to cancel
        /// background operations.</para></remarks>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="func"/> was null.</exception>
        public static AsyncFuncOpViewModel<TState, TResult> AddPending(TState initialState, string initialMessage,
            [DisallowNull] AsyncOpResultManagerViewModel<TState, AsyncFuncOpViewModel<TState, TResult>, StatusListenerImpl, TResult> operationManager,
            [AllowNull] Action<StatusListenerImpl> onListenerCreated, [DisallowNull] Func<TState, StatusListenerImpl, TResult> func, Guid? concurrencyId = null)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            AsyncFuncOpViewModel<TState, TResult> item = new(concurrencyId ?? Guid.NewGuid(), initialState, initialMessage, sl => new Task<TResult>(s =>
            {
                sl.RaiseTaskStarted();
                return func((TState)s, sl);
            }, initialState, sl.CancellationToken), onListenerCreated);
            Add(operationManager, item);
            return item;
        }

        /// <summary>
        /// Adds a new pending background operation.
        /// </summary>
        /// <param name="initialState">The initial value for the <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}.AsyncOpViewModel.State"/> property.</param>
        /// <param name="operationManager">The <see cref="AsyncOpResultManagerViewModel{TState, AsyncFuncOpViewModel{TState, TResult}, StatusListenerImpl, TResult}"/>
        /// that will track the background operation.</param>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TState, TResult}"/> object representing the status of the background operation.</returns>
        /// <remarks>Use <see cref="AsyncOpManagerViewModel{TState, Task{TResult}, TItem, TListener}.StartPendingOperation(TItem, TaskScheduler)">AsyncOpManagerViewModel&lt;TState, TResult&gt;.StartPendingOperation(AsyncFuncOpViewModel&lt;TState, TResult&gt;, TaskScheduler?)</see> to start pending background operations.
        /// <para>Use <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}.AsyncOpViewModel.Cancel(bool)">AsyncFuncOpViewModel{TState, TResult}.Cancel(bool)</see>
        /// or <see cref="AsyncOpManagerViewModel{TState, TTask, TItem, TListener}.AsyncOpViewModel.Cancel()">AsyncFuncOpViewModel{TState, TResult}.Cancel()</see> to cancel
        /// background operations.</para></remarks>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="func"/> was null.</exception>
        public static AsyncFuncOpViewModel<TState, TResult> AddPending(TState initialState, string initialMessage,
            [DisallowNull] AsyncOpResultManagerViewModel<TState, AsyncFuncOpViewModel<TState, TResult>, StatusListenerImpl, TResult> operationManager,
            [DisallowNull] Func<TState, StatusListenerImpl, TResult> func, Guid? concurrencyId = null) => AddPending(initialState, initialMessage, operationManager, null, func, concurrencyId);

        class Builder : FuncItemBuilder
        {
            internal Builder(TState initialState, Func<StatusListenerImpl, Task<TResult>> createTask) : base(initialState, createTask) { }
            protected internal override StatusListenerImpl GetStatusListener(AsyncOpManagerViewModel<TState, Task<TResult>, AsyncFuncOpViewModel<TState, TResult>, StatusListenerImpl>.AsyncOpViewModel instance) => new((AsyncFuncOpViewModel<TState, TResult>)instance);
        }

        class TaskProxy
        {
            private readonly TState _initialState;
            private readonly Func<TState, StatusListenerImpl, Task<TResult>> _func;
            internal TaskProxy(TState initialState, Func<TState, StatusListenerImpl, Task<TResult>> func)
            {
                _initialState = initialState;
                _func = func;
            }
            internal Task<TResult> Handler(StatusListenerImpl sl)
            {
                return Task.Run(async () =>
                {
                    sl.RaiseTaskStarted();
                    return await _func(_initialState, sl);
                }, sl.CancellationToken);
            }
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
        private AsyncFuncOpViewModel(Guid concurrencyId, string initialMessage, Func<StatusListenerImpl, Task<TResult>> createTask, [AllowNull] Action<StatusListenerImpl> onListenerCreated)
            : base(concurrencyId, initialMessage, new Builder(createTask), onListenerCreated) { }

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="operationManager">The <see cref="AsyncOpResultManagerViewModel{AsyncFuncOpViewModel{TResult}, StatusListenerImpl, TResult}"/> that will track the background operation.</param>
        /// <param name="onListenerCreated">The delegate that will be invoked on the UI thread after the <see cref="StatusListenerImpl">status listener</see> is created and
        /// before the <see cref="Task{TResult}"/> is created.</param>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <param name="creationOptions">The backround task creation options.</param>
        /// <param name="scheduler">The optional scheduler to use. The default is the <see cref="TaskScheduler.Default">instance provided by the .NET framework</see>.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TResult}"/> object representing the status of the background operation.</returns>
        /// <remarks>Use <see cref="AsyncOpManagerViewModel{object, TTask, TItem, TListener}.AsyncOpViewModel.Cancel(bool)">AsyncFuncOpViewModel{TResult}.Cancel(bool)</see>
        /// or <see cref="AsyncOpManagerViewModel{object, TTask, TItem, TListener}.AsyncOpViewModel.Cancel()">AsyncFuncOpViewModel{TResult}.Cancel()</see> to cancel
        /// background operations.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="func"/> was null.</exception>
        public static AsyncFuncOpViewModel<TResult> StartNew(string initialMessage, [DisallowNull] AsyncOpResultManagerViewModel<AsyncFuncOpViewModel<TResult>, StatusListenerImpl, TResult> operationManager,
            [AllowNull] Action<StatusListenerImpl> onListenerCreated, [DisallowNull] Func<StatusListenerImpl, TResult> func, TaskCreationOptions creationOptions, Guid? concurrencyId = null, TaskScheduler scheduler = null)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            AsyncFuncOpViewModel<TResult> item = new(concurrencyId ?? Guid.NewGuid(), initialMessage, sl => Task.Factory.StartNew(() =>
            {
                sl.RaiseTaskStarted();
                return func(sl);
            }, sl.CancellationToken, creationOptions, scheduler ?? TaskScheduler.Default), onListenerCreated);
            Add(operationManager, item);
            return item;
        }

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="operationManager">The <see cref="AsyncOpResultManagerViewModel{AsyncFuncOpViewModel{TResult}, StatusListenerImpl, TResult}"/> that will track the background operation.</param>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <param name="creationOptions">The backround task creation options.</param>
        /// <param name="scheduler">The optional scheduler to use. The default is the <see cref="TaskScheduler.Default">instance provided by the .NET framework</see>.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TResult}"/> object representing the status of the background operation.</returns>
        /// <remarks>Use <see cref="AsyncOpManagerViewModel{object, TTask, TItem, TListener}.AsyncOpViewModel.Cancel(bool)">AsyncFuncOpViewModel{TResult}.Cancel(bool)</see>
        /// or <see cref="AsyncOpManagerViewModel{object, TTask, TItem, TListener}.AsyncOpViewModel.Cancel()">AsyncFuncOpViewModel{TResult}.Cancel()</see> to cancel
        /// background operations.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="func"/> was null.</exception>
        public static AsyncFuncOpViewModel<TResult> StartNew(string initialMessage, [DisallowNull] AsyncOpResultManagerViewModel<AsyncFuncOpViewModel<TResult>, StatusListenerImpl, TResult> operationManager,
            [DisallowNull] Func<StatusListenerImpl, TResult> func, TaskCreationOptions creationOptions, Guid? concurrencyId = null, TaskScheduler scheduler = null) =>
            StartNew(initialMessage, operationManager, null, func, creationOptions, concurrencyId, scheduler);

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="operationManager">The <see cref="AsyncOpResultManagerViewModel{AsyncFuncOpViewModel{TResult}, StatusListenerImpl, TResult}"/> that will track the background operation.</param>
        /// <param name="onListenerCreated">The delegate that will be invoked on the UI thread after the <see cref="StatusListenerImpl">status listener</see> is created and
        /// before the <see cref="Task{TResult}"/> is created.</param>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TResult}"/> object representing the status of the background operation.</returns>
        /// <remarks>Use <see cref="AsyncOpManagerViewModel{object, TTask, TItem, TListener}.AsyncOpViewModel.Cancel(bool)">AsyncFuncOpViewModel{TResult}.Cancel(bool)</see>
        /// or <see cref="AsyncOpManagerViewModel{object, TTask, TItem, TListener}.AsyncOpViewModel.Cancel()">AsyncFuncOpViewModel{TResult}.Cancel()</see> to cancel
        /// background operations.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="func"/> was null.</exception>
        public static AsyncFuncOpViewModel<TResult> StartNew(string initialMessage, [DisallowNull] AsyncOpResultManagerViewModel<AsyncFuncOpViewModel<TResult>, StatusListenerImpl, TResult> operationManager,
            [AllowNull] Action<StatusListenerImpl> onListenerCreated, [DisallowNull] Func<StatusListenerImpl, TResult> func, Guid? concurrencyId = null)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            AsyncFuncOpViewModel<TResult> item = new(concurrencyId ?? Guid.NewGuid(), initialMessage, sl => Task.Factory.StartNew(() =>
            {
                sl.RaiseTaskStarted();
                return func(sl);
            }, sl.CancellationToken), onListenerCreated);
            Add(operationManager, item);
            return item;
        }

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="operationManager">The <see cref="AsyncOpResultManagerViewModel{AsyncFuncOpViewModel{TResult}, StatusListenerImpl, TResult}"/> that will track the background operation.</param>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TResult}"/> object representing the status of the background operation.</returns>
        /// <remarks>Use <see cref="AsyncOpManagerViewModel{object, TTask, TItem, TListener}.AsyncOpViewModel.Cancel(bool)">AsyncFuncOpViewModel{TResult}.Cancel(bool)</see>
        /// or <see cref="AsyncOpManagerViewModel{object, TTask, TItem, TListener}.AsyncOpViewModel.Cancel()">AsyncFuncOpViewModel{TResult}.Cancel()</see> to cancel
        /// background operations.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="func"/> was null.</exception>
        public static AsyncFuncOpViewModel<TResult> StartNew(string initialMessage, [DisallowNull] AsyncOpResultManagerViewModel<AsyncFuncOpViewModel<TResult>, StatusListenerImpl, TResult> operationManager,
            [DisallowNull] Func<StatusListenerImpl, TResult> func, Guid? concurrencyId = null) => StartNew(initialMessage, operationManager, null, func, concurrencyId);

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="operationManager">The <see cref="AsyncOpResultManagerViewModel{AsyncFuncOpViewModel{TResult}, StatusListenerImpl, TResult}"/> that will track the background operation.</param>
        /// <param name="onListenerCreated">The delegate that will be invoked on the UI thread after the <see cref="StatusListenerImpl">status listener</see> is created and
        /// before the <see cref="Task{TResult}"/> is created.</param>
        /// <param name="func">The asynchronous delegate that will implement the background operation and produce the result value.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TResult}"/> object representing the status of the background operation.</returns>
        /// <remarks>Use <see cref="AsyncOpManagerViewModel{object, TTask, TItem, TListener}.AsyncOpViewModel.Cancel(bool)">AsyncFuncOpViewModel{TResult}.Cancel(bool)</see>
        /// or <see cref="AsyncOpManagerViewModel{object, TTask, TItem, TListener}.AsyncOpViewModel.Cancel()">AsyncFuncOpViewModel{TResult}.Cancel()</see> to cancel
        /// background operations.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="func"/> was null.</exception>
        public static AsyncFuncOpViewModel<TResult> FromAsync(string initialMessage, [DisallowNull] AsyncOpResultManagerViewModel<AsyncFuncOpViewModel<TResult>, StatusListenerImpl, TResult> operationManager,
            [AllowNull] Action<StatusListenerImpl> onListenerCreated, [DisallowNull] Func<StatusListenerImpl, Task<TResult>> func, Guid? concurrencyId = null)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            AsyncFuncOpViewModel<TResult> item = new(concurrencyId ?? Guid.NewGuid(), initialMessage, async sl =>
            {
                sl.RaiseTaskStarted();
                return await func(sl);
            }, onListenerCreated);
            Add(operationManager, item);
            return item;
        }

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="operationManager">The <see cref="AsyncOpResultManagerViewModel{AsyncFuncOpViewModel{TResult}, StatusListenerImpl, TResult}"/> that will track the background operation.</param>
        /// <param name="func">The asynchronous delegate that will implement the background operation and produce the result value.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TResult}"/> object representing the status of the background operation.</returns>
        /// <remarks>Use <see cref="AsyncOpManagerViewModel{object, TTask, TItem, TListener}.AsyncOpViewModel.Cancel(bool)">AsyncFuncOpViewModel{TResult}.Cancel(bool)</see>
        /// or <see cref="AsyncOpManagerViewModel{object, TTask, TItem, TListener}.AsyncOpViewModel.Cancel()">AsyncFuncOpViewModel{TResult}.Cancel()</see> to cancel
        /// background operations.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="func"/> was null.</exception>
        public static AsyncFuncOpViewModel<TResult> FromAsync(string initialMessage, [DisallowNull] AsyncOpResultManagerViewModel<AsyncFuncOpViewModel<TResult>, StatusListenerImpl, TResult> operationManager,
            [DisallowNull] Func<StatusListenerImpl, Task<TResult>> func, Guid? concurrencyId = null) => FromAsync(initialMessage, operationManager, null, func, concurrencyId);

        /// <summary>
        /// Adds a new pending background operation.
        /// </summary>
        /// <param name="operationManager">The <see cref="AsyncOpResultManagerViewModel{AsyncFuncOpViewModel{TResult}, StatusListenerImpl, TResult}"/> that will track the background operation.</param>
        /// <param name="onListenerCreated">The delegate that will be invoked on the UI thread after the <see cref="StatusListenerImpl">status listener</see> is created and
        /// before the <see cref="Task{TResult}"/> is created.</param>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <param name="creationOptions">The backround task creation options.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TResult}"/> object representing the status of the background operation.</returns>
        /// <remarks>Use <see cref="AsyncOpManagerViewModel{object, Task{TResult}, TItem, TListener}.StartPendingOperation(TItem, TaskScheduler)">AsyncOpManagerViewModel&lt;TResult&gt;.StartPendingOperation(AsyncFuncOpViewModel&lt;TResult&gt;, TaskScheduler?)</see> to start pending background operations.
        /// <para>Use <see cref="AsyncOpManagerViewModel{object, TTask, TItem, TListener}.AsyncOpViewModel.Cancel(bool)">AsyncFuncOpViewModel{TResult}.Cancel(bool)</see>
        /// or <see cref="AsyncOpManagerViewModel{object, TTask, TItem, TListener}.AsyncOpViewModel.Cancel()">AsyncFuncOpViewModel{TResult}.Cancel()</see> to cancel
        /// background operations.</para></remarks>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="func"/> was null.</exception>
        public static AsyncFuncOpViewModel<TResult> AddPending(string initialMessage, [DisallowNull] AsyncOpResultManagerViewModel<AsyncFuncOpViewModel<TResult>, StatusListenerImpl, TResult> operationManager,
            [AllowNull] Action<StatusListenerImpl> onListenerCreated, [DisallowNull] Func<StatusListenerImpl, TResult> func, TaskCreationOptions creationOptions, Guid? concurrencyId = null)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            AsyncFuncOpViewModel<TResult> item = new(concurrencyId ?? Guid.NewGuid(), initialMessage, sl => new Task<TResult>(() =>
            {
                sl.RaiseTaskStarted();
                return func(sl);
            }, sl.CancellationToken, creationOptions), onListenerCreated);
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
        /// <remarks>Use <see cref="AsyncOpManagerViewModel{object, Task{TResult}, TItem, TListener}.StartPendingOperation(TItem, TaskScheduler)">AsyncOpManagerViewModel&lt;TResult&gt;.StartPendingOperation(AsyncFuncOpViewModel&lt;TResult&gt;, TaskScheduler?)</see> to start pending background operations.
        /// <para>Use <see cref="AsyncOpManagerViewModel{object, TTask, TItem, TListener}.AsyncOpViewModel.Cancel(bool)">AsyncFuncOpViewModel{TResult}.Cancel(bool)</see>
        /// or <see cref="AsyncOpManagerViewModel{object, TTask, TItem, TListener}.AsyncOpViewModel.Cancel()">AsyncFuncOpViewModel{TResult}.Cancel()</see> to cancel
        /// background operations.</para></remarks>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="func"/> was null.</exception>
        public static AsyncFuncOpViewModel<TResult> AddPending(string initialMessage, [DisallowNull] AsyncOpResultManagerViewModel<AsyncFuncOpViewModel<TResult>, StatusListenerImpl, TResult> operationManager,
            [DisallowNull] Func<StatusListenerImpl, TResult> func, TaskCreationOptions creationOptions, Guid? concurrencyId = null) => AddPending(initialMessage, operationManager, null, func, creationOptions, concurrencyId);

        /// <summary>
        /// Adds a new pending background operation.
        /// </summary>
        /// <param name="operationManager">The <see cref="AsyncOpResultManagerViewModel{AsyncFuncOpViewModel{TResult}, StatusListenerImpl, TResult}"/> that will track the background operation.</param>
        /// <param name="onListenerCreated">The delegate that will be invoked on the UI thread after the <see cref="StatusListenerImpl">status listener</see> is created and
        /// before the <see cref="Task{TResult}"/> is created.</param>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TResult}"/> object representing the status of the background operation.</returns>
        /// <remarks>Use <see cref="AsyncOpManagerViewModel{object, Task{TResult}, TItem, TListener}.StartPendingOperation(TItem, TaskScheduler)">AsyncOpManagerViewModel&lt;TResult&gt;.StartPendingOperation(AsyncFuncOpViewModel&lt;TResult&gt;, TaskScheduler?)</see> to start pending background operations.
        /// <para>Use <see cref="AsyncOpManagerViewModel{object, TTask, TItem, TListener}.AsyncOpViewModel.Cancel(bool)">AsyncFuncOpViewModel{TResult}.Cancel(bool)</see>
        /// or <see cref="AsyncOpManagerViewModel{object, TTask, TItem, TListener}.AsyncOpViewModel.Cancel()">AsyncFuncOpViewModel{TResult}.Cancel()</see> to cancel
        /// background operations.</para></remarks>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="func"/> was null.</exception>
        public static AsyncFuncOpViewModel<TResult> AddPending(string initialMessage, [DisallowNull] AsyncOpResultManagerViewModel<AsyncFuncOpViewModel<TResult>, StatusListenerImpl, TResult> operationManager,
            [AllowNull] Action<StatusListenerImpl> onListenerCreated, [DisallowNull] Func<StatusListenerImpl, TResult> func, Guid? concurrencyId = null)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            AsyncFuncOpViewModel<TResult> item = new(concurrencyId ?? Guid.NewGuid(), initialMessage, sl => new Task<TResult>(() =>
            {
                sl.RaiseTaskStarted();
                return func(sl);
            }, sl.CancellationToken), onListenerCreated);
            Add(operationManager, item);
            return item;
        }

        /// <summary>
        /// Adds a new pending background operation.
        /// </summary>
        /// <param name="operationManager">The <see cref="AsyncOpResultManagerViewModel{AsyncFuncOpViewModel{TResult}, StatusListenerImpl, TResult}"/> that will track the background operation.</param>
        /// <param name="func">The delegate that will implement the background operation and produce the result value.</param>
        /// <returns>A <see cref="AsyncFuncOpViewModel{TResult}"/> object representing the status of the background operation.</returns>
        /// <remarks>Use <see cref="AsyncOpManagerViewModel{object, Task{TResult}, TItem, TListener}.StartPendingOperation(TItem, TaskScheduler)">AsyncOpManagerViewModel&lt;TResult&gt;.StartPendingOperation(AsyncFuncOpViewModel&lt;TResult&gt;, TaskScheduler?)</see> to start pending background operations.
        /// <para>Use <see cref="AsyncOpManagerViewModel{object, TTask, TItem, TListener}.AsyncOpViewModel.Cancel(bool)">AsyncFuncOpViewModel{TResult}.Cancel(bool)</see>
        /// or <see cref="AsyncOpManagerViewModel{object, TTask, TItem, TListener}.AsyncOpViewModel.Cancel()">AsyncFuncOpViewModel{TResult}.Cancel()</see> to cancel
        /// background operations.</para></remarks>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="func"/> was null.</exception>
        public static AsyncFuncOpViewModel<TResult> AddPending(string initialMessage, [DisallowNull] AsyncOpResultManagerViewModel<AsyncFuncOpViewModel<TResult>, StatusListenerImpl, TResult> operationManager,
            [DisallowNull] Func<StatusListenerImpl, TResult> func, Guid? concurrencyId = null) => AddPending(initialMessage, operationManager, null, func, concurrencyId);

        class Builder : FuncItemBuilder
        {
            internal Builder(Func<StatusListenerImpl, Task<TResult>> createTask) : base(null, createTask) { }
            protected internal override StatusListenerImpl GetStatusListener(AsyncOpManagerViewModel<Task<TResult>, AsyncFuncOpViewModel<TResult>, StatusListenerImpl>.AsyncOpViewModel instance) => new((AsyncFuncOpViewModel<TResult>)instance);
        }
    }
}
