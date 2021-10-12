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

        internal FileCrawlEventArgs(CrawlManagerService.CrawlWorker worker, CrawlManagerService.Context context, FileAccessError accessError = null)
            : base(worker, context, accessError)
        {
            FS = context.FileInfo;
            DB = context.DbFile;
            ParentFS = context.DirectoryInfo;
            ParentDB = context.Subdirectory;
        }
    }
}
