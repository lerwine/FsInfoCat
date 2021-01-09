using System;

namespace FsInfoCat.Models.Crawl
{
    public interface ICrawlMessage
    {
         string Message { get; set; }
         MessageId ID { get; set; }
    }
}
