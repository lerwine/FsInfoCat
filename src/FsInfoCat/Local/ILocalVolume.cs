using System.Collections.Generic;

namespace FsInfoCat.Local
{
    public interface ILocalVolume : IVolume, ILocalDbEntity
    {
        new ILocalFileSystem FileSystem { get; set; }

        new ILocalSubdirectory RootDirectory { get; }

        new IEnumerable<IAccessError<ILocalVolume>> AccessErrors { get; }
    }
}
