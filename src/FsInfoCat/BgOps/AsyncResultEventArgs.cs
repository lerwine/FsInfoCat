namespace FsInfoCat.BgOps
{
    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public class AsyncResultEventArgs<TState, TResult> : AsyncOpEventArgs<TState>, IAsyncOpResultArgs<TState, TResult>
    {
        public TResult Result => throw new System.NotImplementedException();
    }
}
