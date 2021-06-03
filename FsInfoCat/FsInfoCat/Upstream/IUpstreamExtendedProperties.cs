using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamExtendedProperties : IExtendedProperties, IUpstreamDbEntity
    {
        new IEnumerable<IUpstreamFile> Files { get; }
    }
}
