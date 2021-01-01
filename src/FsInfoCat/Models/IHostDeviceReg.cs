using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public interface IHostDeviceReg
    {
        Guid HostDeviceID { get; set; }

        string DisplayName { get; set; }

        /// <summary>
        /// Unique identifier for the machine; usually aa SID string.
        /// </summary>
        string MachineIdentifer { get; set; }

        /// <summary>
        /// Network name of machine.
        /// </summary>
        string MachineName { get; set; }

        bool IsWindows { get; set; }

        /// <summary>
        /// Determines whether to allow crawling.
        /// </summary>
        /// <value>True if crawling is allowed; otherwise false to indicate that crawling is not allowed.</value>
        bool AllowCrawl { get; set; }
    }
}
