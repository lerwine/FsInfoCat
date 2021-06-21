using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    [System.Obsolete]
    public interface IUpstreamExtendedProperties : IExtendedProperties, IUpstreamDbEntity
    {
        new IEnumerable<IUpstreamFile> Files { get; }
    }
}
