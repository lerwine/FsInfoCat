using System;
using System.Threading.Tasks;

namespace FsInfoCat.Background
{
    [Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface IBgStatusEventArgs : IBgActivityObject
    {
        Exception Exception { get; }

        TaskStatus Status { get; }

        string StatusMessage { get; }

        string CurrentOperation { get; }
    }

    [Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface IBgStatusEventArgs<TState> : IBgStatusEventArgs, IBgActivityObject<TState>
    {
    }
}
