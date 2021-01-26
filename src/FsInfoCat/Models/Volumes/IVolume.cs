using System;
using FsInfoCat.Models.DB;

namespace FsInfoCat.Models.Volumes
{
    public interface IVolume : IVolumeInfo, IModficationAuditable
    {
        Guid VolumeID { get; set; }

        string Name { get; }

        Guid? HostDeviceID { get; set; }

        HostDevice Host { get; set; }

        string HostName { get; }

        string DisplayName { get; set; }

        bool IsInactive { get; set; }

        string Notes { get; set; }
    }
}
