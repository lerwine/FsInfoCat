using System;
using System.Threading.Tasks;

namespace FsInfoCat.Background
{
    public class BgActivityProgressEventArgs<TState> : IBgStatusEventArgs<TState>
    {
        public Exception Exception { get; }

        public ActivityCode Activity { get; }

        public string StatusMessage { get; }

        public string CurrentOperation { get; }

        public TState AsyncState { get; }

        object IBgActivityObject.AsyncState => AsyncState;

        TaskStatus IBgStatusEventArgs.Status => TaskStatus.Running;

        public BgActivityProgressEventArgs(TState asyncState, ActivityCode activity, string statusMessage, string currentOperation, Exception exception)
        {
            Activity = activity;
            StatusMessage = statusMessage ?? "";
            CurrentOperation = currentOperation ?? "";
            AsyncState = asyncState;
            Exception = exception;
        }
    }
}
