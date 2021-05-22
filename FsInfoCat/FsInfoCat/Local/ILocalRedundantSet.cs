using System.Collections.Generic;

namespace FsInfoCat.Local
{
    public interface ILocalRedundantSet : IRedundantSet, ILocalDbEntity
    {
        new ILocalContentInfo ContentInfo { get; set; }

        new IEnumerable<ILocalRedundancy> Redundancies { get; }
    }
}
