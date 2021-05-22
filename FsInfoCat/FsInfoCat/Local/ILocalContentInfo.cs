using System.Collections.Generic;

namespace FsInfoCat.Local
{
    public interface ILocalContentInfo : IContentInfo, ILocalDbEntity
    {
        new IEnumerable<ILocalFile> Files { get; }

        new IEnumerable<ILocalRedundantSet> RedundantSets { get; }
    }
}
