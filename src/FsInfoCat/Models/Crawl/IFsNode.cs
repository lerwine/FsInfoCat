using System.Collections.ObjectModel;

namespace FsInfoCat.Models.Crawl
{
    public interface IFsNode
    {
         Collection<ICrawlMessage> Messages { get; set; }
    }
}
