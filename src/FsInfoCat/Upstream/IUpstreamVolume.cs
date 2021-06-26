using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamVolume : IVolume, IUpstreamDbEntity
    {
        IHostDevice HostDevice { get; set; }

        new IUpstreamFileSystem FileSystem { get; set; }

        new IUpstreamSubdirectory RootDirectory { get; }

        new IEnumerable<IAccessError<IUpstreamVolume>> AccessErrors { get; }
    }

}
