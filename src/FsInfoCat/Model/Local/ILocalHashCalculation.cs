using System.Collections.Generic;

namespace FsInfoCat.Model.Local
{
    public interface ILocalHashCalculation : IHashCalculation
    {
        new IReadOnlyCollection<ILocalFile> Files { get; }
    }
}
