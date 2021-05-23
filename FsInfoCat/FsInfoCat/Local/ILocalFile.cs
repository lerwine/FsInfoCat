using System.Collections.Generic;

namespace FsInfoCat.Local
{
    public interface ILocalFile : IFile, ILocalDbEntity
    {
        new ILocalContentInfo Content { get; set; }

        new ILocalSubdirectory Parent { get; set; }

        new IEnumerable<ILocalComparison> ComparisonSources { get; }

        new IEnumerable<ILocalComparison> ComparisonTargets { get; }

        new ILocalRedundancy Redundancy { get; }
    }
}
