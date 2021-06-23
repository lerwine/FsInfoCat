using System;
using System.IO;

namespace FsInfoCat.Local
{
    public class FileCrawlEventArgs : CrawlEventArgs
    {
        public FileInfo FS { get; }
        public DbFile DB { get; }
        public DirectoryInfo ParentFS { get; }
        public Subdirectory ParentDB { get; }
        public int Depth { get; }

        public FileCrawlEventArgs(CrawlWorker.CrawlContext crawlContext, FileInfo fs, DbFile db) : base((crawlContext ?? throw new ArgumentNullException(nameof(crawlContext))).Worker)
        {
            FS = fs ?? throw new ArgumentNullException(nameof(fs));
            DB = db;
            ParentFS = crawlContext.FS;
            ParentDB = crawlContext.DB;
            Depth = crawlContext.Depth;
        }
    }
}
