using System.Collections.Generic;

namespace FsInfoCat.Model.Remote
{
    public interface IRemoteHashCalculation : IHashCalculation, IRemoteTimeStampedEntity
    {
        new IReadOnlyCollection<IRemoteFile> Files { get; }
    }
}
