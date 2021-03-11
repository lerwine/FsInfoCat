using FsInfoCat.Models.DB;
using FsInfoCat.Models.HostDevices;
using System;

namespace FsInfoCat.Models.Volumes
{
    public interface IVolume : IVolumeInfo, IModficationAuditable
    {
        Guid VolumeID { get; set; }

        string Name { get; }

        Guid? HostDeviceID { get; set; }

        IHostDevice Host { get; set; }

        string HostName { get; }

        string DisplayName { get; set; }

        bool IsInactive { get; set; }

        string Notes { get; set; }
    }

    public interface IVolume<H> : IVolume
        where H : IHostDevice
    {
        new H Host { get; set;  }
    }
}
