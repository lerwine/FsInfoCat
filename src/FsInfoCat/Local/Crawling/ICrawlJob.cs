using FsInfoCat.AsyncOps;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Crawling
{
    public interface ICrawlJob : IAsyncResult, IProgress<IJobResult<CrawlTerminationReason>>
    {
        ICurrentItem CurrentItem { get; }

        AsyncJobStatus JobStatus { get; }
    }
}
