using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamVolume : IVolume, IUpstreamDbEntity
    {
#warning Needs a setter
        IHostDevice HostDevice { get; }

        new IUpstreamFileSystem FileSystem { get; set; }

        new IUpstreamSubdirectory RootDirectory { get; }

        new IEnumerable<IAccessError<IUpstreamVolume>> AccessErrors { get; }
    }

}
