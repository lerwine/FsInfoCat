using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace FsInfoCat.BgOps
{
    public interface IAsyncOpService : IAsyncOpFactory,  IReadOnlyCollection<IAsyncOperation>, IHostedService
    {
    }
}
