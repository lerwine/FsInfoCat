using System.Collections.Generic;

namespace FsInfoCat.Model.Upstream
{
    public interface IUpstreamSubDirectory : ISubDirectory, IUpstreamTimeStampedEntity
    {
        new IUpstreamSubDirectory Parent { get; }

        IDirectoryRelocateTask SourceRelocationTask { get; }

        new IUpstreamVolume Volume { get; }

        new IReadOnlyCollection<IUpstreamFile> Files { get; }

        new IReadOnlyCollection<IUpstreamSubDirectory> SubDirectories { get; }
    }
}
