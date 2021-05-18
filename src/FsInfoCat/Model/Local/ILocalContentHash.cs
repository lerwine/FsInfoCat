using System.Collections.Generic;

namespace FsInfoCat.Model.Local
{
    public interface ILocalContentHash : IContentHash, ILocalModel
    {
        new IReadOnlyCollection<ILocalFile> Files { get; }
    }
}
