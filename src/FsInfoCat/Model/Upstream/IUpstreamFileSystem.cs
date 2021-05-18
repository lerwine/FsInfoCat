using System.Collections.Generic;

namespace FsInfoCat.Model.Upstream
{
    public interface IUpstreamFileSystem : IFileSystem, IUpstreamTimeStampedEntity
    {
        new IReadOnlyCollection<IUpstreamVolume> Volumes { get; }
        new IReadOnlyCollection<IUpstreamSymbolicName> SymbolicNames { get; }
        new IUpstreamSymbolicName DefaultSymbolicName { get; }
    }
}
