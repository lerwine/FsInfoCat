using System.Collections.Generic;

namespace FsInfoCat.Model.Local
{
    public interface ILocalRedundancy : IRedundancy
    {
        new IReadOnlyCollection<ILocalFile> Files { get; }
    }
}
