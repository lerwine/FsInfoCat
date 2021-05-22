using System.Collections.Generic;

namespace FsInfoCat.Local
{
    public interface ILocalSubdirectory : ISubdirectory, ILocalDbEntity
    {
        new ILocalSubdirectory Parent { get; set; }

        new ILocalVolume Volume { get; set; }

        new IEnumerable<ILocalFile> Files { get; }

        new IEnumerable<ILocalSubdirectory> SubDirectories { get; }
    }
}
