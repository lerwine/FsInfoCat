using Microsoft.Extensions.Hosting;

namespace FsInfoCat.Services
{

    /// <summary>
    /// IO-Bound background operation queue service
    /// </summary>
    public interface IFSIOQueueService : IHostedService, IBgOperationQueue
    {
    }
}
