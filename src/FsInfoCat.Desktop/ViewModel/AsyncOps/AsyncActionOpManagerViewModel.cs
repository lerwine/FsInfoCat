using System;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public class AsyncActionOpManagerViewModel<TState> : AsyncOpManagerViewModel<TState, Task, AsyncActionOpViewModel<TState>, AsyncActionOpViewModel<TState>.StatusListenerImpl>
    {
        public AsyncActionOpViewModel<TState> StartNewBgOperation(TState initialState, Action<TState, AsyncActionOpViewModel<TState>.StatusListenerImpl> action, TaskCreationOptions creationOptions, TaskScheduler scheduler = null) =>
            AsyncActionOpViewModel<TState>.StartNew(initialState, this, action, creationOptions, scheduler);

        public AsyncActionOpViewModel<TState> StartNewBgOperation(TState initialState, Action<TState, AsyncActionOpViewModel<TState>.StatusListenerImpl> action) =>
            AsyncActionOpViewModel<TState>.StartNew(initialState, this, action);

        public AsyncActionOpViewModel<TState> StartBgOperationFromAsync(TState initialState, Func<TState, AsyncActionOpViewModel<TState>.StatusListenerImpl, Task> action) =>
            AsyncActionOpViewModel<TState>.FromAsync(initialState, this, action);

        public AsyncActionOpViewModel<TState> AddPendingBgOperation(TState initialState, Action<TState, AsyncActionOpViewModel<TState>.StatusListenerImpl> action, TaskCreationOptions creationOptions) =>
            AsyncActionOpViewModel<TState>.AddPending(initialState, this, action, creationOptions);

        public AsyncActionOpViewModel<TState> AddPendingBgOperation(TState initialState, Action<TState, AsyncActionOpViewModel<TState>.StatusListenerImpl> action) =>
            AsyncActionOpViewModel<TState>.AddPending(initialState, this, action);
    }

    public class AsyncActionOpManagerViewModel : AsyncOpManagerViewModel<Task, AsyncActionOpViewModel, AsyncActionOpViewModel.StatusListenerImpl>
    {
        public AsyncActionOpViewModel StartNewBgOperation(Action<AsyncActionOpViewModel.StatusListenerImpl> action, TaskCreationOptions creationOptions, TaskScheduler scheduler = null) =>
            AsyncActionOpViewModel.StartNew(this, action, creationOptions, scheduler);

        public AsyncActionOpViewModel StartNewBgOperation(Action<AsyncActionOpViewModel.StatusListenerImpl> action) =>
            AsyncActionOpViewModel.StartNew(this, action);

        public AsyncActionOpViewModel StartBgOperationFromAsync(Func<AsyncActionOpViewModel.StatusListenerImpl, Task> action) =>
            AsyncActionOpViewModel.FromAsync(this, action);

        public AsyncActionOpViewModel AddPendingBgOperation(Action<AsyncActionOpViewModel.StatusListenerImpl> action, TaskCreationOptions creationOptions) =>
            AsyncActionOpViewModel.AddPending(this, action, creationOptions);

        public AsyncActionOpViewModel AddPendingBgOperation(Action<AsyncActionOpViewModel.StatusListenerImpl> action) =>
            AsyncActionOpViewModel.AddPending(this, action);
    }
}
