using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    public interface ISubdirectory : IDbEntity
    {
        Guid Id { get; set; }

        string Name { get; set; }

        DirectoryCrawlOptions Options { get; set; }

        DateTime LastAccessed { get; set; }

        string Notes { get; set; }

        DateTime CreationTime { get; set; }

        DateTime LastWriteTime { get; set; }

        DirectoryStatus Status { get; set; }

        ISubdirectory Parent { get; set; }

        IVolume Volume { get; set; }

        IEnumerable<IFile> Files { get; }

        IEnumerable<ISubdirectory> SubDirectories { get; }

        IEnumerable<IAccessError<ISubdirectory>> AccessErrors { get; }
    }
}
