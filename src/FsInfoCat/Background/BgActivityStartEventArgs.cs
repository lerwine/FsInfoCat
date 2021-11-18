using System;
using System.Threading.Tasks;

namespace FsInfoCat.Background
{
    public class BgActivityStartEventArgs<TState> : IBgStatusEventArgs<TState>
    {
        Exception IBgStatusEventArgs.Exception => null;

        public ActivityCode Activity { get; }

        public string StatusMessage { get; }

        public string CurrentOperation { get; }

        public TState AsyncState { get; }

        object IBgActivityObject.AsyncState => AsyncState;

        TaskStatus IBgStatusEventArgs.Status => TaskStatus.Running;

        public BgActivityStartEventArgs(TState asyncState, ActivityCode activity, string statusMessage, string currentOperation)
        {
            Activity = activity;
            StatusMessage = statusMessage ?? "";
            CurrentOperation = currentOperation ?? "";
            AsyncState = asyncState;
        }
    }
}
