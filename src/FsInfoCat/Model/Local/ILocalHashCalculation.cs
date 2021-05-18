using System.Collections.Generic;

namespace FsInfoCat.Model.Local
{
    public interface ILocalHashCalculation : IHashCalculation, ILocalModel
    {
        new IReadOnlyCollection<ILocalFile> Files { get; }
    }
}
