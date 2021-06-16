using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    public interface ISubdirectory : IDbFsItem
    {
        DirectoryCrawlOptions Options { get; set; }

        DirectoryStatus Status { get; set; }

        IVolume Volume { get; set; }

        ICrawlConfiguration CrawlConfiguration { get; set; }

        IEnumerable<IFile> Files { get; }

        IEnumerable<ISubdirectory> SubDirectories { get; }

        new IEnumerable<IAccessError<ISubdirectory>> AccessErrors { get; }
    }
}
