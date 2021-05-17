using System.Collections.Generic;

namespace FsInfoCat.Model.Remote
{
    public interface IRemoteSymbolicName : IFsSymbolicName, IRemoteTimeStampedEntity
    {
        new IRemoteFileSystem FileSystem { get; }
        new IReadOnlyCollection<IRemoteFileSystem> DefaultFileSystems { get; }
    }
}
