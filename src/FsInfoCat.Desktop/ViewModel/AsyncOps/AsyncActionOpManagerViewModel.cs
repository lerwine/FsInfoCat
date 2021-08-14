using System;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public class AsyncActionOpManagerViewModel<TState> : AsyncOpManagerViewModel<TState, Task, AsyncActionOpViewModel<TState>, AsyncActionOpViewModel<TState>.StatusListenerImpl>
    {
        public AsyncActionOpViewModel<TState> StartNewBgOperation(TState initialState, string initialMessage, Action<TState, AsyncActionOpViewModel<TState>.StatusListenerImpl> action, TaskCreationOptions creationOptions, Guid? concurrencyId = null, TaskScheduler scheduler = null) =>
            AsyncActionOpViewModel<TState>.StartNew(initialState, initialMessage, this, action, creationOptions, concurrencyId, scheduler);

        public AsyncActionOpViewModel<TState> StartNewBgOperation(TState initialState, string initialMessage, Action<TState, AsyncActionOpViewModel<TState>.StatusListenerImpl> action, Guid? concurrencyId = null) =>
            AsyncActionOpViewModel<TState>.StartNew(initialState, initialMessage, this, action, concurrencyId);

        public AsyncActionOpViewModel<TState> StartBgOperationFromAsync(TState initialState, string initialMessage, Func<TState, AsyncActionOpViewModel<TState>.StatusListenerImpl, Task> action, Guid? concurrencyId = null) =>
            AsyncActionOpViewModel<TState>.FromAsync(initialState, initialMessage, this, action, concurrencyId);

        public AsyncActionOpViewModel<TState> AddPendingBgOperation(TState initialState, string initialMessage, Action<TState, AsyncActionOpViewModel<TState>.StatusListenerImpl> action, TaskCreationOptions creationOptions, Guid? concurrencyId = null) =>
            AsyncActionOpViewModel<TState>.AddPending(initialState, initialMessage, this, action, creationOptions, concurrencyId);

        public AsyncActionOpViewModel<TState> AddPendingBgOperation(TState initialState, string initialMessage, Action<TState, AsyncActionOpViewModel<TState>.StatusListenerImpl> action, Guid? concurrencyId = null) =>
            AsyncActionOpViewModel<TState>.AddPending(initialState, initialMessage, this, action, concurrencyId);
    }

    public class AsyncActionOpManagerViewModel : AsyncOpManagerViewModel<Task, AsyncActionOpViewModel, AsyncActionOpViewModel.StatusListenerImpl>
    {
        public AsyncActionOpViewModel StartNewBgOperation(string initialMessage, Action<AsyncActionOpViewModel.StatusListenerImpl> action, TaskCreationOptions creationOptions, Guid? concurrencyId = null, TaskScheduler scheduler = null) =>
            AsyncActionOpViewModel.StartNew(initialMessage, this, action, creationOptions, concurrencyId, scheduler);

        public AsyncActionOpViewModel StartNewBgOperation(string initialMessage, Action<AsyncActionOpViewModel.StatusListenerImpl> action, Guid? concurrencyId = null) =>
            AsyncActionOpViewModel.StartNew(initialMessage, this, action, concurrencyId);

        public AsyncActionOpViewModel StartBgOperationFromAsync(string initialMessage, Func<AsyncActionOpViewModel.StatusListenerImpl, Task> action, Guid? concurrencyId = null) =>
            AsyncActionOpViewModel.FromAsync(initialMessage, this, action, concurrencyId);

        public AsyncActionOpViewModel AddPendingBgOperation(string initialMessage, Action<AsyncActionOpViewModel.StatusListenerImpl> action, TaskCreationOptions creationOptions, Guid? concurrencyId = null) =>
            AsyncActionOpViewModel.AddPending(initialMessage, this, action, creationOptions, concurrencyId);

        public AsyncActionOpViewModel AddPendingBgOperation(string initialMessage, Action<AsyncActionOpViewModel.StatusListenerImpl> action, Guid? concurrencyId = null) =>
            AsyncActionOpViewModel.AddPending(initialMessage, this, action, concurrencyId);
    }
}
