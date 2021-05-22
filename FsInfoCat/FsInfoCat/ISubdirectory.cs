using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    public interface ISubdirectory : IDbEntity
    {
        string Name { get; set; }

        DirectoryCrawlOptions Options { get; set; }

        DateTime LastAccessed { get; set; }

        string Notes { get; set; }

        bool Deleted { get; set; }

        ISubdirectory Parent { get; set; }

        IVolume Volume { get; set; }

        IEnumerable<IFile> Files { get; }

        IEnumerable<ISubdirectory> SubDirectories { get; }
    }
}
