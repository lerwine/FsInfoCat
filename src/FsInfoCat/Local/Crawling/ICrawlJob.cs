using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Crawling
{
    public interface ICrawlJob : FsInfoCat.Background.ILongRunningAsyncService<ICrawlResult>
    {
        ICurrentItem CurrentItem { get; }
    }
}
