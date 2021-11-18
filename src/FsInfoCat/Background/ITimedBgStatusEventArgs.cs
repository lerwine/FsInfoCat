namespace FsInfoCat.Background
{
    public interface ITimedBgStatusEventArgs : ITimedBgActivityObject, IBgStatusEventArgs
    {
    }

    public interface ITimedBgStatusEventArgs<TState> : ITimedBgStatusEventArgs, IBgStatusEventArgs<TState>, ITimedBgActivityObject<TState>
    {
    }
}
