using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamFileSystem : IFileSystem, IUpstreamDbEntity
    {
        new IEnumerable<IUpstreamVolume> Volumes { get; }

        new IEnumerable<IUpstreamSymbolicName> SymbolicNames { get; }
    }
}
