using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    public interface IHostPlatform : IUpstreamDbEntity
    {
        string DisplayName { get; }

        PlatformType Type { get; }

        string Notes { get; }

        bool IsInactive { get; }

        IUpstreamFileSystem DefaultFsType { get; }

        IReadOnlyCollection<IHostDevice> HostDevices { get; }
    }

}
