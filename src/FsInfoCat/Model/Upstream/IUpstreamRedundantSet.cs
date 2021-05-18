using System.Collections.Generic;

namespace FsInfoCat.Model.Upstream
{
    public interface IUpstreamRedundantSet : IRedundantSet
    {
        new IReadOnlyCollection<IUpstreamRedundancy> Redundancies { get; }
    }
}
