using FsInfoCat.AsyncOps;

namespace FsInfoCat.Background
{
    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface IBgActivityObject
    {
        ActivityCode Activity { get; }

        object AsyncState { get; }
    }

    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface IBgActivityObject<TState> : IBgActivityObject
    {
        new TState AsyncState { get; }
    }
}
