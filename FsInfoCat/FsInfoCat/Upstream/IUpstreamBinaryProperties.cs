using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamBinaryProperties : IBinaryProperties, IUpstreamDbEntity
    {
        new IEnumerable<IUpstreamFile> Files { get; }

        new IEnumerable<IUpstreamRedundantSet> RedundantSets { get; }
    }
}
