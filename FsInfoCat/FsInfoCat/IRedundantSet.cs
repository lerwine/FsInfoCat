using System.Collections.Generic;

namespace FsInfoCat
{
    public interface IRedundantSet : IDbEntity
    {
        string Reference { get; set; }

        string Notes { get; set; }

        IContentInfo ContentInfo { get; set; }

        IEnumerable<IRedundancy> Redundancies { get; }
    }
}
