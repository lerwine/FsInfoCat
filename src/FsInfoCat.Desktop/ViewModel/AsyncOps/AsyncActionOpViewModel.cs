using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    /// <summary>
    /// View model which indicates the status of a background operation implemented through a <c><see cref="Action{TState, StatusListenerImpl}"/>. 
    /// <para>Extends <see cref="AsyncOpManagerViewModel{TState, Task, AsyncActionOpViewModel{TState}, StatusListenerImpl}.AsyncOpViewModel" />.</para>
    /// </summary>
    /// <typeparam name="TState">The type of the state object associated with the background <see cref="Task"/>.</typeparam>
    /// <seealso cref="AsyncOpManagerViewModel{TState, Task, AsyncActionOpViewModel{TState}, StatusListenerImpl}.AsyncOpViewModel" />
    public partial class AsyncActionOpViewModel<TState> : AsyncOpManagerViewModel<TState, Task, AsyncActionOpViewModel<TState>, AsyncActionOpViewModel<TState>.StatusListenerImpl>.AsyncOpViewModel
    {
        private AsyncActionOpViewModel(Guid concurrencyId, TState initialState, string initialMessage, [DisallowNull] Func<StatusListenerImpl, Task> createTask, [AllowNull] Action<StatusListenerImpl> onListenerCreated)
            : base(concurrencyId, initialMessage, new Builder(initialState, createTask), onListenerCreated) { }

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="initialState">The initial value for the <see cref="AsyncOpManagerViewModel{TState, Task, AsyncActionOpViewModel{TState}, TListener}.AsyncOpViewModel.State"/> property.</param>
        /// <param name="operationManager">The <see cref="AsyncOpManagerViewModel{TState, Task, AsyncActionOpViewModel{TState}, TListener}"/> that will track the background operation.</param>
        /// <param name="onListenerCreated">The delegate that will be invoked on the UI thread after the <typeparamref name="TListener">status listener</typeparamref> is created and
        /// before the <see cref="Task"/> is created.</param>
        /// <param name="action">The delegate that will implement the background operation.</param>
        /// <param name="creationOptions">The task creation options.</param>
        /// <param name="scheduler">The optional scheduler to use. The default is the <see cref="TaskScheduler.Default">instance provided by the .NET framework</see>.</param>
        /// <returns>The <see cref="AsyncActionOpViewModel{TState}"/> object representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="action"/> was null.</exception>
        public static AsyncActionOpViewModel<TState> StartNew(TState initialState, string initialMessage,
            [DisallowNull] AsyncOpManagerViewModel<TState, Task, AsyncActionOpViewModel<TState>, StatusListenerImpl> operationManager,
            [DisallowNull] Action<StatusListenerImpl> onListenerCreated, [DisallowNull] Action<TState, StatusListenerImpl> action, TaskCreationOptions creationOptions, Guid? concurrencyId = null, TaskScheduler scheduler = null)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            AsyncActionOpViewModel<TState> item = new(concurrencyId ?? Guid.NewGuid(), initialState, initialMessage, sl => Task.Factory.StartNew(s =>
            {
                sl.RaiseTaskStarted();
                action((TState)s, sl);
            }, initialState, sl.CancellationToken, creationOptions, scheduler ?? TaskScheduler.Default), onListenerCreated);
            Add(operationManager, item);
            return item;
        }

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="initialState">The initial value for the <see cref="AsyncOpManagerViewModel{TState, Task, AsyncActionOpViewModel{TState}, TListener}.AsyncOpViewModel.State"/> property.</param>
        /// <param name="operationManager">The <see cref="AsyncOpManagerViewModel{TState, Task, AsyncActionOpViewModel{TState}, TListener}"/> that will track the background operation.</param>
        /// <param name="action">The delegate that will implement the background operation.</param>
        /// <param name="creationOptions">The task creation options.</param>
        /// <param name="scheduler">The optional scheduler to use. The default is the <see cref="TaskScheduler.Default">instance provided by the .NET framework</see>.</param>
        /// <returns>The <see cref="AsyncActionOpViewModel{TState}"/> object representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="action"/> was null.</exception>
        public static AsyncActionOpViewModel<TState> StartNew(TState initialState, string initialMessage,
            [DisallowNull] AsyncOpManagerViewModel<TState, Task, AsyncActionOpViewModel<TState>, StatusListenerImpl> operationManager,
            [DisallowNull] Action<TState, StatusListenerImpl> action, TaskCreationOptions creationOptions, Guid? concurrencyId = null, TaskScheduler scheduler = null) =>
            StartNew(initialState, initialMessage, operationManager, null, action, creationOptions, concurrencyId, scheduler);

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="initialState">The initial value for the <see cref="AsyncOpManagerViewModel{TState, Task, AsyncActionOpViewModel{TState}, TListener}.AsyncOpViewModel.State"/> property.</param>
        /// <param name="operationManager">The <see cref="AsyncOpManagerViewModel{TState, Task, AsyncActionOpViewModel{TState}, TListener}"/> that will track the background operation.</param>
        /// <param name="onListenerCreated">The delegate that will be invoked on the UI thread after the <typeparamref name="TListener">status listener</typeparamref> is created and
        /// before the <see cref="Task"/> is created.</param>
        /// <param name="action">The delegate that will implement the background operation.</param>
        /// <returns>The <see cref="AsyncActionOpViewModel{TState}"/> object representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="action"/> was null.</exception>
        public static AsyncActionOpViewModel<TState> StartNew(TState initialState, string initialMessage,
            [DisallowNull] AsyncOpManagerViewModel<TState, Task, AsyncActionOpViewModel<TState>, StatusListenerImpl> operationManager,
           [DisallowNull] Action<StatusListenerImpl> onListenerCreated, [DisallowNull] Action<TState, StatusListenerImpl> action, Guid? concurrencyId = null)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            AsyncActionOpViewModel<TState> item = new(concurrencyId ?? Guid.NewGuid(), initialState, initialMessage, sl => Task.Factory.StartNew(s =>
            {
                sl.RaiseTaskStarted();
                action((TState)s, sl);
            }, initialState, sl.CancellationToken), onListenerCreated);
            Add(operationManager, item);
            return item;
        }

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="initialState">The initial value for the <see cref="AsyncOpManagerViewModel{TState, Task, AsyncActionOpViewModel{TState}, TListener}.AsyncOpViewModel.State"/> property.</param>
        /// <param name="operationManager">The <see cref="AsyncOpManagerViewModel{TState, Task, AsyncActionOpViewModel{TState}, TListener}"/> that will track the background operation.</param>
        /// <param name="action">The delegate that will implement the background operation.</param>
        /// <returns>The <see cref="AsyncActionOpViewModel{TState}"/> object representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="action"/> was null.</exception>
        public static AsyncActionOpViewModel<TState> StartNew(TState initialState, string initialMessage,
            [DisallowNull] AsyncOpManagerViewModel<TState, Task, AsyncActionOpViewModel<TState>, StatusListenerImpl> operationManager,
            [DisallowNull] Action<TState, StatusListenerImpl> action, Guid? concurrencyId = null) => StartNew(initialState, initialMessage, operationManager, null, action, concurrencyId);

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="initialState">The initial value for the <see cref="AsyncOpManagerViewModel{TState, Task, AsyncActionOpViewModel{TState}, TListener}.AsyncOpViewModel.State"/> property.</param>
        /// <param name="operationManager">The <see cref="AsyncOpManagerViewModel{TState, Task, AsyncActionOpViewModel{TState}, TListener}"/> that will track the background operation.</param>
        /// <param name="onListenerCreated">The delegate that will be invoked on the UI thread after the <typeparamref name="TListener">status listener</typeparamref> is created and
        /// before the <see cref="Task"/> is created.</param>
        /// <param name="action">The asynchronous delegate that will implement the background operation.</param>
        /// <returns>The <see cref="AsyncActionOpViewModel{TState}"/> object representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="action"/> was null.</exception>
        public static AsyncActionOpViewModel<TState> FromAsync(TState initialState, string initialMessage,
            [DisallowNull] AsyncOpManagerViewModel<TState, Task, AsyncActionOpViewModel<TState>, StatusListenerImpl> operationManager,
            [DisallowNull] Action<StatusListenerImpl> onListenerCreated, [DisallowNull] Func<TState, StatusListenerImpl, Task> action, Guid? concurrencyId = null)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            AsyncActionOpViewModel<TState> item = new(concurrencyId ?? Guid.NewGuid(), initialState, initialMessage, async sl =>
            {
                sl.RaiseTaskStarted();
                await action(initialState, sl);
            }, onListenerCreated);
            Add(operationManager, item);
            return item;
        }

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="initialState">The initial value for the <see cref="AsyncOpManagerViewModel{TState, Task, AsyncActionOpViewModel{TState}, TListener}.AsyncOpViewModel.State"/> property.</param>
        /// <param name="operationManager">The <see cref="AsyncOpManagerViewModel{TState, Task, AsyncActionOpViewModel{TState}, TListener}"/> that will track the background operation.</param>
        /// <param name="action">The asynchronous delegate that will implement the background operation.</param>
        /// <returns>The <see cref="AsyncActionOpViewModel{TState}"/> object representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="action"/> was null.</exception>
        public static AsyncActionOpViewModel<TState> FromAsync(TState initialState, string initialMessage,
            [DisallowNull] AsyncOpManagerViewModel<TState, Task, AsyncActionOpViewModel<TState>, StatusListenerImpl> operationManager,
            [DisallowNull] Func<TState, StatusListenerImpl, Task> action, Guid? concurrencyId = null) => FromAsync(initialState, initialMessage, operationManager, null, action, concurrencyId);

        /// <summary>
        /// Adds a new pending background operation.
        /// </summary>
        /// <param name="initialState">The initial value for the <see cref="AsyncOpManagerViewModel{TState, Task, AsyncActionOpViewModel{TState}, TListener}.AsyncOpViewModel.State"/> property.</param>
        /// <param name="operationManager">The <see cref="AsyncOpManagerViewModel{TState, Task, AsyncActionOpViewModel{TState}, TListener}"/> that will track the background operation.</param>
        /// <param name="onListenerCreated">The delegate that will be invoked on the UI thread after the <typeparamref name="TListener">status listener</typeparamref> is created and
        /// before the <see cref="Task"/> is created.</param>
        /// <param name="action">The delegate that will implement the background operation.</param>
        /// <param name="creationOptions">The task creation options.</param>
        /// <returns>The <see cref="AsyncActionOpViewModel{TState}"/> object representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="action"/> was null.</exception>
        public static AsyncActionOpViewModel<TState> AddPending(TState initialState, string initialMessage,
            [DisallowNull] AsyncOpManagerViewModel<TState, Task, AsyncActionOpViewModel<TState>, StatusListenerImpl> operationManager,
            [DisallowNull] Action<StatusListenerImpl> onListenerCreated, [DisallowNull] Action<TState, StatusListenerImpl> action, TaskCreationOptions creationOptions, Guid? concurrencyId = null)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            AsyncActionOpViewModel<TState> item = new(concurrencyId ?? Guid.NewGuid(), initialState, initialMessage, sl => new Task(s =>
            {
                sl.RaiseTaskStarted();
                action((TState)s, sl);
            }, initialState, sl.CancellationToken, creationOptions), onListenerCreated);
            Add(operationManager, item);
            return item;
        }

        /// <summary>
        /// Adds a new pending background operation.
        /// </summary>
        /// <param name="initialState">The initial value for the <see cref="AsyncOpManagerViewModel{TState, Task, AsyncActionOpViewModel{TState}, TListener}.AsyncOpViewModel.State"/> property.</param>
        /// <param name="operationManager">The <see cref="AsyncOpManagerViewModel{TState, Task, AsyncActionOpViewModel{TState}, TListener}"/> that will track the background operation.</param>
        /// <param name="action">The delegate that will implement the background operation.</param>
        /// <param name="creationOptions">The task creation options.</param>
        /// <returns>The <see cref="AsyncActionOpViewModel{TState}"/> object representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="action"/> was null.</exception>
        public static AsyncActionOpViewModel<TState> AddPending(TState initialState, string initialMessage,
            [DisallowNull] AsyncOpManagerViewModel<TState, Task, AsyncActionOpViewModel<TState>, StatusListenerImpl> operationManager,
            [DisallowNull] Action<TState, StatusListenerImpl> action, TaskCreationOptions creationOptions, Guid? concurrencyId = null) =>
            AddPending(initialState, initialMessage, operationManager, null, action, creationOptions, concurrencyId);

        /// <summary>
        /// Adds a new pending background operation.
        /// </summary>
        /// <param name="initialState">The initial value for the <see cref="AsyncOpManagerViewModel{TState, Task, AsyncActionOpViewModel{TState}, TListener}.AsyncOpViewModel.State"/> property.</param>
        /// <param name="operationManager">The <see cref="AsyncOpManagerViewModel{TState, Task, AsyncActionOpViewModel{TState}, TListener}"/> that will track the background operation.</param>
        /// <param name="onListenerCreated">The delegate that will be invoked on the UI thread after the <typeparamref name="TListener">status listener</typeparamref> is created and
        /// before the <see cref="Task"/> is created.</param>
        /// <param name="action">The delegate that will implement the background operation.</param>
        /// <returns>The <see cref="AsyncActionOpViewModel{TState}"/> object representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="action"/> was null.</exception>
        public static AsyncActionOpViewModel<TState> AddPending(TState initialState, string initialMessage,
            [DisallowNull] AsyncOpManagerViewModel<TState, Task, AsyncActionOpViewModel<TState>, StatusListenerImpl> operationManager,
            [DisallowNull] Action<StatusListenerImpl> onListenerCreated, [DisallowNull] Action<TState, StatusListenerImpl> action, Guid? concurrencyId = null)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            AsyncActionOpViewModel<TState> item = new(concurrencyId ?? Guid.NewGuid(), initialState, initialMessage, sl => new Task(s =>
            {
                sl.RaiseTaskStarted();
                action((TState)s, sl);
            }, initialState, sl.CancellationToken), onListenerCreated);
            Add(operationManager, item);
            return item;
        }

        /// <summary>
        /// Adds a new pending background operation.
        /// </summary>
        /// <param name="initialState">The initial value for the <see cref="AsyncOpManagerViewModel{TState, Task, AsyncActionOpViewModel{TState}, TListener}.AsyncOpViewModel.State"/> property.</param>
        /// <param name="operationManager">The <see cref="AsyncOpManagerViewModel{TState, Task, AsyncActionOpViewModel{TState}, TListener}"/> that will track the background operation.</param>
        /// <param name="action">The delegate that will implement the background operation.</param>
        /// <returns>The <see cref="AsyncActionOpViewModel{TState}"/> object representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="action"/> was null.</exception>
        public static AsyncActionOpViewModel<TState> AddPending(TState initialState, string initialMessage,
            [DisallowNull] AsyncOpManagerViewModel<TState, Task, AsyncActionOpViewModel<TState>, StatusListenerImpl> operationManager,
            [DisallowNull] Action<TState, StatusListenerImpl> action, Guid? concurrencyId = null) =>
            AddPending(initialState, initialMessage, operationManager, null, action, concurrencyId);
    }

    /// <summary>
    /// View model which indicates the status of a background operation implemented through a <c><see cref="Action{StatusListenerImpl}"/>.
    /// <para>Extends <see cref="AsyncOpManagerViewModel{object, Task, AsyncActionOpViewModel, StatusListenerImpl}.AsyncOpViewModel" />.</para>
    /// </summary>
    /// <seealso cref="AsyncOpManagerViewModel{object, Task, AsyncActionOpViewModel, StatusListenerImpl}.AsyncOpViewModel" />
    public partial class AsyncActionOpViewModel : AsyncOpManagerViewModel<object, Task, AsyncActionOpViewModel, AsyncActionOpViewModel.StatusListenerImpl>.AsyncOpViewModel
    {
        private AsyncActionOpViewModel(Guid concurrencyId, string initialMessage, [DisallowNull] Func<StatusListenerImpl, Task> createTask, [AllowNull] Action<StatusListenerImpl> onListenerCreated) : base(concurrencyId, initialMessage, new Builder(createTask), onListenerCreated) { }

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="operationManager">The <see cref="AsyncOpManagerViewModel{Task, AsyncActionOpViewModel, StatusListenerImpl}"/> that will track the background operation.</param>
        /// <param name="onListenerCreated">The delegate that will be invoked on the UI thread after the <typeparamref name="TListener">status listener</typeparamref> is created and
        /// before the <see cref="Task"/> is created.</param>
        /// <param name="action">The delegate that will implement the background operation.</param>
        /// <param name="creationOptions">The task creation options.</param>
        /// <param name="scheduler">The optional scheduler to use. The default is the <see cref="TaskScheduler.Default">instance provided by the .NET framework</see>.</param>
        /// <returns>A <see cref="AsyncActionOpViewModel"/> representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="action"/> was null.</exception>
        public static AsyncActionOpViewModel StartNew(string initialMessage, [DisallowNull] AsyncOpManagerViewModel<Task, AsyncActionOpViewModel, StatusListenerImpl> operationManager,
            [DisallowNull] Action<StatusListenerImpl> onListenerCreated, [DisallowNull] Action<StatusListenerImpl> action, TaskCreationOptions creationOptions, Guid? concurrencyId = null, TaskScheduler scheduler = null)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            AsyncActionOpViewModel item = new(concurrencyId ?? Guid.NewGuid(), initialMessage, sl => Task.Factory.StartNew(() =>
            {
                sl.RaiseTaskStarted();
                action(sl);
            }, sl.CancellationToken, creationOptions, scheduler ?? TaskScheduler.Default), onListenerCreated);
            Add(operationManager, item);
            return item;
        }

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="operationManager">The <see cref="AsyncOpManagerViewModel{Task, AsyncActionOpViewModel, StatusListenerImpl}"/> that will track the background operation.</param>
        /// <param name="action">The delegate that will implement the background operation.</param>
        /// <param name="creationOptions">The task creation options.</param>
        /// <param name="scheduler">The optional scheduler to use. The default is the <see cref="TaskScheduler.Default">instance provided by the .NET framework</see>.</param>
        /// <returns>A <see cref="AsyncActionOpViewModel"/> representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="action"/> was null.</exception>
        public static AsyncActionOpViewModel StartNew(string initialMessage, [DisallowNull] AsyncOpManagerViewModel<Task, AsyncActionOpViewModel, StatusListenerImpl> operationManager,
            [DisallowNull] Action<StatusListenerImpl> action, TaskCreationOptions creationOptions, Guid? concurrencyId = null, TaskScheduler scheduler = null) =>
            StartNew(initialMessage, operationManager, null, action, creationOptions, concurrencyId, scheduler);

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="operationManager">The <see cref="AsyncOpManagerViewModel{Task, AsyncActionOpViewModel, StatusListenerImpl}"/> that will track the background operation.</param>
        /// <param name="onListenerCreated">The delegate that will be invoked on the UI thread after the <typeparamref name="TListener">status listener</typeparamref> is created and
        /// before the <see cref="Task"/> is created.</param>
        /// <param name="action">The delegate that will implement the background operation.</param>
        /// <returns>A <see cref="AsyncActionOpViewModel"/> representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="action"/> was null.</exception>
        public static AsyncActionOpViewModel StartNew(string initialMessage, [DisallowNull] AsyncOpManagerViewModel<Task, AsyncActionOpViewModel, StatusListenerImpl> operationManager,
            [DisallowNull] Action<StatusListenerImpl> onListenerCreated, [DisallowNull] Action<StatusListenerImpl> action, Guid? concurrencyId = null)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            AsyncActionOpViewModel item = new(concurrencyId ?? Guid.NewGuid(), initialMessage, sl => Task.Factory.StartNew(() =>
            {
                sl.RaiseTaskStarted();
                action(sl);
            }, sl.CancellationToken), onListenerCreated);
            Add(operationManager, item);
            return item;
        }

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="operationManager">The <see cref="AsyncOpManagerViewModel{Task, AsyncActionOpViewModel, StatusListenerImpl}"/> that will track the background operation.</param>
        /// <param name="action">The delegate that will implement the background operation.</param>
        /// <returns>A <see cref="AsyncActionOpViewModel"/> representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="action"/> was null.</exception>
        public static AsyncActionOpViewModel StartNew(string initialMessage, [DisallowNull] AsyncOpManagerViewModel<Task, AsyncActionOpViewModel, StatusListenerImpl> operationManager,
            [DisallowNull] Action<StatusListenerImpl> action, Guid? concurrencyId = null) => StartNew(initialMessage, operationManager, null, action, concurrencyId);

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="operationManager">The <see cref="AsyncOpManagerViewModel{Task, AsyncActionOpViewModel, StatusListenerImpl}"/> that will track the background operation.</param>
        /// <param name="onListenerCreated">The delegate that will be invoked on the UI thread after the <typeparamref name="TListener">status listener</typeparamref> is created and
        /// before the <see cref="Task"/> is created.</param>
        /// <param name="action">The asynchronous delegate that will implement the background operation.</param>
        /// <returns>A <see cref="AsyncActionOpViewModel"/> representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="action"/> was null.</exception>
        public static AsyncActionOpViewModel FromAsync(string initialMessage, [DisallowNull] AsyncOpManagerViewModel<Task, AsyncActionOpViewModel, StatusListenerImpl> operationManager,
            [DisallowNull] Action<StatusListenerImpl> onListenerCreated, [DisallowNull] Func<StatusListenerImpl, Task> action, Guid? concurrencyId = null)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            AsyncActionOpViewModel item = new(concurrencyId ?? Guid.NewGuid(), initialMessage, async sl =>
            {
                sl.RaiseTaskStarted();
                await action(sl);
            }, onListenerCreated);
            Add(operationManager, item);
            return item;
        }

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="operationManager">The <see cref="AsyncOpManagerViewModel{Task, AsyncActionOpViewModel, StatusListenerImpl}"/> that will track the background operation.</param>
        /// <param name="action">The asynchronous delegate that will implement the background operation.</param>
        /// <returns>A <see cref="AsyncActionOpViewModel"/> representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="action"/> was null.</exception>
        public static AsyncActionOpViewModel FromAsync(string initialMessage, [DisallowNull] AsyncOpManagerViewModel<Task, AsyncActionOpViewModel, StatusListenerImpl> operationManager,
            [DisallowNull] Func<StatusListenerImpl, Task> action, Guid? concurrencyId = null) => FromAsync(initialMessage, operationManager, null, action, concurrencyId);

        /// <summary>
        /// Adds a new pending background operation.
        /// </summary>
        /// <param name="operationManager">The <see cref="AsyncOpManagerViewModel{Task, AsyncActionOpViewModel, StatusListenerImpl}"/> that will track the background operation.</param>
        /// <param name="onListenerCreated">The delegate that will be invoked on the UI thread after the <typeparamref name="TListener">status listener</typeparamref> is created and
        /// before the <see cref="Task"/> is created.</param>
        /// <param name="action">The delegate that will implement the background operation.</param>
        /// <param name="creationOptions">The task creation options.</param>
        /// <returns>A <see cref="AsyncActionOpViewModel"/> representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="action"/> was null.</exception>
        public static AsyncActionOpViewModel AddPending(string initialMessage, [DisallowNull] AsyncOpManagerViewModel<Task, AsyncActionOpViewModel, StatusListenerImpl> operationManager,
            [DisallowNull] Action<StatusListenerImpl> onListenerCreated, [DisallowNull] Action<StatusListenerImpl> action, TaskCreationOptions creationOptions, Guid? concurrencyId = null)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            AsyncActionOpViewModel item = new(concurrencyId ?? Guid.NewGuid(), initialMessage, sl => new Task(() =>
            {
                sl.RaiseTaskStarted();
                action(sl);
            }, sl.CancellationToken, creationOptions), onListenerCreated);
            Add(operationManager, item);
            return item;
        }

        /// <summary>
        /// Adds a new pending background operation.
        /// </summary>
        /// <param name="operationManager">The <see cref="AsyncOpManagerViewModel{Task, AsyncActionOpViewModel, StatusListenerImpl}"/> that will track the background operation.</param>
        /// <param name="action">The delegate that will implement the background operation.</param>
        /// <param name="creationOptions">The task creation options.</param>
        /// <returns>A <see cref="AsyncActionOpViewModel"/> representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="action"/> was null.</exception>
        public static AsyncActionOpViewModel AddPending(string initialMessage, [DisallowNull] AsyncOpManagerViewModel<Task, AsyncActionOpViewModel, StatusListenerImpl> operationManager,
            [DisallowNull] Action<StatusListenerImpl> action, TaskCreationOptions creationOptions, Guid? concurrencyId = null) => AddPending(initialMessage, operationManager, null, action, creationOptions, concurrencyId);

        /// <summary>
        /// Adds a new pending background operation.
        /// </summary>
        /// <param name="operationManager">The <see cref="AsyncOpManagerViewModel{Task, AsyncActionOpViewModel, StatusListenerImpl}"/> that will track the background operation.</param>
        /// <param name="onListenerCreated">The delegate that will be invoked on the UI thread after the <typeparamref name="TListener">status listener</typeparamref> is created and
        /// before the <see cref="Task"/> is created.</param>
        /// <param name="action">The delegate that will implement the background operation.</param>
        /// <returns>A <see cref="AsyncActionOpViewModel"/> representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="action"/> was null.</exception>
        public static AsyncActionOpViewModel AddPending(string initialMessage, [DisallowNull] AsyncOpManagerViewModel<Task, AsyncActionOpViewModel, StatusListenerImpl> operationManager,
            [DisallowNull] Action<StatusListenerImpl> onListenerCreated, [DisallowNull] Action<StatusListenerImpl> action, Guid? concurrencyId = null)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            AsyncActionOpViewModel item = new(concurrencyId ?? Guid.NewGuid(), initialMessage, sl => new Task(() =>
            {
                sl.RaiseTaskStarted();
                action(sl);
            }, sl.CancellationToken), onListenerCreated);
            Add(operationManager, item);
            return item;
        }

        /// <summary>
        /// Adds a new pending background operation.
        /// </summary>
        /// <param name="operationManager">The <see cref="AsyncOpManagerViewModel{Task, AsyncActionOpViewModel, StatusListenerImpl}"/> that will track the background operation.</param>
        /// <param name="action">The delegate that will implement the background operation.</param>
        /// <returns>A <see cref="AsyncActionOpViewModel"/> representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="action"/> was null.</exception>
        public static AsyncActionOpViewModel AddPending(string initialMessage, [DisallowNull] AsyncOpManagerViewModel<Task, AsyncActionOpViewModel, StatusListenerImpl> operationManager,
            [DisallowNull] Action<StatusListenerImpl> action, Guid? concurrencyId = null) => AddPending(initialMessage, operationManager, null, action, concurrencyId);
    }
}
