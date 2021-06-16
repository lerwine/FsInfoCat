using System.Collections.Generic;

namespace FsInfoCat.Local
{
    public interface ILocalFile : IFile, ILocalDbFsItem
    {
        new ILocalContentInfo Content { get; set; }

        new ILocalExtendedProperties ExtendedProperties { get; set; }

        new ILocalSubdirectory Parent { get; set; }

        new ILocalRedundancy Redundancy { get; }

        new IEnumerable<ILocalComparison> ComparisonSources { get; }

        new IEnumerable<ILocalComparison> ComparisonTargets { get; }

        new IEnumerable<IAccessError<ILocalFile>> AccessErrors { get; }
    }
}
