namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public partial class AsyncFuncOpViewModel<TState, TResult>
    {
        public class StatusListenerImpl : StatusListener
        {
            internal StatusListenerImpl(AsyncFuncOpViewModel<TState, TResult> item) : base(item) { }
        }
    }

    public partial class AsyncFuncOpViewModel<TResult>
    {
        public class StatusListenerImpl : StatusListener
        {
            internal StatusListenerImpl(AsyncFuncOpViewModel<TResult> item) : base(item) { }
        }
    }
}
