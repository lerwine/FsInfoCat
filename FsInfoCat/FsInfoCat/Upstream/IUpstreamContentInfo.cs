using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamContentInfo : IContentInfo, IUpstreamDbEntity
    {
        new IEnumerable<IUpstreamFile> Files { get; }

        new IEnumerable<IUpstreamRedundantSet> RedundantSets { get; }
    }
}
