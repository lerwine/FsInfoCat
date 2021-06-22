using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamBinaryPropertySet : IBinaryPropertySet, IUpstreamDbEntity
    {
        new IEnumerable<IUpstreamFile> Files { get; }

        new IEnumerable<IUpstreamRedundantSet> RedundantSets { get; }
    }
}
