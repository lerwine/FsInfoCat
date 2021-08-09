using System;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public partial class AsyncActionOpViewModel<TState>
    {
        public class StatusListenerImpl : StatusListener
        {
            internal StatusListenerImpl(AsyncActionOpViewModel<TState> item) : base(item) { }
        }
    }

    public partial class AsyncActionOpViewModel
    {
        public class StatusListenerImpl : StatusListener
        {
            internal StatusListenerImpl(AsyncActionOpViewModel item) : base(item) { }
        }
    }
}
