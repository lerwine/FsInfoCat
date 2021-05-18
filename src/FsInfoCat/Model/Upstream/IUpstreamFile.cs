using System.Collections.Generic;

namespace FsInfoCat.Model.Upstream
{
    public interface IUpstreamFile : IFile, IUpstreamTimeStampedEntity
    {
        new IReadOnlyCollection<IUpstreamFileComparison> Comparisons1 { get; }

        new IReadOnlyCollection<IUpstreamFileComparison> Comparisons2 { get; }

        new IUpstreamSubDirectory Parent { get; }

        new IUpstreamContentHash HashInfo { get; }

        new IUpstreamRedundancy Redundancy { get; }

    }
}
