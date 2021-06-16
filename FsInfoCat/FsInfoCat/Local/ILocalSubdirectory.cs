using System.Collections.Generic;

namespace FsInfoCat.Local
{
    public interface ILocalSubdirectory : ISubdirectory, ILocalDbFsItem
    {
        new ILocalSubdirectory Parent { get; set; }

        new ILocalVolume Volume { get; set; }

        new ILocalCrawlConfiguration CrawlConfiguration { get; set; }

        new IEnumerable<ILocalFile> Files { get; }

        new IEnumerable<ILocalSubdirectory> SubDirectories { get; }

        new IEnumerable<IAccessError<ILocalSubdirectory>> AccessErrors { get; }
    }
}
