using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamSubdirectory : ISubdirectory, IUpstreamDbEntity
    {
        new IUpstreamSubdirectory Parent { get; set; }

        new IUpstreamVolume Volume { get; set; }

        new IEnumerable<IUpstreamFile> Files { get; }

        new IEnumerable<IUpstreamSubdirectory> SubDirectories { get; }

        IEnumerable<IFileAction> FileActions { get; }

        IEnumerable<ISubdirectoryAction> SubdirectoryActions { get; }

        IEnumerable<ISubdirectoryAction> SubdirectoryActionSources { get; }
    }
}
