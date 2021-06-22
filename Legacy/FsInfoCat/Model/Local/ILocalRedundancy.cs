using System;
using System.Collections.Generic;

namespace FsInfoCat.Model.Local
{
    public interface ILocalRedundancy : IRedundancy, ILocalModel
    {
        new ILocalFile TargetFile { get; }

        new ILocalRedundantSet RedundantSet { get; }
    }
}
