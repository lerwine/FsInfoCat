using System;
using System.Threading.Tasks;

namespace FsInfoCat.Background
{
    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IBgStatusEventArgs : IBgActivityObject
    {
        Exception Exception { get; }

        TaskStatus Status { get; }

        string StatusMessage { get; }

        string CurrentOperation { get; }
    }

    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IBgStatusEventArgs<TState> : IBgStatusEventArgs, IBgActivityObject<TState>
    {
    }
}
