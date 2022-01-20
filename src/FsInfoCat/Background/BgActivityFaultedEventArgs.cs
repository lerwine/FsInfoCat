using FsInfoCat.AsyncOps;
using System;
using System.Threading.Tasks;

namespace FsInfoCat.Background
{
    [Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public class BgActivityFaultedEventArgs<TState> : IBgStatusEventArgs<TState>
    {
        public Exception Exception { get; }

        public ActivityCode Activity { get; }

        public string StatusMessage { get; }

        public string CurrentOperation { get; }

        public TState AsyncState { get; }

        object IBgActivityObject.AsyncState => AsyncState;

        TaskStatus IBgStatusEventArgs.Status => TaskStatus.Faulted;

        public BgActivityFaultedEventArgs(TState asyncState, ActivityCode activity, string statusMessage, string currentOperation, Exception exception)
        {
            Activity = activity;
            StatusMessage = statusMessage ?? "";
            CurrentOperation = currentOperation ?? "";
            AsyncState = asyncState;
            Exception = exception;
        }
    }
}
