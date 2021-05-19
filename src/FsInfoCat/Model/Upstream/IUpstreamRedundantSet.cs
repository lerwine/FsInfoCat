using System.Collections.Generic;

namespace FsInfoCat.Model.Upstream
{
    public interface IUpstreamRedundantSet : IRedundantSet, IUpstreamTimeStampedEntity
    {
        new IReadOnlyCollection<IUpstreamRedundancy> Redundancies { get; }
    }
}
