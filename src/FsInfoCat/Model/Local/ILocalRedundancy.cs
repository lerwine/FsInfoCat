using System;
using System.Collections.Generic;

namespace FsInfoCat.Model.Local
{
    public interface ILocalRedundancy : IRedundancy, ILocalModel
    {
        new IReadOnlyCollection<ILocalFile> Files { get; }
    }
}
