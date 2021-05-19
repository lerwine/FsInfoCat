using System.Collections.Generic;

namespace FsInfoCat.Model.Local
{
    public interface ILocalFile : IFile, ILocalModel
    {
        new IReadOnlyCollection<ILocalFileComparison> SourceComparisons { get; }

        new IReadOnlyCollection<ILocalFileComparison> TargetComparisons { get; }

        new ILocalSubDirectory Parent { get; }

        new ILocalRedundancy Redundancy { get; }
    }
}
