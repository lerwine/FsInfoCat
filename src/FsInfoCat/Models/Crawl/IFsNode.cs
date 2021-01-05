using System.Collections.ObjectModel;

namespace FsInfoCat.Models.Crawl
{
    public interface IFsNode
    {
         Collection<CrawlError> Errors { get; set; }
    }
}
