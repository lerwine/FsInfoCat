using System;
using System.Threading;

namespace FsInfoCat.Background
{
    [Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface IBgActivityProgress : IProgress<string>, IProgress<Exception>, IBgActivityObject, IBgActivitySource
    {
        CancellationToken Token { get; }

        void ReportCurrentOperation(string operation);

        void Report(string statusMessage, string operation);

        void Report(string statusMessage, Exception error);
    }

    [Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface IBgActivityProgress<TState> : IBgActivityProgress, IBgActivityObject<TState>
    {
    }
}
