using System.Collections.Generic;

namespace FsInfoCat.Model.Upstream
{
    public interface IUpstreamContentInfo : IContentInfo, IUpstreamTimeStampedEntity
    {
        new IReadOnlyCollection<IUpstreamFile> Files { get; }
    }
}
