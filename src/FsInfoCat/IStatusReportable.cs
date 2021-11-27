using FsInfoCat.AsyncOps;
using System;
using System.Threading;

namespace FsInfoCat
{
    [Obsolete("Use FsInfoCat.AsyncOps.IBackgroundProgress<TEvent>, instead")]
    public interface IStatusReportable : IProgress<OperationStatus>, IProgress<string>, IProgress<MessageCode>
    {
        CancellationToken Token { get; }

        ActivityCode Activity { get; }

        OperationStatus Current { get; }

        void Report(MessageCode statusDescription, string currentOperation);
    }
}
