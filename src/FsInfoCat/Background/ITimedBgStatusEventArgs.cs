namespace FsInfoCat.Background
{
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedBgStatusEventArgs : ITimedBgActivityObject, IBgStatusEventArgs
    {
    }

    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface ITimedBgStatusEventArgs<TState> : ITimedBgStatusEventArgs, IBgStatusEventArgs<TState>, ITimedBgActivityObject<TState>
    {
    }
}
