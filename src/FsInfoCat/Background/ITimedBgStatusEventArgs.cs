namespace FsInfoCat.Background
{
    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface ITimedBgStatusEventArgs : ITimedBgActivityObject, IBgStatusEventArgs
    {
    }

    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface ITimedBgStatusEventArgs<TState> : ITimedBgStatusEventArgs, IBgStatusEventArgs<TState>, ITimedBgActivityObject<TState>
    {
    }
}
