using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace FsInfoCat.BgOps
{
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IAsyncOpService : IAsyncOpFactory,  IReadOnlyCollection<IAsyncAction>, IHostedService
    {
    }
}
