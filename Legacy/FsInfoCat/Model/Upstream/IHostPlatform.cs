using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model.Upstream
{
    public interface IHostPlatform : IUpstreamTimeStampedEntity, IValidatableObject
    {
        Guid Id { get; }
        string DisplayName { get; }
        PlatformType Type { get; }
        string Notes { get; }
        bool IsInactive { get; }

        IUpstreamFileSystem DefaultFsType { get; }
        IReadOnlyCollection<IHostDevice> HostDevices { get; }
    }
}
