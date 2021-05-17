using System.Collections.Generic;

namespace FsInfoCat.Model.Remote
{
    public interface IRemoteRedundancy : IRedundancy, IRemoteTimeStampedEntity
    {
        new IReadOnlyCollection<IRemoteFile> Files { get; }
    }
}
