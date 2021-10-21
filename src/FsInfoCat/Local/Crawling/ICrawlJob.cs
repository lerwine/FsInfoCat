using FsInfoCat.AsyncOps;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Crawling
{
    public interface ICrawlJob : IJobResult<CrawlTerminationReason>, ICancellableJob
    {
        ICurrentItem CurrentItem { get; }
    }
}
