using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamRedundantSet : IRedundantSet, IUpstreamDbEntity
    {
        new IUpstreamContentInfo ContentInfo { get; set; }

        new IEnumerable<IUpstreamRedundancy> Redundancies { get; }
    }
}
