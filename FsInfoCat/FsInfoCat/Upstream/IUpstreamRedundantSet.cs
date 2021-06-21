using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamRedundantSet : IRedundantSet, IUpstreamDbEntity
    {
        new IUpstreamBinaryProperties BinaryProperties { get; set; }

        new IEnumerable<IUpstreamRedundancy> Redundancies { get; }
    }
}
