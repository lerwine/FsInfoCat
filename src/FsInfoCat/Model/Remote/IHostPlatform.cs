using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model.Remote
{
    public interface IHostPlatform : IRemoteTimeStampedEntity, IValidatableObject
    {
        Guid Id { get; }
        string DisplayName { get; }
        PlatformType Type { get; }
        Guid? DefaultFsTypeId { get; }
        string Notes { get; }
        bool IsInactive { get; }

        IRemoteFileSystem DefaultFSType { get; }
        IReadOnlyCollection<IHostDevice> HostDevices { get; }
    }
}
