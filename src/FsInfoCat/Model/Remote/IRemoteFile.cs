using System.Collections.Generic;

namespace FsInfoCat.Model.Remote
{
    public interface IRemoteFile : IFile, IRemoteTimeStampedEntity
    {
        new IReadOnlyCollection<IRemoteFileComparison> Comparisons1 { get; }

        new IReadOnlyCollection<IRemoteFileComparison> Comparisons2 { get; }

        new IRemoteSubDirectory Parent { get; }
        new IRemoteHashCalculation HashCalculation { get; }

        new IRemoteRedundancy Redundancy { get; }

    }
}
