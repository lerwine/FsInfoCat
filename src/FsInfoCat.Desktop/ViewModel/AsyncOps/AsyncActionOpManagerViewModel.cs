using System;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public class AsyncActionOpManagerViewModel<TState> : AsyncOpManagerViewModel<TState, Task, AsyncActionOpViewModel<TState>, AsyncActionOpViewModel<TState>.StatusListenerImpl>
    {
        public AsyncActionOpViewModel<TState> StartNewBgOperation(TState initialState, Action<TState, AsyncActionOpViewModel<TState>.StatusListenerImpl> action, TaskCreationOptions creationOptions, Guid? concurrencyId = null, TaskScheduler scheduler = null) =>
            AsyncActionOpViewModel<TState>.StartNew(initialState, this, action, creationOptions, concurrencyId, scheduler);

        public AsyncActionOpViewModel<TState> StartNewBgOperation(TState initialState, Action<TState, AsyncActionOpViewModel<TState>.StatusListenerImpl> action, Guid? concurrencyId = null) =>
            AsyncActionOpViewModel<TState>.StartNew(initialState, this, action, concurrencyId);

        public AsyncActionOpViewModel<TState> StartBgOperationFromAsync(TState initialState, Func<TState, AsyncActionOpViewModel<TState>.StatusListenerImpl, Task> action, Guid? concurrencyId = null) =>
            AsyncActionOpViewModel<TState>.FromAsync(initialState, this, action, concurrencyId);

        public AsyncActionOpViewModel<TState> AddPendingBgOperation(TState initialState, Action<TState, AsyncActionOpViewModel<TState>.StatusListenerImpl> action, TaskCreationOptions creationOptions, Guid? concurrencyId = null) =>
            AsyncActionOpViewModel<TState>.AddPending(initialState, this, action, creationOptions, concurrencyId);

        public AsyncActionOpViewModel<TState> AddPendingBgOperation(TState initialState, Action<TState, AsyncActionOpViewModel<TState>.StatusListenerImpl> action, Guid? concurrencyId = null) =>
            AsyncActionOpViewModel<TState>.AddPending(initialState, this, action, concurrencyId);
    }

    public class AsyncActionOpManagerViewModel : AsyncOpManagerViewModel<Task, AsyncActionOpViewModel, AsyncActionOpViewModel.StatusListenerImpl>
    {
        public AsyncActionOpViewModel StartNewBgOperation(Action<AsyncActionOpViewModel.StatusListenerImpl> action, TaskCreationOptions creationOptions, Guid? concurrencyId = null, TaskScheduler scheduler = null) =>
            AsyncActionOpViewModel.StartNew(this, action, creationOptions, concurrencyId, scheduler);

        public AsyncActionOpViewModel StartNewBgOperation(Action<AsyncActionOpViewModel.StatusListenerImpl> action, Guid? concurrencyId = null) =>
            AsyncActionOpViewModel.StartNew(this, action, concurrencyId);

        public AsyncActionOpViewModel StartBgOperationFromAsync(Func<AsyncActionOpViewModel.StatusListenerImpl, Task> action, Guid? concurrencyId = null) =>
            AsyncActionOpViewModel.FromAsync(this, action, concurrencyId);

        public AsyncActionOpViewModel AddPendingBgOperation(Action<AsyncActionOpViewModel.StatusListenerImpl> action, TaskCreationOptions creationOptions, Guid? concurrencyId = null) =>
            AsyncActionOpViewModel.AddPending(this, action, creationOptions, concurrencyId);

        public AsyncActionOpViewModel AddPendingBgOperation(Action<AsyncActionOpViewModel.StatusListenerImpl> action, Guid? concurrencyId = null) =>
            AsyncActionOpViewModel.AddPending(this, action, concurrencyId);
    }
}
