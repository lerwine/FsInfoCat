using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamRedundantSet : IRedundantSet, IUpstreamDbEntity
    {
        new IUpstreamBinaryPropertySet BinaryProperties { get; set; }

        new IEnumerable<IUpstreamRedundancy> Redundancies { get; }
    }
}
