using System.Collections.Generic;

namespace FsInfoCat
{
    public interface IContentInfo : IDbEntity
    {
        long Length { get; set; }

        IReadOnlyList<byte> Hash { get; }

        IEnumerable<IFile> Files { get; }

        IEnumerable<IRedundantSet> RedundantSets { get; }
    }
}
