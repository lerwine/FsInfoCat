using System.Collections.Generic;

namespace FsInfoCat.Model.Remote
{
    public interface IRemoteSubDirectory : ISubDirectory, IRemoteTimeStampedEntity
    {
        new IRemoteSubDirectory Parent { get; }

        new IRemoteVolume Volume { get; }

        new IReadOnlyCollection<IRemoteFile> Files { get; }

        new IReadOnlyCollection<IRemoteSubDirectory> SubDirectories { get; }
    }
}
