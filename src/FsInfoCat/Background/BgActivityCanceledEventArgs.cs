using FsInfoCat.AsyncOps;
using System;
using System.Threading.Tasks;

namespace FsInfoCat.Background
{
    [Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public class BgActivityCanceledEventArgs<TState> : IBgStatusEventArgs<TState>
    {
        Exception IBgStatusEventArgs.Exception => null;

        public ActivityCode Activity { get; }

        public string StatusMessage { get; }

        public string CurrentOperation { get; }

        public TState AsyncState { get; }

        object IBgActivityObject.AsyncState => AsyncState;

        TaskStatus IBgStatusEventArgs.Status => TaskStatus.Canceled;

        public BgActivityCanceledEventArgs(TState asyncState, ActivityCode activity, string statusMessage, string currentOperation)
        {
            Activity = activity;
            StatusMessage = statusMessage ?? "";
            CurrentOperation = currentOperation ?? "";
            AsyncState = asyncState;
        }
    }
}
