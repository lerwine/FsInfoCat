using System;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{

    public partial class AsyncActionOpViewModel
    {
        class Builder : AsyncOpManagerViewModel<object, Task, AsyncActionOpViewModel, StatusListenerImpl>.ItemBuilder
        {
            private readonly Func<StatusListenerImpl, Task> _createTask;
            public Builder(Func<StatusListenerImpl, Task> createTask) : base(null) { _createTask = createTask; }

            protected internal override StatusListenerImpl GetStatusListener(AsyncOpManagerViewModel<object, Task, AsyncActionOpViewModel, StatusListenerImpl>.AsyncOpViewModel instance) => new((AsyncActionOpViewModel)instance);

            protected internal override Task GetTask(object state, StatusListenerImpl listener, AsyncOpManagerViewModel<object, Task, AsyncActionOpViewModel, StatusListenerImpl>.AsyncOpViewModel instance) => _createTask(listener);
        }
    }
}
