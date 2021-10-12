using System;
using System.IO;

namespace FsInfoCat.Local
{
    public class DirectoryCrawlEventArgs : CrawlEventArgs
    {
        public DirectoryInfo FS { get; }

        public Subdirectory DB { get; }

        internal DirectoryCrawlEventArgs(CrawlManagerService.CrawlWorker worker, CrawlManagerService.Context context, SubdirectoryAccessError accessError = null)
            : base(worker, context, accessError)
        {
            FS = context.DirectoryInfo;
            DB = context.Subdirectory;
        }
    }
}
