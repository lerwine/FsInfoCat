using System.Collections.Generic;

namespace FsInfoCat.Model.Upstream
{
    public interface IUpstreamHashCalculation : IHashCalculation, IUpstreamTimeStampedEntity
    {
        new IReadOnlyCollection<IUpstreamFile> Files { get; }
    }
}
