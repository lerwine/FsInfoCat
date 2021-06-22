using System.Collections.Generic;

namespace FsInfoCat.Model.Upstream
{
    public interface IUpstreamFile : IFile, IUpstreamTimeStampedEntity
    {
        new IReadOnlyCollection<IUpstreamFileComparison> SourceComparisons { get; }

        new IReadOnlyCollection<IUpstreamFileComparison> TargetComparisons { get; }

        new IUpstreamSubDirectory Parent { get; }

        new IUpstreamContentInfo HashInfo { get; }

        new IUpstreamRedundancy Redundancy { get; }

    }
}
