using System;
using System.Threading;

namespace FsInfoCat.Services
{
    public interface IBackgroundProgress<TEvent> : IBackgroundProgressInfo, IProgress<TEvent>, IBackgroundProgressFactory
        where TEvent : IBackgroundProgressEvent
    {
        CancellationToken Token { get; }

        void ReportCurrentOperation(string currentOperation);

        void ReportStatusDescription(string statusDescription, string currentOperation);

        void ReportStatusDescription(string statusDescription);

        void ReportException(Exception exception, string statusDescription, string currentOperation);

        void ReportException(Exception exception, string statusDescription);

        void ReportException(Exception exception);
    }

    public interface IBackgroundProgress<TState, TEvent> : IBackgroundProgress<TEvent>, IBackgroundProgressInfo<TState>
        where TEvent : IBackgroundProgressEvent<TState>
    {
    }
}
