using System;
using System.Threading.Tasks;

namespace FsInfoCat.Background
{
    public interface IBgStatusEventArgs : IBgActivityObject
    {
        Exception Exception { get; }

        TaskStatus Status { get; }

        string StatusMessage { get; }

        string CurrentOperation { get; }
    }

    public interface IBgStatusEventArgs<TState> : IBgStatusEventArgs, IBgActivityObject<TState>
    {
    }
}
