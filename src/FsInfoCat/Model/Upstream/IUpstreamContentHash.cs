using System.Collections.Generic;

namespace FsInfoCat.Model.Upstream
{
    public interface IUpstreamContentHash : IContentHash, IUpstreamTimeStampedEntity
    {
        new IReadOnlyCollection<IUpstreamFile> Files { get; }
    }
}
