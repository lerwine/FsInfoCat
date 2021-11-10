using System;

namespace FsInfoCat.Local.Crawling
{
    public interface ICrawlProgress : IProgress<ICrawlJobEventArgs>, IProgress<DirectoryCrawlEventArgs>, IProgress<FileCrawlEventArgs>
    {
        void ReportOtherActivity(CrawlActivityEventArgs e);
    }
}
