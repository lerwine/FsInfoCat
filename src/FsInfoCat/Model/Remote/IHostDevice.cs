using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model.Remote
{
    public interface IHostDevice : IRemoteTimeStampedEntity, IValidatableObject
    {
        Guid Id { get; }
        string DisplayName { get; }
        string MachineIdentifer { get; }
        string MachineName { get; }
        Guid PlatformId { get; }
        string Notes { get; }
        bool IsInactive { get; }

        IReadOnlyCollection<IRemoteVolume> Volumes { get; }
    }
}
