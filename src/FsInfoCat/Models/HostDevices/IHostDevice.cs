using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models.HostDevices
{
    public interface IHostDevice : IHostDeviceReg, IModficationAuditable
    {
        string Name { get; }

        bool IsInactive { get; set; }

        string Notes { get; set; }
    }
}
