using System.Collections.Generic;

namespace FsInfoCat.Model.Local
{
    public interface ILocalContentInfo : IContentInfo, ILocalModel
    {
        new IReadOnlyCollection<ILocalFile> Files { get; }
    }
}
