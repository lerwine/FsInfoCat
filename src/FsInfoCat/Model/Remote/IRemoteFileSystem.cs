using System.Collections.Generic;

namespace FsInfoCat.Model.Remote
{
    public interface IRemoteFileSystem : IFileSystem, IRemoteTimeStampedEntity
    {
        new IReadOnlyCollection<IRemoteVolume> Volumes { get; }
        new IReadOnlyCollection<IRemoteSymbolicName> SymbolicNames { get; }
        new IRemoteSymbolicName DefaultSymbolicName { get; }
    }
}
