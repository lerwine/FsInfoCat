using System.Collections.Generic;

namespace FsInfoCat.Model.Local
{
    public interface ILocalSubDirectory : ISubDirectory
    {
        new ILocalSubDirectory Parent { get; }

        new ILocalVolume Volume { get; }

        new IReadOnlyCollection<ILocalFile> Files { get; }

        new IReadOnlyCollection<ILocalSubDirectory> SubDirectories { get; }
    }
}
