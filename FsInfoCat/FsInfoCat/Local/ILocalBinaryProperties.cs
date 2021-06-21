using System.Collections.Generic;

namespace FsInfoCat.Local
{
    public interface ILocalBinaryProperties : IBinaryProperties, ILocalDbEntity
    {
        new IEnumerable<ILocalFile> Files { get; }

        new IEnumerable<ILocalRedundantSet> RedundantSets { get; }
    }
}
