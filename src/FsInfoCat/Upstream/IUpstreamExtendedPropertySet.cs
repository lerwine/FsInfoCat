using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    [System.Obsolete]
    public interface IUpstreamExtendedPropertySet : IExtendedProperties, IUpstreamDbEntity
    {
        new IEnumerable<IUpstreamFile> Files { get; }
    }
}
