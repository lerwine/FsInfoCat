using System;
using System.Threading;

namespace FsInfoCat
{
    [Obsolete("Use System.IProgress<FsInfoCat.AsyncOps.IBackgroundProgressInfo>, instead")]
    public interface IAsyncOperationProgress : IProgress<IAsyncOperationInfo>
    {
        CancellationToken Token { get; }
    }
}
