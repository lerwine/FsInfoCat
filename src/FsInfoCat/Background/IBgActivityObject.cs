using FsInfoCat.AsyncOps;

namespace FsInfoCat.Background
{
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IBgActivityObject
    {
        ActivityCode Activity { get; }

        object AsyncState { get; }
    }

    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IBgActivityObject<TState> : IBgActivityObject
    {
        new TState AsyncState { get; }
    }
}
