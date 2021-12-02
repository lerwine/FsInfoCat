using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace FsInfoCat.BgOps
{
    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface IAsyncOpService : IAsyncOpFactory,  IReadOnlyCollection<IAsyncAction>, IHostedService
    {
    }
}
