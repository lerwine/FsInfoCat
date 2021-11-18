using System;
using System.Threading;

namespace FsInfoCat.Background
{
    public interface IBgActivityProgress : IProgress<string>, IProgress<Exception>, IBgActivityObject, IBgActivitySource
    {
        CancellationToken Token { get; }

        void ReportCurrentOperation(string operation);

        void Report(string statusMessage, string operation);

        void Report(string statusMessage, Exception error);
    }

    public interface IBgActivityProgress<TState> : IBgActivityProgress, IBgActivityObject<TState>
    {
    }
}
