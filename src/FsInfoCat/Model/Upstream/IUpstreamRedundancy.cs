using System.Collections.Generic;

namespace FsInfoCat.Model.Upstream
{
    public interface IUpstreamRedundancy : IRedundancy, IUpstreamTimeStampedEntity
    {
        new IReadOnlyCollection<IUpstreamFile> Files { get; }
    }
}
