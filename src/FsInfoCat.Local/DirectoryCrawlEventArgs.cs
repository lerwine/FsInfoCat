using System;
using System.IO;

namespace FsInfoCat.Local
{
    public class DirectoryCrawlEventArgs : CrawlEventArgs
    {
        public DirectoryInfo FS { get; }

        public Subdirectory DB { get; }

        public int Depth { get; }

        public DirectoryCrawlEventArgs(CrawlTaskManager.CrawlContext crawlContext) : base((crawlContext ?? throw new ArgumentNullException(nameof(crawlContext))).Worker)
        {
            FS = crawlContext.FS;
            DB = crawlContext.DB;
            Depth = crawlContext.Depth;
        }
    }
}
