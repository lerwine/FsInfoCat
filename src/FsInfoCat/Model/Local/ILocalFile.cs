using System.Collections.Generic;

namespace FsInfoCat.Model.Local
{
    public interface ILocalFile : IFile, ILocalModel
    {
        new IReadOnlyCollection<ILocalFileComparison> Comparisons1 { get; }

        new IReadOnlyCollection<ILocalFileComparison> Comparisons2 { get; }

        new ILocalSubDirectory Parent { get; }

        new ILocalRedundancy Redundancy { get; }
    }
}
