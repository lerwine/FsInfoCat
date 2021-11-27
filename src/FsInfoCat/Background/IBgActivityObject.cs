using FsInfoCat.AsyncOps;

namespace FsInfoCat.Background
{
    public interface IBgActivityObject
    {
        ActivityCode Activity { get; }

        object AsyncState { get; }
    }

    public interface IBgActivityObject<TState> : IBgActivityObject
    {
        new TState AsyncState { get; }
    }
}
