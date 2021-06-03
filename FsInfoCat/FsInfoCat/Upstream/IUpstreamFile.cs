using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamFile : IFile, IUpstreamDbEntity
    {
        new IUpstreamContentInfo Content { get; set; }

        new IUpstreamExtendedProperties ExtendedProperties { get; set; }

        new IUpstreamSubdirectory Parent { get; set; }

        IEnumerable<IFileAction> FileActions { get; }

        new IEnumerable<IUpstreamComparison> ComparisonSources { get; }

        new IEnumerable<IUpstreamComparison> ComparisonTargets { get; }

        new IUpstreamRedundancy Redundancy { get; }
    }
}
