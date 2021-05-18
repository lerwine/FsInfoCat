using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model.Upstream
{
    public interface IHostDevice : IUpstreamTimeStampedEntity, IValidatableObject
    {
        Guid Id { get; }
        string DisplayName { get; }
        string MachineIdentifer { get; }
        string MachineName { get; }
        IHostPlatform Platform { get; }
        string Notes { get; }
        bool IsInactive { get; }

        IReadOnlyCollection<IUpstreamVolume> Volumes { get; }
    }
}
