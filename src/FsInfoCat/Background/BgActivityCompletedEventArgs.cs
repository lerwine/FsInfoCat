using FsInfoCat.AsyncOps;
using System;
using System.Threading.Tasks;

namespace FsInfoCat.Background
{
    public class BgActivityCompletedEventArgs<TState> : IBgStatusEventArgs<TState>
    {
        Exception IBgStatusEventArgs.Exception => null;

        public ActivityCode Activity { get; }

        public string StatusMessage { get; }

        string IBgStatusEventArgs.CurrentOperation => "";

        public TState AsyncState { get; }

        object IBgActivityObject.AsyncState => AsyncState;

        TaskStatus IBgStatusEventArgs.Status => TaskStatus.RanToCompletion;

        public BgActivityCompletedEventArgs(TState asyncState, ActivityCode activity, string statusMessage)
        {
            Activity = activity;
            StatusMessage = statusMessage ?? "";
            AsyncState = asyncState;
        }
    }

    public class BgActivityCompletedEventArgs<TState, TResult> : BgActivityCompletedEventArgs<TState>
    {
        public TResult Result { get; }

        public BgActivityCompletedEventArgs(TState asyncState, ActivityCode activity, string statusMessage, TResult result)
            : base(asyncState, activity, statusMessage)
        {
            Result = result;
        }
    }
}
