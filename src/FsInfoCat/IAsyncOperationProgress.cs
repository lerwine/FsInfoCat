using System;
using System.Threading;

namespace FsInfoCat
{
    public interface IAsyncOperationProgress : IProgress<IAsyncOperationInfo>
    {
        CancellationToken Token { get; }
    }
}
