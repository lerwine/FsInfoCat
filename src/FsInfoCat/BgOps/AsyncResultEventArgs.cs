namespace FsInfoCat.BgOps
{
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public class AsyncResultEventArgs<TState, TResult> : AsyncOpEventArgs<TState>, IAsyncOpResultArgs<TState, TResult>
    {
        public TResult Result => throw new System.NotImplementedException();
    }
}
