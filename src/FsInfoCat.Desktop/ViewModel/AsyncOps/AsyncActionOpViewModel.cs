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
        private AsyncActionOpViewModel(TState initialState, Func<StatusListenerImpl, Task> createTask) : base(new Builder(initialState, createTask)) { }

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
        public static AsyncActionOpViewModel<TState> StartNew(TState initialState, [DisallowNull] AsyncOpManagerViewModel<TState, Task, AsyncActionOpViewModel<TState>, StatusListenerImpl> operationManager,
            [DisallowNull] Action<TState, StatusListenerImpl> action, TaskCreationOptions creationOptions, TaskScheduler scheduler = null)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            AsyncActionOpViewModel<TState> item = new(initialState, sl => Task.Factory.StartNew(s =>
            {
                sl.RaiseTaskStarted();
                action((TState)s, sl);
            }, initialState, sl.CancellationToken, creationOptions, scheduler ?? TaskScheduler.Default));
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
        public static AsyncActionOpViewModel<TState> StartNew(TState initialState, [DisallowNull] AsyncOpManagerViewModel<TState, Task, AsyncActionOpViewModel<TState>, StatusListenerImpl> operationManager,
            [DisallowNull] Action<TState, StatusListenerImpl> action)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            AsyncActionOpViewModel<TState> item = new(initialState, sl => Task.Factory.StartNew(s =>
            {
                sl.RaiseTaskStarted();
                action((TState)s, sl);
            }, initialState, sl.CancellationToken));
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
        public static AsyncActionOpViewModel<TState> FromAsync(TState initialState,
            [DisallowNull] AsyncOpManagerViewModel<TState, Task, AsyncActionOpViewModel<TState>, StatusListenerImpl> operationManager,
            [DisallowNull] Func<TState, StatusListenerImpl, Task> action)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            AsyncActionOpViewModel<TState> item = new(initialState, async sl =>
            {
                sl.RaiseTaskStarted();
                await action(initialState, sl);
            });
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
        public static AsyncActionOpViewModel<TState> AddPending(TState initialState, [DisallowNull] AsyncOpManagerViewModel<TState, Task, AsyncActionOpViewModel<TState>, StatusListenerImpl> operationManager,
            [DisallowNull] Action<TState, StatusListenerImpl> action, TaskCreationOptions creationOptions)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            AsyncActionOpViewModel<TState> item = new(initialState, sl => new Task(s =>
            {
                sl.RaiseTaskStarted();
                action((TState)s, sl);
            }, initialState, sl.CancellationToken, creationOptions));
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
        public static AsyncActionOpViewModel<TState> AddPending(TState initialState, [DisallowNull] AsyncOpManagerViewModel<TState, Task, AsyncActionOpViewModel<TState>, StatusListenerImpl> operationManager,
            [DisallowNull] Action<TState, StatusListenerImpl> action)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            AsyncActionOpViewModel<TState> item = new(initialState, sl => new Task(s =>
            {
                sl.RaiseTaskStarted();
                action((TState)s, sl);
            }, initialState, sl.CancellationToken));
            Add(operationManager, item);
            return item;
        }
        class Builder : AsyncOpManagerViewModel<TState, Task, AsyncActionOpViewModel<TState>, StatusListenerImpl>.ItemBuilder
        {
            private readonly Func<StatusListenerImpl, Task> _createTask;
            public Builder(TState initialState, Func<StatusListenerImpl, Task> createTask) : base(initialState) { _createTask = createTask; }

            protected internal override StatusListenerImpl GetStatusListener(AsyncOpManagerViewModel<TState, Task, AsyncActionOpViewModel<TState>, StatusListenerImpl>.AsyncOpViewModel instance) => new((AsyncActionOpViewModel<TState>)instance);

            protected internal override Task GetTask(TState state, StatusListenerImpl listener, AsyncOpManagerViewModel<TState, Task, AsyncActionOpViewModel<TState>, StatusListenerImpl>.AsyncOpViewModel instance) => _createTask(listener);
        }
    }

    /// <summary>
    /// View model which indicates the status of a background operation implemented through a <c><see cref="Action{StatusListenerImpl}"/>.
    /// <para>Extends <see cref="AsyncOpManagerViewModel{object, Task, AsyncActionOpViewModel, StatusListenerImpl}.AsyncOpViewModel" />.</para>
    /// </summary>
    /// <seealso cref="AsyncOpManagerViewModel{object, Task, AsyncActionOpViewModel, StatusListenerImpl}.AsyncOpViewModel" />
    public partial class AsyncActionOpViewModel : AsyncOpManagerViewModel<object, Task, AsyncActionOpViewModel, AsyncActionOpViewModel.StatusListenerImpl>.AsyncOpViewModel
    {
        private AsyncActionOpViewModel(Func<StatusListenerImpl, Task> createTask) : base(new Builder(createTask)) { }

        /// <summary>
        /// Starts a new background operation.
        /// </summary>
        /// <param name="operationManager">The <see cref="AsyncOpManagerViewModel{Task, AsyncActionOpViewModel, StatusListenerImpl}"/> that will track the background operation.</param>
        /// <param name="action">The delegate that will implement the background operation.</param>
        /// <param name="creationOptions">The task creation options.</param>
        /// <param name="scheduler">The optional scheduler to use. The default is the <see cref="TaskScheduler.Default">instance provided by the .NET framework</see>.</param>
        /// <returns>A <see cref="AsyncActionOpViewModel"/> representing the status of the background operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="operationManager"/> or <paramref name="action"/> was null.</exception>
        public static AsyncActionOpViewModel StartNew([DisallowNull] AsyncOpManagerViewModel<Task, AsyncActionOpViewModel, StatusListenerImpl> operationManager,
            [DisallowNull] Action<StatusListenerImpl> action, TaskCreationOptions creationOptions, TaskScheduler scheduler = null)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            AsyncActionOpViewModel item = new(sl => Task.Factory.StartNew(() =>
            {
                sl.RaiseTaskStarted();
                action(sl);
            }, sl.CancellationToken, creationOptions, scheduler ?? TaskScheduler.Default));
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
        public static AsyncActionOpViewModel StartNew([DisallowNull] AsyncOpManagerViewModel<Task, AsyncActionOpViewModel, StatusListenerImpl> operationManager,
            [DisallowNull] Action<StatusListenerImpl> action)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            AsyncActionOpViewModel item = new(sl => Task.Factory.StartNew(() =>
            {
                sl.RaiseTaskStarted();
                action(sl);
            }, sl.CancellationToken));
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
        public static AsyncActionOpViewModel FromAsync([DisallowNull] AsyncOpManagerViewModel<Task, AsyncActionOpViewModel, StatusListenerImpl> operationManager,
            [DisallowNull] Func<StatusListenerImpl, Task> action)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            AsyncActionOpViewModel item = new(async sl =>
            {
                sl.RaiseTaskStarted();
                await action(sl);
            });
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
        public static AsyncActionOpViewModel AddPending([DisallowNull] AsyncOpManagerViewModel<Task, AsyncActionOpViewModel, StatusListenerImpl> operationManager,
            [DisallowNull] Action<StatusListenerImpl> action, TaskCreationOptions creationOptions)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            AsyncActionOpViewModel item = new(sl => new Task(() =>
            {
                sl.RaiseTaskStarted();
                action(sl);
            }, sl.CancellationToken, creationOptions));
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
        public static AsyncActionOpViewModel AddPending([DisallowNull] AsyncOpManagerViewModel<Task, AsyncActionOpViewModel, StatusListenerImpl> operationManager,
            [DisallowNull] Action<StatusListenerImpl> action)
        {
            (operationManager ?? throw new ArgumentNullException(nameof(operationManager))).VerifyAccess();
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            AsyncActionOpViewModel item = new(sl => new Task(() =>
            {
                sl.RaiseTaskStarted();
                action(sl);
            }, sl.CancellationToken));
            Add(operationManager, item);
            return item;
        }
        class Builder : AsyncOpManagerViewModel<object, Task, AsyncActionOpViewModel, StatusListenerImpl>.ItemBuilder
        {
            private readonly Func<StatusListenerImpl, Task> _createTask;
            public Builder(Func<StatusListenerImpl, Task> createTask) : base(null) { _createTask = createTask; }

            protected internal override StatusListenerImpl GetStatusListener(AsyncOpManagerViewModel<object, Task, AsyncActionOpViewModel, StatusListenerImpl>.AsyncOpViewModel instance) => new((AsyncActionOpViewModel)instance);

            protected internal override Task GetTask(object state, StatusListenerImpl listener, AsyncOpManagerViewModel<object, Task, AsyncActionOpViewModel, StatusListenerImpl>.AsyncOpViewModel instance) => _createTask(listener);
        }
    }
}
