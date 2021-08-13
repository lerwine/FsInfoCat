using System;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public partial class AsyncActionOpViewModel<TState>
    {
        class Builder : AsyncOpManagerViewModel<TState, Task, AsyncActionOpViewModel<TState>, StatusListenerImpl>.ItemBuilder
        {
            private readonly Func<StatusListenerImpl, Task> _createTask;
            public Builder(TState initialState, Func<StatusListenerImpl, Task> createTask) : base(initialState) { _createTask = createTask; }

            protected internal override StatusListenerImpl GetStatusListener(AsyncOpManagerViewModel<TState, Task, AsyncActionOpViewModel<TState>, StatusListenerImpl>.AsyncOpViewModel instance) => new((AsyncActionOpViewModel<TState>)instance);

            protected internal override Task GetTask(TState state, StatusListenerImpl listener, AsyncOpManagerViewModel<TState, Task, AsyncActionOpViewModel<TState>, StatusListenerImpl>.AsyncOpViewModel instance) => _createTask(listener);
        }
    }
}
