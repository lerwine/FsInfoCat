using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models.HostDevices
{
    public interface IHostDeviceReg
    {
        Guid HostDeviceID { get; set; }

        string DisplayName { get; set; }

        /// <summary>
        /// Unique identifier for the machine.
        /// </summary>
        /// <remarks>For Windows machines, this is the machine SID string. For Linux machines, this is the GUID string from /etc/machine-id</remarks>
        string MachineIdentifer { get; set; }

        /// <summary>
        /// Network name of machine.
        /// </summary>
        string MachineName { get; set; }

        // TODO: Change to an enum or formatted string that will distinguish host types
        /// <summary>
        /// Indicates if the host machine is a windows machine.
        /// </summary>
        /// <value></value>
        bool IsWindows { get; set; }

        /// <summary>
        /// Determines whether to allow crawling.
        /// </summary>
        /// <value>True if crawling is allowed; otherwise false to indicate that crawling is not allowed.</value>
        bool AllowCrawl { get; set; }
    }
}
