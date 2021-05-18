using System.Collections.Generic;

namespace FsInfoCat.Model.Upstream
{
    public interface IUpstreamSymbolicName : IFsSymbolicName, IUpstreamTimeStampedEntity
    {
        new IUpstreamFileSystem FileSystem { get; }
        new IReadOnlyCollection<IUpstreamFileSystem> DefaultFileSystems { get; }
    }
}
