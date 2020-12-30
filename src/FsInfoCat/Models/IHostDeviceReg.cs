using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public interface IHostDeviceReg
    {
        Guid HostID { get; set; }

        string DisplayName { get; set; }

        string MachineIdentifer { get; set; }

        string MachineName { get; set; }

        bool IsWindows { get; set; }
    }
}
