namespace FsInfoCat.BgOps
{
    public class AsyncResultEventArgs<TState, TResult> : AsyncOpEventArgs<TState>, IAsyncOpResultArgs<TState, TResult>
    {
        public TResult Result => throw new System.NotImplementedException();
    }
}
