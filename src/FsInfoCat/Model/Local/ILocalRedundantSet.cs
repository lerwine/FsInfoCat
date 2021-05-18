using System.Collections.Generic;

namespace FsInfoCat.Model.Local
{
    public interface ILocalRedundantSet : IRedundantSet, ILocalModel
    {
        new IReadOnlyCollection<ILocalRedundancy> Redundancies { get; }
    }
}
